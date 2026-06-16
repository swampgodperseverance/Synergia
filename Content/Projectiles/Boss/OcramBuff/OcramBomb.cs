using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;

namespace Synergia.Content.Projectiles.Boss.OcramBuff
{
    public class OcramBomb : ModProjectile{
        private ref float Timer => ref Projectile.ai[0];
        private ref float PulseCount => ref Projectile.ai[1];

        public override string Texture => "Synergia/Content/Projectiles/Boss/OcramBuff/OcramBomb";

        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.timeLeft = 155;
            Projectile.penetrate = 1;
            Projectile.damage = 100;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Default;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.scale = 1f;
        }

        public override void AI()
        {
            Timer++;

            Projectile.rotation += 0.12f;

            if (Timer < 35)
            {
                Projectile.scale = MathHelper.Lerp(0.05f, 1f, Timer / 35f);
                Projectile.alpha = (int)MathHelper.Lerp(255f, 0f, Timer / 35f);
            }
            else
            {
                Projectile.scale = 1f;
                Projectile.alpha = 0;
            }

            if (Timer >= 45 && Timer % 58 == 0 && PulseCount < 3)
            {
                PulseCount++;
            }

            if (Timer == 192)
            {
                Projectile.Kill();
            }

            Lighting.AddLight(Projectile.Center, new Vector3(0.48f, 0.12f, 0.78f) * 1.15f);
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 35; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 27, Scale: 1.6f);
                dust.noGravity = true;
                dust.velocity *= 2.5f;
            }

            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero,
                ModContent.ProjectileType<OcramBombExplosion>(), Projectile.damage, 0f, Projectile.owner);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch sb = Main.spriteBatch;
            Texture2D projTex = ModContent.Request<Texture2D>(Texture).Value;
            Texture2D glowTex = ModContent.Request<Texture2D>("Synergia/Content/Projectiles/Boss/OcramBuff/OcramBomb_Glow").Value;
            Texture2D trailTex = ModContent.Request<Texture2D>("Synergia/Assets/Textures/LightTrail_1").Value;

            Vector2 drawPos = Projectile.Center - Main.screenPosition;
            float pulse = 1f + MathF.Sin(Timer * 0.42f) * 0.13f;
            float strongPulse = 1f + MathF.Sin(Timer * 0.85f) * 0.22f; 

            // used it for disablin
            float fade = 1f;

            sb.End();
            sb.Begin(SpriteSortMode.Deferred, BlendState.Additive, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

            // effect
            if (Projectile.timeLeft < 890 && Projectile.oldPos.Length > 1)
            {
                for (int k = 0; k < Projectile.oldPos.Length - 1; k++)
                {
                    Vector2 trailPos = Projectile.oldPos[k] + new Vector2(Projectile.width, Projectile.height) / 2f - Main.screenPosition;
                    Color trailColor = new Color(70 - k * 6, 20, 90 + k * 5, 50 + k * 6);

                    float rotation = (float)Math.Atan2(Projectile.oldPos[k].Y - Projectile.oldPos[k + 1].Y,
                                                       Projectile.oldPos[k].X - Projectile.oldPos[k + 1].X);

                    float trailScale = (Projectile.scale - k / (float)Projectile.oldPos.Length) * 0.85f;

                    sb.Draw(trailTex, trailPos, null, trailColor, rotation, trailTex.Size() / 2f, trailScale, SpriteEffects.None, 0f);
                    sb.Draw(trailTex, trailPos - Projectile.oldPos[k] * 0.4f + Projectile.oldPos[k + 1] * 0.6f, null, trailColor * 0.7f,
                            rotation, trailTex.Size() / 2f, trailScale * 0.75f, SpriteEffects.None, 0f);
                }
            }

            // here is outline
            for (int i = 0; i < 6; i++)
            {
                float offsetAngle = i * MathHelper.Pi / 3f + Timer * 0.1f;
                Vector2 offset = offsetAngle.ToRotationVector2() * (4.5f + (float)Math.Sin(Timer * 3f) * 1.8f);
                float gScale = Projectile.scale * (1.42f + i * 0.08f) * strongPulse;

                Color col = new Color(65, 25, 95) * fade * (0.65f - i * 0.08f);
                sb.Draw(glowTex, drawPos + offset, null, col, Projectile.rotation * 0.75f, glowTex.Size() / 2f, gScale, SpriteEffects.None, 0f);
            }

            for (int i = 0; i < 4; i++)
            {
                float offsetAngle = i * MathHelper.PiOver2 + Timer * 0.15f;
                Vector2 offset = offsetAngle.ToRotationVector2() * (6.5f + (float)Math.Sin(Timer * 4f) * 2.2f);
                float gScale = Projectile.scale * 1.38f * pulse;

                Color col = new Color(110, 45, 165) * fade * 0.75f;
                sb.Draw(glowTex, drawPos + offset, null, col, -Projectile.rotation * 0.45f, glowTex.Size() / 2f, gScale, SpriteEffects.None, 0f);
            }

            // central chaotic glow
            sb.Draw(glowTex, drawPos, null, new Color(140, 60, 210) * fade * 0.95f,
                Projectile.rotation * 1.15f, glowTex.Size() / 2f, Projectile.scale * 1.48f * strongPulse, SpriteEffects.None, 0f);
            sb.End();
            sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            sb.Draw(projTex, drawPos, null, new Color(120, 70, 160) * fade, Projectile.rotation, projTex.Size() / 2f, Projectile.scale, SpriteEffects.None, 0f);
            sb.Draw(projTex, drawPos, null, Color.White * fade * 0.7f, Projectile.rotation, projTex.Size() / 2f, Projectile.scale * 0.9f, SpriteEffects.None, 0f);

            return false;
        }
    }

    // OcramBombExplosion
    public class OcramBombExplosion : ModProjectile
    {
        private ref float Timer => ref Projectile.ai[0];

        public override string Texture => "Synergia/Assets/Textures/Glow";

        public override void SetDefaults()
        {
            Projectile.width = 64;
            Projectile.height = 64;
            Projectile.timeLeft = 13;
            Projectile.penetrate = -1;
            Projectile.damage = 120;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Default;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
        }

        public override void AI()
        {
            Timer++;
            Projectile.scale = MathHelper.Lerp(0.35f, 1.45f, Timer / 5f);
            Projectile.rotation += 0.17f;

            if (Timer == 1)
            {
                SoundStyle boom = Reassures.Reassures.RSounds.BigBoom;
                boom.Volume = 0.7f;
                boom.PitchVariance = 0.18f;
                SoundEngine.PlaySound(boom, Projectile.Center);
            }

            Lighting.AddLight(Projectile.Center, new Vector3(0.52f, 0.18f, 0.88f) * 1.25f);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch sb = Main.spriteBatch;
            Texture2D glowTex = ModContent.Request<Texture2D>("Synergia/Assets/Textures/Glow").Value;
            Texture2D coreTex = ModContent.Request<Texture2D>("Synergia/Assets/Textures/LightTrail_1").Value;
            Vector2 drawPos = Projectile.Center - Main.screenPosition;
            float fade = MathHelper.Clamp(1f - Timer / 10.5f, 0f, 1f);
            float pulse = 1f + MathF.Sin(Timer * 0.6f) * 0.1f;

            Color outer = new Color(65, 15, 95) * fade * 0.75f;
            Color mid = new Color(125, 35, 190) * fade * 0.85f;
            Color core = Color.White * fade * 0.8f;

            sb.End();
            sb.Begin(SpriteSortMode.Deferred, BlendState.Additive, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

            sb.Draw(glowTex, drawPos, null, outer, -Projectile.rotation * 0.45f, glowTex.Size() / 2f, Projectile.scale * 1.35f, SpriteEffects.None, 0f);
            sb.Draw(glowTex, drawPos, null, mid, Projectile.rotation * 0.4f, glowTex.Size() / 2f, Projectile.scale * 0.95f, SpriteEffects.None, 0f);
            sb.Draw(coreTex, drawPos, null, core * 0.4f, Projectile.rotation * 0.3f, coreTex.Size() / 2f, Projectile.scale * 0.85f * pulse, SpriteEffects.None, 0f);
            sb.Draw(coreTex, drawPos, null, core * 0.55f, -Projectile.rotation * 0.22f, coreTex.Size() / 2f, Projectile.scale * 0.6f * pulse, SpriteEffects.None, 0f);
            sb.Draw(coreTex, drawPos, null, core, 0f, coreTex.Size() / 2f, Projectile.scale * 0.38f, SpriteEffects.None, 0f);

            sb.End();
            sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

            return false;
        }
    }
}