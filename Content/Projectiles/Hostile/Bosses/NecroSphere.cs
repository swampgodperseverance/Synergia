using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Content.Projectiles.Hostile.Bosses
{
    public class NecroSphere : ModProjectile
    {
        private bool hasFired;

        public override void SetDefaults()
        {
            Projectile.hostile = true;
            Projectile.width = 32; 
            Projectile.height = 32;
            Projectile.friendly = false;
            Projectile.aiStyle = -1;
            Projectile.timeLeft = 90; 
            Projectile.tileCollide = false;
            Projectile.scale = 1.2f;

            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10; 
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void AI()
        {

            Projectile.rotation += 0.08f;


            float appearTime = 20f;
            float disappearTime = 20f;
            if (Projectile.timeLeft > 60) 
                Projectile.alpha = (int)MathHelper.Lerp(255, 0, (90f - Projectile.timeLeft) / appearTime);
            else if (Projectile.timeLeft < disappearTime)
                Projectile.alpha = (int)MathHelper.Lerp(0, 255, (disappearTime - Projectile.timeLeft) / disappearTime);
            else
                Projectile.alpha = 0;
            Lighting.AddLight(Projectile.Center, new Vector3(0.4f, 0f, 0.6f));

            if (Main.rand.NextBool(5))
            {
                Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.PurpleTorch, 0, 0, 150, Color.Purple, 1.3f);
                d.noGravity = true;
                d.velocity *= 0.5f;
            }

        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(180, 100, 255, 200) * ((255 - Projectile.alpha) / 255f);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            Vector2 origin = texture.Size() / 2f;
            Color trailColor = new Color(160, 60, 255, 100);
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                Vector2 drawPos = Projectile.oldPos[i] + Projectile.Size / 2f - Main.screenPosition;
                float scale = Projectile.scale * (1f - i / (float)Projectile.oldPos.Length);
                float alpha = (1f - i / (float)Projectile.oldPos.Length);
                Main.spriteBatch.Draw(texture, drawPos, null, trailColor * alpha, Projectile.rotation, origin, scale, SpriteEffects.None, 0f);
            }
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null,
                Projectile.GetAlpha(lightColor), Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0f);

            return false;
        }
        public override void OnKill(int timeLeft)
        {
            Vector2 spawnOffset = new Vector2(20f, 20f);

            Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.position.X, Projectile.position.Y, 14f, 0f, ModContent.ProjectileType<NecroFire>(), Projectile.damage, 0f, Projectile.owner, 0f, 0f);
			Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.position.X, Projectile.position.Y, -14f, 0f, ModContent.ProjectileType<NecroFire>(), Projectile.damage, 0f, Projectile.owner, 0f, 0f);
			Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.position.X, Projectile.position.Y, 0f, 14f, ModContent.ProjectileType<NecroFire>(), Projectile.damage, 0f, Projectile.owner, 0f, 0f);
			Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.position.X, Projectile.position.Y, 0f, -14f, ModContent.ProjectileType<NecroFire>(), Projectile.damage, 0f, Projectile.owner, 0f, 0f);
			SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
            for (int i = 0; i < 20; i++)
            {
                Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.PurpleTorch, Main.rand.NextFloat(-3f, 3f), Main.rand.NextFloat(-3f, 3f), 150, Color.Purple, 1.8f);
                d.noGravity = true;
            }
            SoundEngine.PlaySound(SoundID.Item10 with { Volume = 0.6f, Pitch = 0.2f }, Projectile.Center);
        }
    }
}
