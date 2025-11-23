using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using System.Linq;
using Synergia.Trails;

namespace Synergia.Common.GlobalProjectiles
{
    public class FuryGP : GlobalProjectile
    {
        public override bool InstancePerEntity => true;

        private PrimDrawer trailDrawer;
        private bool initialized = false;

        public override void AI(Projectile projectile)
        {
            if (!initialized && IsCorrectProjectile(projectile))
            {
                InitializeTrail();
                initialized = true;
            }
        }

        
        private bool IsCorrectProjectile(Projectile projectile)
        {
        
            return projectile.type == ModContent.ProjectileType<Bismuth.Content.Projectiles.FuryOfWatersP>();
        }

        private void InitializeTrail()
        {
        
            Texture2D trailTexture = ModContent.Request<Texture2D>("Synergia/Assets/Textures/Trails/FuryWaters_Trail").Value;

        
            trailDrawer = new PrimDrawer(
                widthFunc: t => 25f * (1f - t), // Ширина уменьшается по длине трейла
                colorFunc: t => new Color(100, 200, 255, (byte)(200 * (1f - t))) // Цвет с прозрачностью
            );
        }

        public override void PostDraw(Projectile projectile, Color lightColor)
        {
            if (!IsCorrectProjectile(projectile) || trailDrawer == null)
                return;

            DrawTrail(projectile);
        }

        private void DrawTrail(Projectile projectile)
        {
            // Берем только непустые позиции и добавляем смещение по центру снаряда
            Vector2[] trailPositions = projectile.oldPos
                .Where(pos => pos != Vector2.Zero)
                .Select(pos => pos + projectile.Size / 2f)
                .ToArray();

            if (trailPositions.Length > 1)
            {
                // Рисуем трейл
                trailDrawer.DrawPrims(trailPositions, -Main.screenPosition, trailPositions.Length);
            }
        }
    }
}
