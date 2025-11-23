using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Synergia.Content.GlobalProjectiles
{
    public class JungleSporeEnemyProjectile : GlobalProjectile
    {
        public override bool InstancePerEntity => true;

        private Dictionary<int, List<Vector2>> oldPositions = new Dictionary<int, List<Vector2>>();

        public override void AI(Projectile projectile)
        {
            if (projectile.ModProjectile != null &&
                projectile.ModProjectile.Mod.Name == "ValhallaMod" &&
                projectile.ModProjectile.Name == "JungleSporeEnemy")
            {
                if (!oldPositions.ContainsKey(projectile.whoAmI))
                    oldPositions[projectile.whoAmI] = new List<Vector2>();

                var positions = oldPositions[projectile.whoAmI];
                positions.Insert(0, projectile.Center);

                if (positions.Count > 5)
                    positions.RemoveAt(positions.Count - 1);
            }
        }

        public override bool PreDraw(Projectile projectile, ref Color lightColor)
        {
            if (projectile.ModProjectile != null &&
                projectile.ModProjectile.Mod.Name == "ValhallaMod" &&
                projectile.ModProjectile.Name == "JungleSporeEnemy")
            {
                if (oldPositions.TryGetValue(projectile.whoAmI, out var positions))
                {
                    Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[projectile.type].Value;

                    for (int i = 0; i < positions.Count; i++)
                    {
                        float alpha = 0.5f * (1f - i / 3f); 
                        Vector2 drawPos = positions[i] - Main.screenPosition;

                        Main.EntitySpriteDraw(
                            texture,
                            drawPos,
                            null,
                            projectile.GetAlpha(lightColor) * alpha,
                            projectile.rotation,
                            texture.Size() / 2f,
                            projectile.scale,
                            projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None,
                            0
                        );
                    }
                }
            }

            return true; 
        }
    }
}
