using Bismuth.Utilities.ModSupport;
using Synergia.Dataset;
using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader.IO;
using Terraria.ID;
using Terraria.Audio;
using Terraria.Localization;
using static Synergia.Common.ModSystems.Hooks.Ons.HookForQuest;

namespace Synergia.Helpers
{
    public static partial class SynegiaHelper
    {

#pragma warning disable CS8632

        #region Source
        public static IEntitySource GetSource(object? value)
        {
            if (value is null)
            {
                return Main.LocalPlayer.GetSource_FromThis();
            }
            return value switch
            {
                Item item => item.GetSource_FromThis(),
                NPC npc => npc.GetSource_FromThis(),
                Player player => player.GetSource_FromThis(),
                Projectile proj => proj.GetSource_FromThis(),
                _ => Main.LocalPlayer.GetSource_FromThis()
            };
        }
        #endregion

        public static void SpawnNPC(int posX, int posY, int npcType, IEntitySource? source = null, int start = 0, float ai0 = 0f, float ai1 = 0f, float ai2 = 0f, float ai3 = 0f, int Target = 255)
        {
            IEntitySource s = source ?? Main.LocalPlayer.GetSource_FromThis();
            if (!NPC.AnyNPCs(npcType))
            {
                NPC.NewNPC(s, posX, posY, npcType, start, ai0, ai1, ai2, ai3, Target);
            }
        }

        public static void EzSave<T>(TagCompound tag, string name, ref T type) => tag[name] = type;
        public static void EzLoad<T>(TagCompound tag, string name, ref T type) => type = tag.Get<T>(name);

        public static bool TryGetTalkNPC(Player player, out NPC npc)
        {
            npc = null;

            int index = player.talkNPC;
            if (index < 0 || index >= Main.npc.Length)
            {
                return false;
            }

            npc = Main.npc[index];
            return npc.active;
        }

        public static bool TryGetQuest(Player player, out NPC npc, out QuestData questData, out IQuest quest)
        {
            quest = null;
            questData = default;

            if (!TryGetTalkNPC(player, out npc)) { return false; }
            if (!NpcQuestKeys.TryGetValue(npc.type, out questData)) { return false; }
            if (string.IsNullOrEmpty(questData.QuestKey)) { return false; }

            quest = QuestRegistry.GetAvailableQuests(player, questData.QuestKey).FirstOrDefault();

            return quest != null;
        }

//From victima, for malebolge
        public static bool ClosestNPC(ref NPC target, Vector2 position, float maxDistance = 0, int type = 0,
            bool ignoreTiles = false, bool withoutRepeat = false, NPC[] hittedNPC = null, int maxHittedNPC = -1,
            float[] npcAI = null, int whoAmI = -1)
        {
            bool foundTarget = false;

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                if (npc == null || !npc.active || npc.life <= 0 || !npc.CanBeChasedBy())
                    continue;

                float distance = Vector2.Distance(position, npc.Center);

                if ((type != 0 && npc.type != type) ||
                    (maxDistance > 0 && distance >= maxDistance) ||
                    (!ignoreTiles && !Collision.CanHit(position, 0, 0, npc.Center, 0, 0)) ||
                    (whoAmI != -1 && npc.whoAmI == whoAmI))
                    continue;

                bool skip = false;

                if (withoutRepeat && hittedNPC != null)
                {
                    for (int j = 0; j < maxHittedNPC; j++)
                    {
                        if (hittedNPC[j] == npc)
                        {
                            skip = true;
                            break;
                        }
                    }
                }

                if (npcAI != null)
                {
                    for (int g = 0; g <= 3; g++)
                    {
                        if (npcAI[g] != -1 && npcAI[g] != npc.ai[g])
                        {
                            skip = true;
                            break;
                        }
                    }
                }

                if (!skip)
                {
                    target = npc;
                    foundTarget = true;
                    maxDistance = distance;
                }
            }

            return foundTarget;
        }

        public static bool ClosestProj(ref Projectile target, Vector2 position, float maxDistance = 0, int type = 0,
            bool ignoreTiles = false, int whoAmI = -1, float[] projAI = null)
        {
            bool foundTarget = false;

            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile proj = Main.projectile[i];

                float distance = Vector2.Distance(position, proj.Center);

                if ((type != 0 && proj.type != type) ||
                    (maxDistance > 0 && distance >= maxDistance) ||
                    (!ignoreTiles && !Collision.CanHit(position, 0, 0, proj.Center, 0, 0)) ||
                    (whoAmI != -1 && proj.whoAmI == whoAmI))
                    continue;

                bool skip = false;

                if (projAI != null)
                {
                    for (int g = 0; g <= 1; g++)
                    {
                        if (projAI[g] != -1 && projAI[g] != proj.ai[g])
                        {
                            skip = true;
                            break;
                        }
                    }
                }

                if (!skip)
                {
                    target = proj;
                    foundTarget = true;
                    maxDistance = distance;
                }
            }

            return foundTarget;
        }

        public static bool IsOnPlatformNPC(this NPC npc, Vector2 offset)
        {
            int tileY = (int)((npc.Bottom.Y + offset.Y) / 16);
            int tileX1 = (int)((npc.Left.X - offset.X) / 16);
            int tileX2 = (int)((npc.Right.X + offset.X) / 16);

            for (int tileX = tileX1; tileX < tileX2; tileX++)
            {
                Tile tile = Framing.GetTileSafely(tileX, tileY);

                if (tile.HasTile &&
                    (Main.tileSolidTop[tile.TileType] || TileID.Sets.Platforms[tile.TileType]))
                {

                    if (!Main.tileSolidTop[tile.TileType] &&
                        Main.tileSolid[tile.TileType] &&
                        !TileID.Sets.Platforms[tile.TileType])
                    {
                        return false;
                    }

                    return true;
                }
            }

            return false;
        }

        public static bool HasTileOnSide(this NPC npc, int side, Vector2 offset, bool ignoreTopSolid = false)
        {
            if (side > 4) side = 1;

            int x1 = (int)((npc.Left.X - offset.X) / 16);
            int x2 = (int)((npc.Right.X + offset.X) / 16);
            int y1 = (int)((npc.Top.Y - offset.Y) / 16);
            int y2 = (int)((npc.Bottom.Y + offset.Y) / 16);

            switch (side)
            {
                case 1:
                    for (int y = y1; y < y2; y++)
                        if (CheckTile(x1, y, ignoreTopSolid)) return true;
                    break;

                case 2:
                    for (int y = y1; y < y2; y++)
                        if (CheckTile(x2, y, ignoreTopSolid)) return true;
                    break;

                case 3:
                    for (int x = x1; x < x2; x++)
                        if (CheckTile(x, y1, ignoreTopSolid)) return true;
                    break;

                case 4:
                    for (int x = x1; x < x2; x++)
                        if (CheckTile(x, y2, ignoreTopSolid)) return true;
                    break;
            }

            return false;
        }

        private static bool CheckTile(int x, int y, bool ignoreTopSolid)
        {
            Tile tile = Framing.GetTileSafely(x, y);

            if (!tile.HasTile) return false;

            if (ignoreTopSolid)
                return Main.tileSolid[tile.TileType] && !Main.tileSolidTop[tile.TileType];

            return Main.tileSolid[tile.TileType];
        }

        public static void DamageNPC(NPC npc, int dmg, float kb, int dir, Entity source,
            bool variation = true, bool ignoreDefense = false, bool crit = false, Item item = null)
        {
            if (npc.dontTakeDamage || (npc.immortal && npc.type != NPCID.TargetDummy))
                return;

            if (ignoreDefense)
                dmg += (int)(npc.defense * 0.5f);

            if (variation)
                dmg = Main.DamageVar(dmg);

            var hit = new NPC.HitInfo
            {
                Damage = dmg,
                Knockback = kb,
                HitDirection = dir,
                Crit = crit
            };

            npc.StrikeNPC(hit);

            if (Main.netMode != NetmodeID.SinglePlayer)
            {
                NetMessage.SendData(MessageID.DamageNPC, -1, -1, null,
                    npc.whoAmI, 1, kb, dir, dmg);
            }
        }

        public static bool DamagePlayer(ref Player player, int damage, int iFrame = 0, bool text = true, bool sound = true)
        {
            if (player.statLife <= damage) return false;

            player.statLife -= damage;

            if (text)
            {
                CombatText.NewText(player.Hitbox, CombatText.LifeRegen, damage, true);
            }

            if (sound)
            {
                SoundEngine.PlaySound(SoundID.PlayerHit, player.position);
            }

            player.immuneTime = iFrame;
            return true;
        }

        public static Vector2 PolarVector(float theta, float radius)
        {
            return new Vector2((float)Math.Cos(theta), (float)Math.Sin(theta)) * radius;
        }
    }
}