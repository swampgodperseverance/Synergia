using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Content.Projectiles.Reworks.AltUse
{
    public class GloomRing : ModProjectile
    {
        private bool spawnedOrbitals = false; 

        public override void SetDefaults()
        {
            Projectile.width = 122;
            Projectile.height = 122;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 1200; 
            Projectile.alpha = 255; 
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (!player.active || player.dead)
            {
                Projectile.Kill();
                return;
            }

            Projectile.Center = player.Center;

            if (!spawnedOrbitals)
            {
                SpawnOrbitals(player);
                spawnedOrbitals = true;
            }

            Projectile.rotation += 0.05f;

            if (Projectile.timeLeft > 1150 && Projectile.alpha > 0)
            {
                Projectile.alpha -= 15;
                if (Projectile.alpha < 0)
                    Projectile.alpha = 0;
            }

            if (Projectile.timeLeft < 50)
            {
                Projectile.alpha += 15;
                if (Projectile.alpha > 255)
                    Projectile.alpha = 255;
            }

            float pulse = 1f + 0.04f * (float)System.Math.Sin(Main.GlobalTimeWrappedHourly * 5f);
            Projectile.scale = pulse;

            Lighting.AddLight(Projectile.Center, 0.08f, 0.12f, 0.25f);
        }

        private void SpawnOrbitals(Player player)
        {
            var projUp = Projectile.NewProjectileDirect(
                Projectile.GetSource_FromThis(),
                player.Center,
                Vector2.Zero,
                ModContent.ProjectileType<GloomProjOrbit>(),
                Projectile.damage,
                Projectile.knockBack,
                player.whoAmI
            );
            if (projUp.ModProjectile is GloomProjOrbit up)
                up.SetDirection(OrbitDirection.Up);

            var projDown = Projectile.NewProjectileDirect(
                Projectile.GetSource_FromThis(),
                player.Center,
                Vector2.Zero,
                ModContent.ProjectileType<GloomProjOrbit>(),
                Projectile.damage,
                Projectile.knockBack,
                player.whoAmI
            );
            if (projDown.ModProjectile is GloomProjOrbit down)
                down.SetDirection(OrbitDirection.Down);

            var projLeft = Projectile.NewProjectileDirect(
                Projectile.GetSource_FromThis(),
                player.Center,
                Vector2.Zero,
                ModContent.ProjectileType<GloomProjOrbit>(),
                Projectile.damage,
                Projectile.knockBack,
                player.whoAmI
            );
            if (projLeft.ModProjectile is GloomProjOrbit left)
                left.SetDirection(OrbitDirection.Left);

            var projRight = Projectile.NewProjectileDirect(
                Projectile.GetSource_FromThis(),
                player.Center,
                Vector2.Zero,
                ModContent.ProjectileType<GloomProjOrbit>(),
                Projectile.damage,
                Projectile.knockBack,
                player.whoAmI
            );
            if (projRight.ModProjectile is GloomProjOrbit right)
                right.SetDirection(OrbitDirection.Right);
        }

        public override void Kill(int timeLeft)
        {
            Player player = Main.player[Projectile.owner];
            if (player.active && !player.dead)
            {
                player.AddBuff(ModContent.BuffType<Buffs.GloomDebuff>(), 2400);
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            Vector2 origin = texture.Size() / 2f;
            Vector2 drawPos = Projectile.Center - Main.screenPosition;

            Main.EntitySpriteDraw(
                texture,
                drawPos,
                null,
                Projectile.GetAlpha(lightColor),
                Projectile.rotation,
                origin,
                Projectile.scale,
                SpriteEffects.None,
                0
            );

            float auraStrength = 0.25f + 0.15f * (float)System.Math.Sin(Main.GlobalTimeWrappedHourly * 2f);
            float auraScale = Projectile.scale * (1.08f + 0.03f * (float)System.Math.Sin(Main.GlobalTimeWrappedHourly * 3f));
            Color auraColor = new Color(60, 100, 180) * auraStrength * (1f - Projectile.alpha / 255f) * 0.8f;

            for (int i = 0; i < 6; i++)
            {
                Vector2 offset = Vector2.UnitX.RotatedBy(MathHelper.ToRadians(60 * i)) * 2.5f;
                Main.EntitySpriteDraw(
                    texture,
                    drawPos + offset,
                    null,
                    auraColor,
                    Projectile.rotation,
                    origin,
                    auraScale,
                    SpriteEffects.None,
                    0
                );
            }

            return false;
        }

        public override bool? CanDamage() => false;
    }
}
