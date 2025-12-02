using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.GameContent;
using Synergia.Trails;
using System.Collections.Generic;
using System.Linq;

namespace Synergia.Content.Projectiles.Reworks.Reworks2
{
    public class WaterGunBubble : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 1; 
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 3; 
        }

        public override void SetDefaults()
        {
            Projectile.penetrate = 1;
            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.aiStyle = 1;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.timeLeft = 30;
            Projectile.scale = 1.4f;

            Projectile.alpha = 255;
        }

        public override void AI()
        {
  
            if (Projectile.alpha > 0)
                Projectile.alpha -= 25;
            if (Projectile.alpha < 0)
                Projectile.alpha = 0;

            Lighting.AddLight(Projectile.Center,
                0f,
                0.2f,
                0.25f);

            Projectile.spriteDirection = Projectile.direction;
            Projectile.scale *= 0.985f;
            Projectile.ai[0]++;

            if (Projectile.ai[0] == 4f)
            {
                for (int i = 0; i < 10; i++)
                {
                    int dust = Dust.NewDust(
                        Projectile.position,
                        8, 8,
                        DustID.Wet,
                        Projectile.velocity.X * .5f,
                        Projectile.velocity.Y * .5f
                    );
                    Main.dust[dust].noGravity = true;
                }
            }

            if (Main.rand.NextBool(6))
                Dust.NewDust(
                    Projectile.position,
                    8, 8,
                    DustID.Wet,
                    Projectile.velocity.X * .1f,
                    Projectile.velocity.Y * .1f
                );
        }


        public override bool PreDraw(ref Color lightColor)
        {
       
            var texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            Vector2 origin = texture.Size() / 2f;

            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                Vector2 drawPos = Projectile.oldPos[i] + Projectile.Size / 2f - Main.screenPosition;

                float progress = (Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length;

                Color trailColor = new Color(100, 150, 255) * progress * 0.6f;

                Main.EntitySpriteDraw(
                    texture,
                    drawPos,
                    null,
                    trailColor,
                    Projectile.rotation,
                    origin,
                    Projectile.scale,
                    SpriteEffects.None,
                    0
                );
            }

            return true;
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.NPCDeath9, Projectile.position);

            for (int i = 0; i < 10; i++)
            {
                Dust.NewDust(
                    Projectile.position,
                    8, 8,
                    DustID.Wet,
                    Projectile.oldVelocity.X * .2f,
                    Projectile.oldVelocity.Y * .2f
                );
            }

            NPC target = FindClosestNPC(500f);
            if (target == null)
                return;

            Vector2 dir = (target.Center - Projectile.Center).SafeNormalize(Vector2.Zero);

            Projectile.NewProjectile(
                Projectile.GetSource_FromThis(),
                Projectile.Center,
                dir * 10f,
                ModContent.ProjectileType<WaterGunBubble2>(),
                Projectile.damage,
                Projectile.knockBack,
                Projectile.owner
            );
        }

        private NPC FindClosestNPC(float maxDetectDistance)
        {
            NPC closest = null;
            float sqDist = maxDetectDistance * maxDetectDistance;

            foreach (NPC npc in Main.npc.Where(n => n.CanBeChasedBy()))
            {
                float dist = Vector2.DistanceSquared(npc.Center, Projectile.Center);
                if (dist < sqDist)
                {
                    sqDist = dist;
                    closest = npc;
                }
            }
            return closest;
        }
    }
}
