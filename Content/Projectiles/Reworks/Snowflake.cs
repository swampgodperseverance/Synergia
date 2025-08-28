using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;

namespace Synergia.Content.Projectiles.Reworks
{
    public class Snowflake : ModProjectile
    {
        private int shootTimer = 0;

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Snowflake");
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 22;
            Projectile.height = 22;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 120; // можно сделать "бесконечным" обновляя вручную
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (!player.active || player.dead)
            {
                Projectile.Kill();
                return;
            }

            // вращение вокруг игрока
            float orbitRadius = 64f;
            float orbitSpeed = 0.08f;

            if (Projectile.ai[1] == 0) // только первая увеличивает угол
                Projectile.ai[0] += orbitSpeed;

            // угол: вторая снежинка смещена на 180°
            float angle = Projectile.ai[0] + (Projectile.ai[1] == 1 ? (float)Math.PI : 0f);

            Vector2 offset = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * orbitRadius;
            Projectile.Center = player.Center + offset;

            // плавное появление/исчезновение
            Projectile.alpha = (int)MathHelper.Lerp(255, 0, Math.Min(1f, Projectile.timeLeft / 60f));
            if (Projectile.timeLeft < 60)
                Projectile.alpha = (int)MathHelper.Lerp(0, 255, (60f - Projectile.timeLeft) / 60f);

            // трейл и дастики
            if (Main.rand.NextBool(2))
            {
                Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.IceTorch);
                d.noGravity = true;
                d.scale = 1.2f;
                d.velocity *= 0.3f;
            }

            // --- стрельба каждые 80 тиков ---
            shootTimer++;
            if (shootTimer >= 80)
            {
                shootTimer = 0;
                NPC target = FindClosestNPC(500f);
                if (target != null && Main.myPlayer == Projectile.owner)
                {
                    Vector2 velocity = Vector2.Normalize(target.Center - Projectile.Center) * 10f;
                    Projectile.NewProjectile(
                        Projectile.GetSource_FromThis(),
                        Projectile.Center,
                        velocity,
                        ProjectileID.NorthPoleSpear,
                        Projectile.damage,
                        Projectile.knockBack,
                        Projectile.owner
                    );
                }
            }
        }

        private NPC FindClosestNPC(float maxDetectDistance)
        {
            NPC closestNPC = null;
            float sqrMaxDetectDistance = maxDetectDistance * maxDetectDistance;

            foreach (NPC npc in Main.npc)
            {
                if (npc.CanBeChasedBy())
                {
                    float sqrDistanceToNPC = Vector2.DistanceSquared(npc.Center, Projectile.Center);
                    if (sqrDistanceToNPC < sqrMaxDetectDistance)
                    {
                        sqrMaxDetectDistance = sqrDistanceToNPC;
                        closestNPC = npc;
                    }
                }
            }
            return closestNPC;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            var texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);

            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = Projectile.oldPos[k] + drawOrigin - Main.screenPosition;
                Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
                Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
            }
            return true;
        }
    }
}
