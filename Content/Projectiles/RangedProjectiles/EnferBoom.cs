using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Synergia.Reassures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Content.Projectiles.RangedProjectiles
{
    public class EnferBoom : ModProjectile
    {
        private ref float Timer => ref Projectile.ai[0];

        public override string Texture => "Synergia/Assets/Textures/Glow";

        public override void SetDefaults()
        {
            Projectile.width = 64;
            Projectile.height = 64;
            Projectile.timeLeft = 16;
            Projectile.penetrate = -1;
            Projectile.damage = 1000;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
        }

        public override void AI()
        {
            Timer++;

            Projectile.scale = MathHelper.Lerp(0.35f, 2.1f, Timer / 5f);
            Projectile.rotation += 0.18f;

            if (Timer == 1)
            {
                Projectile.Damage();

                SoundStyle boom = Reassures.Reassures.RSounds.BigBoom;
                boom.Volume = 0.8f;
                boom.PitchVariance = 0.12f;

                SoundEngine.PlaySound(boom, Projectile.Center);
            }


            Lighting.AddLight(Projectile.Center, Color.OrangeRed.ToVector3() * 1.4f);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch sb = Main.spriteBatch;

            Texture2D glowTex = ModContent.Request<Texture2D>("Synergia/Assets/Textures/Glow").Value;
            Texture2D coreTex = ModContent.Request<Texture2D>("Synergia/Assets/Textures/LightTrail_1").Value;

            sb.End();
            sb.Begin(SpriteSortMode.Deferred, BlendState.Additive, SamplerState.LinearClamp,
                DepthStencilState.None, RasterizerState.CullNone, null,
                Main.GameViewMatrix.TransformationMatrix);

            float fade = MathHelper.Clamp(1f - Timer / 12f, 0f, 1f);
            float pulse = 1f + MathF.Sin(Timer * 0.6f) * 0.08f;

            Color outer = Color.Lerp(Color.DarkRed, Color.Red, 0.5f) * fade * 0.75f;
            Color mid = Color.Lerp(Color.OrangeRed, Color.Yellow, 0.45f) * fade;
            Color core = Color.White * fade;

            sb.Draw(glowTex,
                Projectile.Center - Main.screenPosition,
                null,
                outer,
                -Projectile.rotation * 0.35f,
                glowTex.Size() / 2f,
                Projectile.scale * 1.3f,
                SpriteEffects.None,
                0f);

            sb.Draw(glowTex,
                Projectile.Center - Main.screenPosition,
                null,
                mid,
                Projectile.rotation * 0.5f,
                glowTex.Size() / 2f,
                Projectile.scale,
                SpriteEffects.None,
                0f);

            sb.Draw(coreTex,
                Projectile.Center - Main.screenPosition,
                null,
                core * 0.35f,
                Projectile.rotation * 0.2f,
                coreTex.Size() / 2f,
                Projectile.scale * 0.95f * pulse,
                SpriteEffects.None,
                0f);

            sb.Draw(coreTex,
                Projectile.Center - Main.screenPosition,
                null,
                core * 0.55f,
                -Projectile.rotation * 0.15f,
                coreTex.Size() / 2f,
                Projectile.scale * 0.7f * pulse,
                SpriteEffects.None,
                0f);

            sb.Draw(coreTex,
                Projectile.Center - Main.screenPosition,
                null,
                core,
                0f,
                coreTex.Size() / 2f,
                Projectile.scale * 0.45f,
                SpriteEffects.None,
                0f);

            sb.End();
            sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp,
                DepthStencilState.None, RasterizerState.CullNone, null,
                Main.GameViewMatrix.TransformationMatrix);

            return false;
        }
    }
}
