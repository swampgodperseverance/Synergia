using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using System.Collections.Generic;

namespace Synergia.Common.GlobalNPCs
{
    public class InfernalBoneSerpent : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        private int dashCooldown;
        private int chargeTimer;
        private bool charging;
        private bool dashing;

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
                npc.velocity *= 0.9f;
                chargeTimer--;

                Lighting.AddLight(npc.Center, 1.2f, 0.4f, 0.05f);

                if (chargeTimer <= 0)
                {
                    charging = false;
                    dashing = true;

                    Vector2 direction = target.Center - npc.Center;
                    direction.Normalize();
                    npc.velocity = direction * 22f;

                    dashCooldown = 220;
                }
            }

            if (dashing)
            {
                npc.velocity *= 0.99f;

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

        public override bool PreDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (npc.type == NPCID.BoneSerpentHead && dashing)
            {
                Texture2D texture = Terraria.GameContent.TextureAssets.Npc[npc.type].Value;
                Vector2 origin = texture.Size() / 2f;

                for (int i = 0; i < 5; i++)
                {
                    Vector2 afterPos = npc.Center - npc.velocity * i * 0.5f;

                    Color lavaColor = Color.Lerp(Color.OrangeRed, Color.Yellow, 0.5f)
                        * (0.5f - i * 0.08f);

                    spriteBatch.Draw(
                        texture,
                        afterPos - screenPos,
                        npc.frame,
                        lavaColor,
                        npc.rotation,
                        origin,
                        npc.scale,
                        SpriteEffects.None,
                        0f
                    );
                }
            }

            return true;
        }
    }

    
    public class DemonTeleport : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        private int teleportCooldown = 300;

        public override void AI(NPC npc)
        {
            if (npc.type != NPCID.Demon && npc.type != NPCID.VoodooDemon)
                return;

            if (teleportCooldown > 0)
            {
                teleportCooldown--;
                return;
            }

            Player target = Main.player[npc.target];
            if (!target.active || target.dead)
                return;

            if (Main.rand.NextBool(600))
            {
                TeleportDemon(npc, target);
                teleportCooldown = 180;
            }
        }

        private void TeleportDemon(NPC npc, Player target)
        {
            for (int i = 0; i < 30; i++)
            {
                Dust d = Dust.NewDustDirect(npc.position, npc.width, npc.height, DustID.Shadowflame);
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
                    if (tile == null || !tile.HasTile || !Main.tileSolid[tile.TileType])
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
                    Dust d = Dust.NewDustDirect(npc.position, npc.width, npc.height, DustID.Shadowflame);
                    d.velocity = new Vector2(Main.rand.NextFloat(-4f, 4f), Main.rand.NextFloat(-4f, 4f));
                    d.scale = Main.rand.NextFloat(1.8f, 2.8f);
                    d.noGravity = true;
                    d.alpha = 50;
                }

                for (int i = 0; i < 15; i++)
                {
                    Dust d = Dust.NewDustDirect(npc.position, npc.width, npc.height, DustID.PurpleTorch);
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