using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ValhallaMod.Projectiles.AI;

namespace Synergia.Content.Projectiles.Reworks.Reworks2
{
    public class FaceMace2 : FlailAI
    {
        private float pulseTimer;
        private bool hitGround;

        public override void SetDefaults()
        {
            base.Projectile.netImportant = true;
            base.Projectile.width = 30;
            base.Projectile.height = 30;
            base.Projectile.friendly = true;
            base.Projectile.penetrate = -1;
            base.Projectile.DamageType = DamageClass.Melee;
            base.Projectile.usesLocalNPCImmunity = true;
            base.Projectile.localNPCHitCooldown = 10;

            this.ChainTexturePath = "ValhallaMod/Projectiles/Flail/FaceMace_Chain";
        }

        public override void AI()
        {
            base.AI();

            pulseTimer += 0.12f; 

            if (!hitGround && Projectile.velocity.Y == 0f && Math.Abs(Projectile.velocity.X) < 0.2f)
            {
                hitGround = true;
                SpawnSnowBurst();
            }

            if (Projectile.velocity.Y != 0f)
                hitGround = false;
        }

        private void SpawnSnowBurst()
        {
            if (Main.myPlayer != Projectile.owner)
                return;

            Player player = Main.player[Projectile.owner];

            int amount = Main.rand.Next(4, 7);
            float speed = 9f;

            Vector2 direction = Vector2.Normalize(Main.MouseWorld - Projectile.Center);

            for (int i = 0; i < amount; i++)
            {
                float spread = MathHelper.ToRadians(15);
                float randomRot = Main.rand.NextFloat(-spread, spread);

                Vector2 velocity = direction.RotatedBy(randomRot) * speed;

                Projectile.NewProjectile(
                    Projectile.GetSource_FromThis(),
                    Projectile.Center,
                    velocity,
                    ProjectileID.SnowBallFriendly,
                    Projectile.damage / 2,
                    0f,
                    Projectile.owner
                );
            }
            for (int i = 0; i < 25; i++)
            {
                Dust d = Dust.NewDustPerfect(
                    Projectile.Center,
                    DustID.Snow,
                    Main.rand.NextVector2Circular(4f, 4f),
                    150,
                    Color.White,
                    1.6f
                );
                d.noGravity = true;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D ring = ModContent.Request<Texture2D>("Synergia/Assets/Textures/Ring").Value;
            float pulse = 0.8f + (float)Math.Sin(pulseTimer) * 0.25f;
            float scale = (Projectile.width / (float)ring.Width) * 1.5f * pulse;
            Color snowColor = new Color(160, 210, 255, 0) * 0.85f;

            Main.EntitySpriteDraw(
                ring,
                Projectile.Center - Main.screenPosition,
                null,
                snowColor,
                pulseTimer * 0.5f, 
                ring.Size() / 2f,
                scale,
                SpriteEffects.None,
                0
            );

            return base.PreDraw(ref lightColor);
        }
    }
}
