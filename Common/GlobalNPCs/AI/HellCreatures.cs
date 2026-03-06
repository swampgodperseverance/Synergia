using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using System;
using System.IO;
using System.Collections.Generic;

namespace Synergia.Common.GlobalNPCs.AI
{
    public class InfernalBoneSerpent : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        private int dashCooldown;
        private int chargeTimer;
        private bool charging;
        bool dashing;

        public override void SendExtraAI(NPC npc, BitWriter bitWriter, BinaryWriter binaryWriter) {
            if(Main.netMode == 0) return;
            bitWriter.WriteBit(charging);
            bitWriter.WriteBit(dashing);
            binaryWriter.Write(dashCooldown);
            binaryWriter.Write(chargeTimer);
        }
        public override void ReceiveExtraAI(NPC npc, BitReader bitReader, BinaryReader binaryReader) {
            if(Main.netMode == 0) return;
            charging = bitReader.ReadBit();
            dashing = bitReader.ReadBit();
            dashCooldown = binaryReader.ReadInt32();
            chargeTimer = binaryReader.ReadInt32();
        }
        public override bool AppliesToEntity(NPC npc, bool lateInstatiation) => npc.type == NPCID.BoneSerpentHead || npc.type == NPCID.BoneSerpentBody || npc.type == NPCID.BoneSerpentTail;

        public override void AI(NPC npc)
        {
            if (npc.type != NPCID.BoneSerpentHead)
                return;

            Player target = Main.player[npc.target];
            if (!target.active || target.dead)
                return;

            float distance = Vector2.Distance(npc.Center, target.Center);

            if (dashCooldown > 0)
                dashCooldown--;

            if (!charging && !dashing && dashCooldown <= 0)
            {
                if (distance < 700f && Main.rand.NextBool(180))
                {
                    charging = true;
                    chargeTimer = 40;
                }
            }

            if (charging)
            {
                Vector2 direction = target.Center - npc.Center;
                direction.Normalize();
                npc.velocity += direction * (chargeTimer / 40f);
                npc.velocity *= 0.9f;
                chargeTimer--;

                Lighting.AddLight(npc.Center, 1.2f, 0.4f, 0.05f);

                if (chargeTimer <= 0)
                {
                    charging = false;
                    dashing = true;

                    npc.velocity = npc.oldVelocity = direction * 22f;

                    dashCooldown = 220;
                }
            }

            if (dashing)
            {
                npc.oldVelocity *= 0.98f;
                npc.velocity = npc.oldVelocity;

                if (Main.rand.NextBool(2))
                {
                    Dust d = Dust.NewDustDirect(
                        npc.position,
                        npc.width,
                        npc.height,
                        DustID.Torch
                    );

                    d.noGravity = true;
                    d.velocity *= 0.3f;
                    d.scale = 1.5f;
                }

                if (npc.velocity.Length() < 5f)
                    dashing = false;
            }
        }

        public override void PostDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (npc.type == NPCID.BoneSerpentHead ? dashing : Main.npc[npc.realLife].GetGlobalNPC<InfernalBoneSerpent>().dashing)
            {
                Texture2D texture = Terraria.GameContent.TextureAssets.Npc[npc.type].Value;
                Vector2 origin = texture.Size() / 2f;

                Color lavaColor = Color.Lerp(Color.OrangeRed, Color.Yellow, 0.5f) with { A = 0 };

                lavaColor *= MathHelper.Clamp(((npc.type == NPCID.BoneSerpentHead ? npc : Main.npc[npc.realLife]).velocity.Length() - 5f) / 5f, 0f, 1f);

                for (int i = 0; i < 2; i++) spriteBatch.Draw(texture, npc.Center - Vector2.UnitY * 4 - screenPos, npc.frame, lavaColor, npc.rotation, origin, npc.scale, SpriteEffects.None, 0f);
            }
        }
    }

    
    public class DemonTeleport : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        public override bool AppliesToEntity(NPC npc, bool lateInstatiation) => npc.type == NPCID.Demon || npc.type == NPCID.VoodooDemon || npc.type == NPCID.RedDevil || npc.type == ModLoader.GetMod("Consolaria").Find<ModNPC>("ArchDemon").Type;

        private int teleportCooldown = 300;

        public override void AI(NPC npc)
        {

            if (npc.type != NPCID.RedDevil && teleportCooldown > 0)
            {
                teleportCooldown--;
                return;
            }

            Player target = Main.player[npc.target];
            if (!target.active || target.dead)
                return;
            else if(npc.type == NPCID.RedDevil)
            {
                if(npc.ai[0] == 180f) TeleportDemon(npc, target);
                else if(npc.ai[0] < 180f) npc.velocity *= 0.8f;
                if(npc.ai[0] > 220f) {
                    npc.velocity = npc.oldVelocity;
                    npc.direction = npc.oldDirection;
                    Dust d = Dust.NewDustDirect(npc.position, npc.width, npc.height, DustID.Shadowflame);
                    d.velocity = npc.velocity;
                    d.scale = Main.rand.NextFloat(1.2f, 1.8f);
                    d.noGravity = true;
                }
                else if(npc.ai[0] == 220f) npc.velocity = Vector2.Normalize(target.Center - npc.Center) * 10f;
            }
            else if (Main.rand.NextBool(npc.life + 200) && (npc.type == NPCID.Demon || npc.type == NPCID.VoodooDemon ? npc.ai[0] > 100f : npc.dontTakeDamage))
            {
                TeleportDemon(npc, target);
                teleportCooldown = 180;
            }
        }

        public override void SendExtraAI(NPC npc, BitWriter bitWriter, BinaryWriter binaryWriter) {
            if(Main.netMode > 0) binaryWriter.Write(teleportCooldown);
        }
        public override void ReceiveExtraAI(NPC npc, BitReader bitReader, BinaryReader binaryReader) {
            if(Main.netMode > 0) teleportCooldown = binaryReader.ReadInt32();
        }

        private void TeleportDemon(NPC npc, Player target)
        {
            for (int i = 0; i < 30; i++)
            {
                Dust d = Dust.NewDustDirect(npc.position, npc.width, npc.height, npc.type == NPCID.Demon || npc.type == NPCID.VoodooDemon || npc.type == NPCID.RedDevil ? DustID.Shadowflame : DustID.FlameBurst);
                d.velocity = new Vector2(Main.rand.NextFloat(-6f, 6f), Main.rand.NextFloat(-6f, 6f));
                d.scale = Main.rand.NextFloat(1.5f, 2.5f);
                d.noGravity = true;
                d.alpha = 100;
            }

            Vector2 teleportPos;
            int attempts = 0;
            bool validPosition = false;

            do
            {
                attempts++;
                float angle = Main.rand.NextFloat(MathHelper.TwoPi);
                float distance = Main.rand.NextFloat(150f, 300f);
                teleportPos = target.Center + angle.ToRotationVector2() * distance;

                Point tilePos = teleportPos.ToTileCoordinates();

                if (WorldGen.InWorld(tilePos.X, tilePos.Y))
                {
                    Tile tile = Main.tile[tilePos.X, tilePos.Y];
                    if ((tile == null || !tile.HasTile || !Main.tileSolid[tile.TileType]) && tile.LiquidAmount == 0)
                    {
                        validPosition = true;
                    }
                }

            } while (!validPosition && attempts < 20);

            if (validPosition)
            {
                npc.Center = teleportPos;
                npc.velocity = Vector2.Zero;
                npc.netUpdate = true;

                for (int i = 0; i < 40; i++)
                {
                    Dust d = Dust.NewDustDirect(npc.position, npc.width, npc.height, npc.type == NPCID.Demon || npc.type == NPCID.VoodooDemon || npc.type == NPCID.RedDevil ? DustID.Shadowflame : DustID.FlameBurst);
                    d.velocity = new Vector2(Main.rand.NextFloat(-4f, 4f), Main.rand.NextFloat(-4f, 4f));
                    d.scale = Main.rand.NextFloat(1.8f, 2.8f);
                    d.noGravity = true;
                    d.alpha = 50;
                }

                for (int i = 0; i < 15; i++)
                {
                    Dust d = Dust.NewDustDirect(npc.position, npc.width, npc.height, npc.type == NPCID.Demon || npc.type == NPCID.VoodooDemon || npc.type == NPCID.RedDevil ? DustID.PurpleTorch : DustID.Torch);
                    d.velocity = new Vector2(Main.rand.NextFloat(-3f, 3f), Main.rand.NextFloat(-3f, 3f));
                    d.scale = Main.rand.NextFloat(1.2f, 1.8f);
                    d.noGravity = true;
                }

                for (int i = 0; i < 3; i++)
                {
                    Vector2 pos = npc.Center + new Vector2(Main.rand.NextFloat(-30f, 30f), Main.rand.NextFloat(-30f, 30f));
                    Gore.NewGore(npc.GetSource_FromAI(), pos, Vector2.Zero, Main.rand.Next(61, 64));
                }
            }
        }
    }
}
