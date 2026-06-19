using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Synergia.Trails;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.Graphics;
using Terraria.Graphics.CameraModifiers;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Content.Projectiles.Friendly
{
    public class BlasphemousHeavensProj : ModProjectile
    {
        private readonly VertexStrip vertexStrip = new VertexStrip();
        private bool framePicked;
        private int returnTimer = 0;

        private const int NormalStarCount = 4;
        private const int FastStarCount = 2;
        private int[] NormalStarWhoAmI = new int[NormalStarCount];
        private int[] FastStarWhoAmI = new int[FastStarCount];

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 18;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 36;
            Projectile.height = 36;
            Projectile.friendly = true;
            Projectile.timeLeft = 180;
            Projectile.penetrate = 5;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.extraUpdates = 2;
            Projectile.scale = 1.12f;
        }

        public override void OnSpawn(IEntitySource source)
        {
            for (int i = 0; i < NormalStarCount; i++)
            {
                int star = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero,
                    ModContent.ProjectileType<BlasphemousStar>(), Projectile.damage / 2, 0.5f, Projectile.owner);

                if (star >= 0)
                {
                    Main.projectile[star].ai[1] = i * MathHelper.TwoPi / NormalStarCount;
                    NormalStarWhoAmI[i] = star;
                }
            }

            for (int i = 0; i < FastStarCount; i++)
            {
                int star = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero,
                    ModContent.ProjectileType<BlasphemousLightningStar>(), Projectile.damage / 2, 0.5f, Projectile.owner);

                if (star >= 0)
                {
                    Main.projectile[star].ai[1] = i * MathHelper.TwoPi / FastStarCount + MathHelper.Pi;
                    FastStarWhoAmI[i] = star;
                }
            }
        }

        public override void AI()
        {
            if (!framePicked)
            {
                Projectile.frame = Main.rand.Next(Main.projFrames[Projectile.type]);
                framePicked = true;
            }

            Projectile.rotation += 0.38f;
            returnTimer++;

            if (returnTimer < 45)
            {
                if (Projectile.velocity.Length() < 19f)
                    Projectile.velocity = Projectile.velocity.SafeNormalize(Vector2.Zero) * 19f;
            }
            else
            {
                Player owner = Main.player[Projectile.owner];
                Vector2 toOwner = owner.Center - Projectile.Center;
                float dist = toOwner.Length();

                if (dist > 30f)
                {
                    toOwner.Normalize();
                    float returnSpeed = MathHelper.Lerp(15f, 29f, (returnTimer - 45) / 60f);
                    Projectile.velocity = Vector2.Lerp(Projectile.velocity, toOwner * returnSpeed, 0.13f);
                }
                else
                {
                    Projectile.Kill();
                }
            }

            if (returnTimer > 60)
                Projectile.velocity *= 0.97f;

            if (Projectile.timeLeft < 35)
            {
                KillAllStars();
            }
        }

        private void KillAllStars()
        {
            foreach (int who in NormalStarWhoAmI)
                if (who >= 0 && Main.projectile[who].active) Main.projectile[who].Kill();
            foreach (int who in FastStarWhoAmI)
                if (who >= 0 && Main.projectile[who].active) Main.projectile[who].Kill();
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Vector2 spawnPos = target.Center - new Vector2(0, target.height * 0.5f + 100f);
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), spawnPos, Vector2.Zero,
                ModContent.ProjectileType<SuperboltFriendly>(), Projectile.damage * 2, 0f, Projectile.owner);
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 20; i++)
            {
                Dust d = Dust.NewDustPerfect(Projectile.Center, 267, Main.rand.NextVector2Circular(5f, 5f),
                    0, new Color(100, 230, 255), 1.4f);
                d.noGravity = true;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch sb = Main.spriteBatch;
            float fade = Projectile.timeLeft < 35 ? Projectile.timeLeft / 35f : 1f;

            sb.BeginBlendState(BlendState.Additive, isUI2: true);
            GameShaders.Misc["MagicMissile"].Apply();

            vertexStrip.PrepareStripWithProceduralPadding(
                Projectile.oldPos,
                Projectile.oldRot,
                p => Color.Lerp(new Color(120, 240, 255), new Color(200, 255, 255), p).MultiplyAlpha(fade * 0.9f),
                p => 44f * Projectile.scale * (1f - p * 0.65f),
                -Main.screenPosition + Projectile.Size / 2,
                true
            );
            vertexStrip.DrawTrail();
            Main.pixelShader.CurrentTechnique.Passes[0].Apply();
            sb.End();

            sb.Begin(SpriteSortMode.Deferred, BlendState.Additive, SamplerState.LinearClamp,
                DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);

            Texture2D tex = ModContent.Request<Texture2D>(Texture).Value;
            Rectangle frame = tex.Frame(1, Main.projFrames[Projectile.type], 0, Projectile.frame);
            Vector2 drawPos = Projectile.Center - Main.screenPosition;
            float rot = Projectile.rotation;

            Color outlineColor = new Color(80, 220, 255);

            for (int i = 0; i < 6; i++)
            {
                float offset = 2.5f + i * 0.6f;
                float alpha = (0.75f - i * 0.11f) * fade;

                for (int k = 0; k < 4; k++)
                {
                    Vector2 offsetVec = new Vector2(offset, 0).RotatedBy(k * MathHelper.PiOver2 + i * 0.3f);
                    sb.Draw(tex, drawPos + offsetVec, frame, outlineColor * alpha,
                        rot, frame.Size() / 2f, Projectile.scale * 1.05f, SpriteEffects.None, 0f);
                }
            }

            sb.End();

            sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp,
                DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);

            sb.Draw(tex, drawPos, frame, new Color(180, 245, 255, 255),
                rot, frame.Size() / 2f, Projectile.scale, SpriteEffects.None, 0f);

            sb.Draw(tex, drawPos, frame, Color.White * 0.75f,
                rot, frame.Size() / 2f, Projectile.scale * 0.8f, SpriteEffects.None, 0f);

            return false;
        }
    }

    public class BlasphemousStar : ModProjectile
    {
        private readonly VertexStrip vertexStrip = new VertexStrip();

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 13;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 18;
            Projectile.friendly = true;
            Projectile.penetrate = 2;
            Projectile.timeLeft = 240;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.extraUpdates = 1;
            Projectile.scale = 0.95f;
            Projectile.alpha = 255;
        }

        public override void AI()
        {
            if (!IsParentAlive()) { Projectile.Kill(); return; }

            Projectile projParent = Main.projectile[Projectile.owner];

            float orbitRadius = 29f;
            Projectile.ai[0] += 0.165f;
            Vector2 offset = new Vector2(orbitRadius, 0).RotatedBy(Projectile.ai[0] + Projectile.ai[1]);

            Vector2 desired = projParent.Center + offset;
            Projectile.Center = Vector2.Lerp(Projectile.Center, desired, 0.38f);
            Projectile.velocity = (desired - Projectile.Center) * 0.85f;

            Projectile.rotation += 0.28f;
        }

        private bool IsParentAlive()
        {
            int parentType = ModContent.ProjectileType<BlasphemousHeavensProj>();
            return Projectile.ai[2] >= 0 && Main.projectile[(int)Projectile.ai[2]].active &&
                   Main.projectile[(int)Projectile.ai[2]].type == parentType;
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 7; i++)
                Dust.NewDustPerfect(Projectile.Center, 267, Main.rand.NextVector2Circular(3.2f, 3.2f),
                    0, new Color(100, 230, 255), 1.1f).noGravity = true;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch sb = Main.spriteBatch;
            float fade = Projectile.timeLeft < 30 ? Projectile.timeLeft / 30f : 1f;

            sb.BeginBlendState(BlendState.Additive, isUI2: true);
            GameShaders.Misc["MagicMissile"].Apply();

            vertexStrip.PrepareStripWithProceduralPadding(
                Projectile.oldPos, Projectile.oldRot,
                p => new Color(140, 235, 255).MultiplyAlpha(fade * (1f - p) * 0.3f),
                p => 18f * (1f - p * 0.6f),
                -Main.screenPosition + Projectile.Size / 2, true);
            vertexStrip.DrawTrail();
            Main.pixelShader.CurrentTechnique.Passes[0].Apply();
            sb.End();

            sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp,
                DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);

            Texture2D tex = ModContent.Request<Texture2D>(Texture).Value;
            Color transparentColor = new Color(180, 245, 255, 30);
            sb.Draw(tex, Projectile.Center - Main.screenPosition, null, transparentColor,
                Projectile.rotation, tex.Size() / 2f, Projectile.scale, SpriteEffects.None, 0f);
            sb.Draw(tex, Projectile.Center - Main.screenPosition, null, Color.White * 0.1f,
                Projectile.rotation, tex.Size() / 2f, Projectile.scale * 0.8f, SpriteEffects.None, 0f);

            return false;
        }
    }

    public class BlasphemousLightningStar : ModProjectile
    {
        private readonly VertexStrip vertexStrip = new VertexStrip();

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 16;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.friendly = true;
            Projectile.penetrate = 2;
            Projectile.timeLeft = 240;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.extraUpdates = 2;
            Projectile.scale = 0.78f;
            Projectile.alpha = 255;
        }

        public override void AI()
        {
            if (!IsParentAlive())
            {
                Projectile.Kill();
                return;
            }

            Projectile projParent = Main.projectile[(int)Projectile.ai[2]];

            float orbitRadius = 34f;
            Projectile.ai[0] += 0.42f;

            Vector2 offset = new Vector2(orbitRadius, 0).RotatedBy(Projectile.ai[0] + Projectile.ai[1]);
            Vector2 desired = projParent.Center + offset;

            Projectile.Center = Vector2.Lerp(Projectile.Center, desired, 0.45f);
            Projectile.rotation += 0.55f;
        }

        private bool IsParentAlive()
        {
            int parentType = ModContent.ProjectileType<BlasphemousHeavensProj>();
            return Projectile.ai[2] >= 0 && Main.projectile[(int)Projectile.ai[2]].active &&
                   Main.projectile[(int)Projectile.ai[2]].type == parentType;
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 6; i++)
            {
                Dust.NewDustPerfect(Projectile.Center, 267, Main.rand.NextVector2Circular(3.5f, 3.5f),
                    0, new Color(255, 240, 100), 1.15f).noGravity = true;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch sb = Main.spriteBatch;
            float fade = Projectile.timeLeft < 30 ? Projectile.timeLeft / 30f : 1f;
            Vector2 drawPos = Projectile.Center - Main.screenPosition;
            Texture2D tex = ModContent.Request<Texture2D>(Texture).Value;

            sb.BeginBlendState(BlendState.Additive, isUI2: true);
            GameShaders.Misc["MagicMissile"].Apply();

            vertexStrip.PrepareStripWithProceduralPadding(
                Projectile.oldPos,
                Projectile.oldRot,
                p => new Color(255, 245, 120).MultiplyAlpha(fade * (1f - p) * 0.25f),
                p => 17f * (1f - p * 0.6f),
                -Main.screenPosition + Projectile.Size / 2,
                true
            );
            vertexStrip.DrawTrail();
            Main.pixelShader.CurrentTechnique.Passes[0].Apply();
            sb.End();

            sb.Begin(SpriteSortMode.Deferred, BlendState.Additive, SamplerState.LinearClamp,
                DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);

            Color yellowOutline = new Color(255, 245, 110);

            for (int i = 0; i < 5; i++)
            {
                float offset = 1.8f + i * 0.45f;
                float alpha = (0.85f - i * 0.16f) * fade * 0.2f;

                for (int k = 0; k < 4; k++)
                {
                    Vector2 offsetVec = new Vector2(offset, 0).RotatedBy(k * MathHelper.PiOver2 + i * 0.4f);
                    sb.Draw(tex, drawPos + offsetVec, null, yellowOutline * alpha,
                        Projectile.rotation, tex.Size() / 2f, Projectile.scale * 1.08f, SpriteEffects.None, 0f);
                }
            }

            sb.End();

            sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp,
                DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);

            Color transparentYellow = new Color(255, 250, 180, 20);
            sb.Draw(tex, drawPos, null, transparentYellow,
                Projectile.rotation, tex.Size() / 2f, Projectile.scale, SpriteEffects.None, 0f);

            sb.Draw(tex, drawPos, null, Color.White * 0.05f,
                Projectile.rotation, tex.Size() / 2f, Projectile.scale * 0.75f, SpriteEffects.None, 0f);

            return false;
        }
    }

    public class SuperboltFriendly : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.DrawScreenCheckFluff[Type] = 1200;
            if (!ModLoader.TryGetMod("Redemption", out Mod mor)) return;
            mor.Call("addElementProj", 7, base.Projectile.type);
        }
        public override void SetDefaults()
        {
            if (ModLoader.TryGetMod("CalamityMod", out Mod calamity)) calamity.Call("SetDefenseDamageProjectile", Projectile, true);
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.MaxUpdates = Projectile.timeLeft = 300;
        }
        public override void AI()
        {
            if (Projectile.localAI[0] == 0f && Projectile.localAI[1] == 0f)
            {
                Projectile.localAI[0] = Projectile.Center.X;
                Projectile.localAI[1] = Projectile.Center.Y;
                if (Projectile.velocity == Vector2.Zero) Projectile.velocity = Vector2.UnitY;
                Projectile.velocity = Vector2.Normalize(Projectile.velocity) * 8f;
                Main.instance.CameraModifiers.Add(new PunchCameraModifier(Projectile.Center, Vector2.Normalize(Projectile.velocity), 6f, 8, 20, 1200f, "Superbolt"));
                SoundEngine.PlaySound(SoundID.Item94, Projectile.Center);
            }
            if (Projectile.ai[0] > 0f || (Projectile.timeLeft <= 2 && Projectile.ai[0] == 0f))
            {
                Projectile.extraUpdates = 0;
                Projectile.timeLeft = 2;
                Projectile.velocity = Vector2.Zero;
                if (Projectile.ai[0] == 0f) for (int i = 0; i < 10; i++)
                    {
                        Vector2 dustVelocity = Main.rand.NextVector2Circular(Projectile.width, Projectile.height);
                        dustVelocity.Y -= Projectile.height;
                        dustVelocity.Y *= 0.5f;
                        Dust.NewDustPerfect(Projectile.Center + dustVelocity, 226, dustVelocity, 128, default(Color), Projectile.scale * 1.1f);
                    }
                if (Projectile.ai[0] < 20f * Projectile.MaxUpdates) Projectile.ai[0]++;
                else Projectile.active = false;
                if (Projectile.ai[0] == 1f) Main.instance.CameraModifiers.Add(new PunchCameraModifier(Projectile.Center, Vector2.Normalize(new Vector2(Projectile.localAI[0], Projectile.localAI[1]) - Projectile.Center), 6f, 8, 20, 1200f, "Grand Thunder Strike"));
            }
        }
        private List<Vector2> arcPoints;
        public override bool PreDraw(ref Color lightColor)
        {
            if (Projectile.ai[0] <= 0f) return false;
            Vector2 startPos = new Vector2(Projectile.localAI[0], Projectile.localAI[1]);
            Vector2 endPos = Projectile.Center;
            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
            float scale = Vector2.UnitX.RotatedBy(Projectile.ai[0] / (20f * Projectile.MaxUpdates) * MathHelper.Pi).Y * Projectile.scale * 0.65f;
            int arcCount = (int)(Vector2.Distance(startPos, endPos) / 32f);
            if (arcPoints == null)
            {
                arcPoints = new();
                for (int i = 1; i < arcCount; i++) arcPoints.Add(Vector2.SmoothStep(startPos, endPos, (float)i / (float)arcCount) + (i > 1 ? 1f : 0f) * Main.rand.NextVector2Circular(texture.Width, texture.Height));
            }
            lightColor = Color.White;
            for (int i = 0; i < arcPoints.Count; i++)
            {
                if (i > 0) startPos = arcPoints[i - 1];
                Main.EntitySpriteDraw(texture, startPos - Main.screenPosition, null, lightColor, Main.rand.NextFloat(MathHelper.TwoPi), texture.Size() / 2f, scale, SpriteEffects.None, 0);
            }
            Main.EntitySpriteDraw(texture, endPos - Main.screenPosition, null, lightColor, Main.rand.NextFloat(MathHelper.TwoPi), texture.Size() / 2f, scale, SpriteEffects.None, 0);
            startPos = new Vector2(Projectile.localAI[0], Projectile.localAI[1]);
            for (int i = 0; i < arcPoints.Count; i++)
            {
                if (i > 0) startPos = arcPoints[i - 1];
                if (i < arcPoints.Count - 1) endPos = arcPoints[i];
                else endPos = Projectile.Center;
                Main.EntitySpriteDraw(texture, startPos - Main.screenPosition, new Rectangle(0, texture.Height / 2 - 1, texture.Width, 1), lightColor, (startPos - endPos).ToRotation() + MathHelper.PiOver2, new Vector2(texture.Width / 2f, 0f), new Vector2(scale, Vector2.Distance(startPos, endPos)), SpriteEffects.None, 0);
            }
            startPos = new Vector2(Projectile.localAI[0], Projectile.localAI[1]);
            texture = (Texture2D)ModContent.Request<Texture2D>("Terraria/Images/Projectile_238");
            if (++Projectile.frameCounter > Main.projFrames[238] * 3 - 2) Projectile.frameCounter = 0;
            Projectile.frame = Projectile.frameCounter / 3;
            Main.EntitySpriteDraw(texture, startPos - Main.screenPosition, new Rectangle(0, texture.Height / Main.projFrames[238] * Projectile.frame, texture.Width, texture.Height / Main.projFrames[238]), lightColor * MathHelper.Min(scale * 3f, 1f), 0f, new Vector2(texture.Width, texture.Height / Main.projFrames[238]) / 2f, Projectile.scale * 1.25f, SpriteEffects.None, 0);
            return false;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.extraUpdates = 0;
            Projectile.tileCollide = false;
            Projectile.velocity = Vector2.Zero;
            return false;
        }
    }
}