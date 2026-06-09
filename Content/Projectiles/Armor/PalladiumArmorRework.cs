using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Synergia.Helpers;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;

namespace Synergia.Content.Projectiles.Armor
{
    public class PalladiumArmorRework : ModProjectile
    {
        private Vector2 oldPos = Vector2.Zero;
        private bool spawned = false;

        public override string Texture
        {
            get
            {
                return "Synergia/Content/Projectiles/Armor/PalladiumArmorRework";
            }
        }

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(93);
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 300;
            Projectile.scale = 0.5f;
        }

        public override void OnSpawn(IEntitySource source)
        {
            if (!spawned && Main.myPlayer == Projectile.owner)
            {
                Player ownerPlayer = Main.player[Projectile.owner];

                int side = Main.rand.Next(4);

                Vector2 spawnPosition = Vector2.Zero;
                Vector2 screenCenter = ownerPlayer.Center;

                int screenWidth = Main.screenWidth;
                int screenHeight = Main.screenHeight;

                float offsetDistance = Main.rand.Next(50, 300);

                switch (side)
                {
                    case 0:
                        spawnPosition = new Vector2(
                            screenCenter.X - screenWidth / 2 - offsetDistance,
                            screenCenter.Y + Main.rand.Next(-screenHeight / 2, screenHeight / 2)
                        );
                        break;
                    case 1:
                        spawnPosition = new Vector2(
                            screenCenter.X + screenWidth / 2 + offsetDistance,
                            screenCenter.Y + Main.rand.Next(-screenHeight / 2, screenHeight / 2)
                        );
                        break;
                    case 2:
                        spawnPosition = new Vector2(
                            screenCenter.X + Main.rand.Next(-screenWidth / 2, screenWidth / 2),
                            screenCenter.Y - screenHeight / 2 - offsetDistance
                        );
                        break;
                    case 3:
                        spawnPosition = new Vector2(
                            screenCenter.X + Main.rand.Next(-screenWidth / 2, screenWidth / 2),
                            screenCenter.Y + screenHeight / 2 + offsetDistance
                        );
                        break;
                }

                Projectile.Center = spawnPosition;

                Vector2 directionToPlayer = ownerPlayer.Center - Projectile.Center;
                directionToPlayer.Normalize();

                float speed = Main.rand.Next(8, 21);
                Projectile.velocity = directionToPlayer * speed;

                float deviation = MathHelper.ToRadians(Main.rand.Next(-15, 16));
                Projectile.velocity = Projectile.velocity.RotatedBy(deviation);

                spawned = true;

                if (Main.netMode != NetmodeID.SinglePlayer)
                {
                    NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, Projectile.whoAmI);
                }
            }
        }

        public override void AI()
        {
            Player ownerPlayer = Main.player[Projectile.owner];

            Projectile.ai[0] += 1f;

            if (Projectile.ai[0] > 20f)
            {
                Projectile.rotation = Projectile.velocity.ToRotation() + 1.5707964f;

                float distanceToPlayer = Vector2.Distance(Projectile.Center, ownerPlayer.Center);
                if (distanceToPlayer < 100f)
                {
                    Projectile.velocity *= 0.96f;
                }

                return;
            }

            if (Projectile.ai[0] == 20f)
            {
                if (Main.myPlayer == Projectile.owner)
                {
                    Projectile.velocity = Vector2.Normalize(Main.MouseWorld - Projectile.Center) * 16f;
                    NetMessage.SendData(27, -1, -1, null, Projectile.whoAmI, 0f, 0f, 0f, 0, 0, 0);
                }
                SoundEngine.PlaySound(SoundID.Item46, Projectile.position);
                return;
            }

            Projectile.rotation += Projectile.velocity.X / 8f;
            Projectile.velocity *= 0.97f;

            if (ownerPlayer.active && !ownerPlayer.dead)
            {
                Projectile.position += ownerPlayer.velocity / Projectile.MaxUpdates;
            }

            this.oldPos = Projectile.Center;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (this.oldPos.HasNaNs() || this.oldPos == Vector2.Zero)
            {
                this.oldPos = Projectile.Center;
            }
            else if (!Main.gamePaused)
            {
                this.oldPos = Vector2.Lerp(this.oldPos, Projectile.Center, 0.1f);
            }

            Color yellow = new Color(255, 220, 40);
            Color darkYellow = new Color(255, 160, 20);
            Color lightYellow = new Color(255, 255, 100);
            Color glowYellow = new Color(255, 200, 30);

            Texture2D trailTexture = TextureAssets.Extra[98].Value;

            if (this.oldPos != Projectile.Center && Projectile.ai[0] >= 20f)
            {
                float opacity = MathHelper.Min(Projectile.timeLeft, 30f) / 30f;

                Main.EntitySpriteDraw(
                    trailTexture,
                    Projectile.Center - Main.screenPosition,
                    new Rectangle(0, trailTexture.Height / 2, trailTexture.Width, trailTexture.Height / 2),
                    yellow * opacity * 0.7f,
                    (Projectile.Center - this.oldPos).ToRotation() + 1.5707964f,
                    new Vector2(trailTexture.Width * 0.5f, 0f),
                    new Vector2(Projectile.scale * 0.5f, Vector2.Distance(Projectile.Center, this.oldPos) / trailTexture.Height * 2.75f),
                    SpriteEffects.None,
                    0f
                );
            }

            Texture2D mainTexture = TextureAssets.Projectile[Projectile.type].Value;

            SpriteEffects effects = Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

            for (int i = 2; i >= 0; i--)
            {
                float scale = Projectile.scale + (i * 0.08f);
                Color color = i == 2 ? glowYellow * 0.2f : (i == 1 ? darkYellow * 0.4f : yellow);
                float alpha = 1f - (Projectile.alpha / 255f);

                Main.EntitySpriteDraw(
                    mainTexture,
                    Projectile.Center - Main.screenPosition,
                    null,
                    color * alpha,
                    Projectile.rotation,
                    mainTexture.Size() * 0.5f,
                    scale,
                    effects,
                    0f
                );
            }

            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            for (int i = 0; i < 15; i++)
            {
                Dust dust = Dust.NewDustDirect(
                    target.Center,
                    20, 20,
                    DustID.SolarFlare,
                    Main.rand.NextFloat(-5f, 5f),
                    Main.rand.NextFloat(-5f, 5f),
                    100,
                    new Color(255, 200, 0),
                    1.5f
                );
                dust.noGravity = true;
            }

            SoundEngine.PlaySound(SoundID.Item74, target.Center);
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 30; i++)
            {
                Dust dust = Dust.NewDustDirect(
                    Projectile.Center,
                    10, 10,
                    DustID.SolarFlare,
                    Main.rand.NextFloat(-6f, 6f),
                    Main.rand.NextFloat(-6f, 6f),
                    100,
                    new Color(255, 180, 0),
                    1.8f
                );
                dust.noGravity = true;
            }

            SoundEngine.PlaySound(SoundID.Item14, Projectile.Center);
        }
    }
}