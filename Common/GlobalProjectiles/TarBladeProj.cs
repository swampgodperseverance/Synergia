using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.Graphics.Shaders;
using Synergia.Trails;

namespace Synergia.Common.GlobalProjectiles
{
    public class TarBladeGP : GlobalProjectile
    {
        public override bool InstancePerEntity => true;

        private PrimDrawer visualTrail;

        public override void AI(Projectile projectile)
        {
            if (projectile.ModProjectile != null &&
                projectile.ModProjectile.Mod.Name == "ValhallaMod" &&
                projectile.ModProjectile.Name == "TarBlade")
            {

                if (visualTrail == null)
                {

                    MiscShaderData shader = null;
                    string[] keys = { "FlameLashTrailColorGradient", "FlameLashTrailShape", "FlameLashTrailErosion" };
                    foreach (var k in keys)
                        if (GameShaders.Misc.TryGetValue(k, out shader))
                            break;

                    if (shader != null)
                    {
                        shader.UseImage1("Images/Misc/noise");
                        shader.UseOpacity(0.85f);
                        shader.UseColor(new Color(135, 206, 250)); 
                        shader.UseSecondaryColor(new Color(139, 69, 19)); 
                    }

                    visualTrail = new PrimDrawer(
                        widthFunc: t => MathHelper.Lerp(5f, 0.3f, t), 
                        colorFunc: t =>
                        {

                            Color inner = new Color(139, 69, 19); 
                            Color outer = new Color(135, 206, 250); 


                            float blend = MathHelper.Clamp(t * 1.8f, 0f, 1f); 
                            Color mix = Color.Lerp(inner, outer, blend);

                            mix *= (1f - t * 0.8f);

                            return mix;
                        },
                        shader: shader
                    );


                    ProjectileID.Sets.TrailCacheLength[projectile.type] = 6;
                    ProjectileID.Sets.TrailingMode[projectile.type] = 2;
                }


                Lighting.AddLight(projectile.Center, 0.3f, 0.2f, 0.1f);

                if (Main.rand.NextBool(5))
                {
                    int d = Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Dirt,
                        -projectile.velocity.X * 0.2f, -projectile.velocity.Y * 0.2f, 100, default, 1.1f);
                    Main.dust[d].noGravity = true;
                    Main.dust[d].velocity *= 0.25f;
                }
            }
        }

        public override bool PreDraw(Projectile projectile, ref Color lightColor)
        {
     
            if (projectile.ModProjectile != null &&
                projectile.ModProjectile.Mod.Name == "ValhallaMod" &&
                projectile.ModProjectile.Name == "TarBlade" &&
                visualTrail != null)
            {
                List<Vector2> points = projectile.oldPos
                    .Where(v => v != Vector2.Zero)
                    .Select(v => v + projectile.Size / 2f)
                    .ToList();

                if (points.Count > 1)
                {
                    Vector2 offset = -Main.screenPosition;
    
                    visualTrail.DrawPrims(points, offset, totalTrailPoints: 14);
                }
            }

            return base.PreDraw(projectile, ref lightColor);
        }
    }
}
