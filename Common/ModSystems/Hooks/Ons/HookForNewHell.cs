// Code by 𝒜𝑒𝓇𝒾𝓈
using Avalon.Biomes;
using Microsoft.Xna.Framework;
using MonoMod.RuntimeDetour;
using Synergia.Common.Biome;
using Synergia.Common.ModSystems.WorldGens;
using Synergia.Content.NPCs;
using Synergia.Helpers;
using System.Reflection;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using ValhallaMod.NPCs.TownNPCs;

namespace Synergia.Common.ModSystems.Hooks.Ons {
    public class HookForNewHell : ModSystem {
        Hook avalonBg;

        delegate bool Orig_IsActive(CaesiumBlastplains caesiumBlastplains, Player player);
        delegate bool Get_IsActiveDetour(Orig_IsActive orig, CaesiumBlastplains caesiumBlastplains, Player player);

        public override void Load() {
            MethodInfo info = typeof(CaesiumBlastplains).GetMethod(nameof(CaesiumBlastplains.IsBiomeActive));
            avalonBg = new Hook(info, (Get_IsActiveDetour)NewLogic); // Disabled biome in Avalon;
            On_WorldGen.PlaceTile += On_WorldGen_PlaceTile; // Disabled Magic Ice if use item Ice Rood;
            On_NPC.checkDead += On_NPC_checkDead; // Disabled lava if lava slime dead;
            On_Projectile.AI_007_GrapplingHooks += On_Projectile_AI_007_GrapplingHooks; // Disabled hook;
            On_Mount.SetMount += On_Mount_SetMount; // Disabled mount;
            On_Main.DrawNPCHeadFriendly += On_Main_DrawNPCHeadFriendly;
        }
        void On_Main_DrawNPCHeadFriendly(On_Main.orig_DrawNPCHeadFriendly orig, Entity theNPC, byte alpha, float headScale, SpriteEffects dir, int townHeadId, float x, float y) {
            if (theNPC is NPC npc) {
                if (npc.active && npc.townNPC) {
                    int index = - 1;
                    if (npc.type == NPCType<Dwarf>()) {
                        index = TownNPCProfiles.GetHeadIndexSafe(npc);
                    }
                    if (npc.type == NPCType<HellDwarf>()) {
                        index = TownNPCProfiles.GetHeadIndexSafe(npc);
                    }
                    Main.TownNPCHeadRenderer.DrawWithOutlines(theNPC, townHeadId, new Vector2(x, y), new Color(alpha, alpha, alpha, alpha), 0f, headScale, dir);
                }
            }
        }
        bool NewLogic(Orig_IsActive orig, CaesiumBlastplains caesiumBlastplains, Player player) => false;
        bool On_WorldGen_PlaceTile(On_WorldGen.orig_PlaceTile orig, int i, int j, int Type, bool mute, bool forced, int plr, int style) {
            bool arena = WorldHelper.CheckBiomeTile(i, j, 199, 119, BaseWorldGens.HellArenaPositionX - 199, BaseWorldGens.HellArenaPositionY - 119);
            bool village = WorldHelper.CheckBiomeTile(i, j, 281, 119, BaseWorldGens.HellVillageX - 280, BaseWorldGens.HellVillageY - 119);
            bool lake = WorldHelper.CheckBiomeTile(i, j, 215, 119, BaseWorldGens.HellLakeX - 236, BaseWorldGens.HellLakeY - 119);
            if ((arena || village || lake) && Type == 127) {
                SoundEngine.PlaySound(SoundID.Item27, new Vector2(i * 16, j * 16));
                Dust.NewDust(new Vector2(i * 16, j * 16), 16, 16, DustID.IceRod);
                return true;
            }
            else { return orig(i, j, Type, mute, forced, plr, style); }
        }
        void On_NPC_checkDead(On_NPC.orig_checkDead orig, NPC npc) {
            orig(npc);
            if ((npc.realLife >= 0 && npc.realLife != npc.whoAmI) || npc.life > 0) {
                return;
            }
            if (npc.target < 0 || npc.target >= Main.maxPlayers) {
                return;
            }
            Player player = Main.player[npc.target];
            if (player.InModBiome<NewHell>()) {
                if (npc.type == NPCID.LavaSlime) {
                    int tileX = (int)(npc.Center.X / 16f);
                    int tileY = (int)(npc.Center.Y / 16f);
                    if (!WorldGen.InWorld(tileX, tileY)) {
                        return;
                    }
                    Tile tile = Main.tile[tileX, tileY];
                    if (tile.LiquidAmount > 0) {
                        tile.LiquidAmount = 0;
                        WorldGen.SquareTileFrame(tileX, tileY); // ?
                    }
                }
            }
        }
        void On_Projectile_AI_007_GrapplingHooks(On_Projectile.orig_AI_007_GrapplingHooks orig, Projectile self) {
            Player player = Main.player[self.owner];
            if (player.InModBiome<NewHell>()) { self.Kill(); } else { orig(self); }
        }
        void On_Mount_SetMount(On_Mount.orig_SetMount orig, Mount self, int m, Player mountedPlayer, bool faceLeft) {
            if (mountedPlayer.InModBiome<NewHell>()) { self.Dismount(mountedPlayer); self.Reset(); return; }
            else { orig(self, m, mountedPlayer, faceLeft); }
        }
        public override void Unload() {
            avalonBg?.Dispose();
            avalonBg = null;
            On_WorldGen.PlaceTile -= On_WorldGen_PlaceTile;
            On_NPC.checkDead -= On_NPC_checkDead;
            On_NPC.checkDead -= On_NPC_checkDead;
            On_Mount.SetMount -= On_Mount_SetMount;
        }
    }
}