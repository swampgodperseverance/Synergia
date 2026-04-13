using System;
using System.Diagnostics.Metrics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Synergia.Content.Dusts;
using Synergia.Content.Projectiles.Aura;
using Synergia.Content.Projectiles.Other;
using Synergia.Helpers;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Content.Items.Weapons.Melee
{
    public class Malebolge : ModItem
    {
        public int counter = 0;

        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.damage = 84;
            Item.DamageType = DamageClass.Melee;
            Item.width = 102;
            Item.height = 118;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.knockBack = 0;
            Item.value = Item.sellPrice(0, 3, 50);
            Item.rare = ItemRarityID.Pink;
            Item.autoReuse = false;
            Item.useTurn = false;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.shoot = 1;
            Item.shootSpeed = 5f;
            Item.UseSound = SoundID.Item1;
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            type = 0;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            var proj = Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<MalebolgeProj>(), Item.damage, Item.knockBack, player.whoAmI);
            Main.projectile[proj].ai[0] = 0;
            return true;
        }

        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            Texture2D texture = TextureAssets.Item[Type].Value;
            Texture2D glow = ModContent.Request<Texture2D>(Texture + "_Glow").Value;

            spriteBatch.Draw(texture, position, frame, drawColor, 0f, origin, scale, SpriteEffects.None, 0f);
            spriteBatch.Draw(glow, position, frame, Color.White, 0f, origin, scale, SpriteEffects.None, 0f);

            return false;
        }

        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            Texture2D texture = TextureAssets.Item[Type].Value;
            Texture2D glow = ModContent.Request<Texture2D>(Texture + "_Glow").Value;

            Vector2 position = Item.position - Main.screenPosition + Item.Size * 0.5f;

            spriteBatch.Draw(texture, position, null, lightColor, rotation, texture.Size() * 0.5f, scale, SpriteEffects.None, 0f);
            spriteBatch.Draw(glow, position, null, Color.White, rotation, texture.Size() * 0.5f, scale, SpriteEffects.None, 0f);

            return false;
        }
    }

    public class MalebolgeProj : HeavyWeaponProj
    {
        public override void SetStats(ref float length, ref float speed, ref int comboTimer, ref int width, ref int height)
        {
            length = 78;
            speed = 0.73f;
            comboTimer = 30;
            width = 118;
            height = 102;
        }
        public int hitCounter = 0;
        public float glowPulse = 0f;
        public float fade = 1f;
        public int trailTimer = 0;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 3;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void PostAI()
        {
            var rot = Projectile.ai[0] == 2 ? -90f : 90f;
            var pos = Projectile.Center + new Vector2((float)Math.Sqrt(width * width + height * height), 0f)
                .RotatedBy(Projectile.rotation + MathHelper.ToRadians(rot));

            player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full,
                (player.Center - pos).ToRotation() + MathHelper.PiOver2);

            glowPulse = MathHelper.Lerp(glowPulse, 0f, 0.1f);

            float life = Projectile.timeLeft / 30f;
            fade = MathHelper.Clamp(life, 0f, 1f);

            trailTimer++;
            if (trailTimer >= 4)
            {
                trailTimer = 0;

                var v = SynegiaHelper.PolarVector(15, (Projectile.Center - player.Center).ToRotation());
                Vector2 trailPos = Projectile.Center - v;

                for (int i = 0; i < Projectile.oldPos.Length; i++)
                {
                    if (Projectile.oldPos[i] == Vector2.Zero) continue;

                    float intensity = 1f - (i / (float)Projectile.oldPos.Length);
                    if (Main.rand.NextFloat() < intensity * 0.5f)
                    {
                        Vector2 oldPos = Projectile.oldPos[i] - v;
                        Vector2 dustPos = oldPos + Main.rand.NextVector2Circular(Projectile.width * 0.3f, Projectile.height * 0.3f);

                        Dust d = Dust.NewDustDirect(dustPos, 2, 2, ModContent.DustType<RingDust>());
                        d.velocity = Main.rand.NextVector2Circular(1.2f, 1.2f);
                        d.scale = Main.rand.NextFloat(0.1f, 0.3f) * intensity;
                        d.color = new Color(255, 100 + Main.rand.Next(50), 40 + Main.rand.Next(30));
                        d.noGravity = true;
                        d.rotation = Main.rand.NextFloat(MathHelper.TwoPi);
                        d.fadeIn = 0.2f;

                        Dust d2 = Dust.NewDustDirect(dustPos, 2, 2, DustID.Torch);
                        d2.velocity = Main.rand.NextVector2Circular(1.5f, 1.5f);
                        d2.scale = Main.rand.NextFloat(0.8f, 1.2f) * intensity;
                        d2.noGravity = true;
                    }
                }
            }

            if (fade < 0.35f)
            {
                float intensity = 1f - fade;

                if (Main.rand.NextFloat() < intensity)
                {
                    var v = SynegiaHelper.PolarVector(15, (Projectile.Center - player.Center).ToRotation());

                    Vector2 edge = new Vector2(
                        Main.rand.NextFloat(-width * 0.5f, width * 0.5f),
                        Main.rand.NextFloat(-height * 0.5f, height * 0.5f)
                    ).RotatedBy(Projectile.rotation);

                    Vector2 spawnPos = Projectile.Center - v + edge;

                    Vector2 dir = (spawnPos - (Projectile.Center - v)).SafeNormalize(Vector2.UnitY);

                    Dust d = Dust.NewDustDirect(spawnPos, 2, 2, DustID.Torch);
                    d.velocity = dir * Main.rand.NextFloat(1.5f, 3.5f);
                    d.scale = Main.rand.NextFloat(1.1f, 1.7f) * intensity;
                    d.noGravity = true;
                }

                if (Main.rand.NextBool(3))
                {
                    Vector2 smokePos = Projectile.Center + Main.rand.NextVector2Circular(Projectile.width * 0.3f, Projectile.height * 0.3f);

                    Dust d = Dust.NewDustDirect(smokePos, 2, 2, DustID.Smoke);
                    d.velocity = Main.rand.NextVector2Circular(1.2f, 1.2f);
                    d.scale = Main.rand.NextFloat(0.8f, 1.2f);
                }
            }

            base.PostAI();
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            hitCounter++;

            glowPulse = 1f;

            Projectile.NewProjectile(
                Projectile.GetSource_OnHit(target),
                target.Center,
                Vector2.Zero,
                ModContent.ProjectileType<MalebolgeBoom>(),
                Projectile.damage,
                0f,
                Projectile.owner
            );

            if (hitCounter >= 5)
            {
                hitCounter = 0;

                int amount = Main.rand.Next(2, 5);

                for (int i = 0; i < amount; i++)
                {
                    Vector2 spawnPos = target.Center + new Vector2(Main.rand.Next(-300, 300), -600f);
                    Vector2 velocity = (target.Center - spawnPos).SafeNormalize(Vector2.UnitY) * Main.rand.NextFloat(10f, 16f);

                    Projectile.NewProjectile(
                        Projectile.GetSource_OnHit(target),
                        spawnPos,
                        velocity,
                        ModContent.ProjectileType<MalebolgeMeteor>(),
                        Projectile.damage,
                        4f,
                        Projectile.owner
                    );
                }
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            var player = Main.player[Projectile.owner];
            var spriteEffects = Projectile.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            var spriteEffects2 = Projectile.ai[0] == 1 || Projectile.ai[0] == 5 ? SpriteEffects.FlipVertically : SpriteEffects.None;

            var texture = TextureAssets.Projectile[Projectile.type].Value;
            var glow = ModContent.Request<Texture2D>(Texture + "_Glow").Value;

            Rectangle rect = new(0, 0, texture.Width, texture.Height);
            Vector2 origin = new(texture.Width / 2f, texture.Height / 2f);
            var v = SynegiaHelper.PolarVector(15, (Projectile.Center - player.Center).ToRotation());

            float pulseScale = 1f + glowPulse * 0.35f;
            Color glowColor = Color.Lerp(Color.OrangeRed, Color.White, glowPulse) * fade;

            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                var drawPos = Projectile.oldPos[k] - v - Main.screenPosition + origin;
                var color = glowColor * 0.5f * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);

                Main.EntitySpriteDraw(glow, drawPos, rect, color, Projectile.rotation, origin,
                    Projectile.scale * pulseScale, spriteEffects | spriteEffects2);
            }

            Main.EntitySpriteDraw(texture,
                Projectile.Center - v - Main.screenPosition,
                rect,
                Projectile.GetAlpha(lightColor) * fade,
                Projectile.rotation,
                origin,
                Projectile.scale,
                spriteEffects | spriteEffects2);

            Main.EntitySpriteDraw(glow,
                Projectile.Center - v - Main.screenPosition,
                rect,
                Color.White * fade,
                Projectile.rotation,
                origin,
                Projectile.scale * pulseScale,
                spriteEffects | spriteEffects2);

            return false;
        }

        public override void OnClick(Player player)
        {
            base.OnClick(player);
        }

        public override bool PreKill(int timeLeft)
        {
            return base.PreKill(timeLeft);
        }

        public override void OnKill(int timeLeft)
        {
            var texture = TextureAssets.Projectile[Projectile.type].Value;
            var player = Main.player[Projectile.owner];
            var v = SynegiaHelper.PolarVector(15, (Projectile.Center - player.Center).ToRotation());

            for (int i = 0; i < 30; i++)
            {
                Vector2 pos = Projectile.Center - v + Main.rand.NextVector2Circular(Projectile.width * 0.4f, Projectile.height * 0.4f);

                Dust d = Dust.NewDustDirect(pos, 2, 2, DustID.Torch);
                d.velocity = Main.rand.NextVector2Circular(3f, 3f);
                d.scale = Main.rand.NextFloat(1.2f, 1.8f);
                d.noGravity = true;
            }

            for (int i = 0; i < 20; i++)
            {
                Vector2 pos = Projectile.Center - v + Main.rand.NextVector2Circular(Projectile.width * 0.3f, Projectile.height * 0.3f);

                Dust d = Dust.NewDustDirect(pos, 2, 2, DustID.Smoke);
                d.velocity = Main.rand.NextVector2Circular(2f, 2f);
                d.scale = Main.rand.NextFloat(0.8f, 1.3f);
            }
        }
    }
    public class MalebolgeMeteor : ModProjectile
    {
        private const string GlowTexture = "Synergia/Content/Items/Weapons/Melee/MalebolgeMeteor_Glow";

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 12;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 300;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Melee;
        }

        public override void AI()
        {
            Projectile.velocity.Y += 0.45f;

            if (Projectile.velocity.Y > 18f)
                Projectile.velocity.Y = 18f;

            Projectile.rotation += Projectile.velocity.X * 0.05f;

            Projectile.spriteDirection = Projectile.direction;

            if (Main.rand.NextBool(2))
            {
                Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Torch);
                d.velocity *= 0.3f;
                d.scale = 1.3f;
                d.noGravity = true;
            }

            Projectile.localAI[2] += 0.08f;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Explode();
            return true;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Explode();
        }

        private void Explode()
        {
            SoundEngine.PlaySound(SoundID.Item14, Projectile.position);

            for (int i = 0; i < 25; i++)
            {
                Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Torch);
                d.velocity *= 2f;
                d.scale = 1.5f;
                d.noGravity = true;
            }

            Projectile.Kill();
        }
        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item14, Projectile.Center);

            for (int i = 0; i < 40; i++)
            {
                Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Torch);
                d.velocity = Main.rand.NextVector2Circular(4f, 4f);
                d.scale = 1.6f;
                d.noGravity = true;
            }

            for (int i = 0; i < 20; i++)
            {
                Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Smoke);
                d.velocity = Main.rand.NextVector2Circular(2f, 2f);
                d.scale = 1.2f;
            }

            Projectile.NewProjectile(
                Projectile.GetSource_Death(),
                Projectile.Center,
                Vector2.Zero,
                ModContent.ProjectileType<MalebolgeBoom>(),
                Projectile.damage,
                0f,
                Projectile.owner
            );
        }
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch spriteBatch = Main.spriteBatch;
            Texture2D texture = ModContent.Request<Texture2D>(Texture).Value;
            Texture2D glow = ModContent.Request<Texture2D>(GlowTexture).Value;
            Vector2 drawOrigin = texture.Size() * 0.5f;
            SpriteEffects effects = Projectile.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = Projectile.oldPos[k] + Projectile.Size / 2f - Main.screenPosition;
                float progress = (Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length;

                Color color = Color.BlueViolet * 0.2f * progress;

                float rotation;
                if (k + 1 >= Projectile.oldPos.Length)
                    rotation = (Projectile.position - Projectile.oldPos[k]).ToRotation() + MathHelper.PiOver2;
                else
                    rotation = (Projectile.oldPos[k + 1] - Projectile.oldPos[k]).ToRotation() + MathHelper.PiOver2;

                spriteBatch.Draw(texture, drawPos, null, color, rotation, drawOrigin, Projectile.scale * progress, effects, 0f);
            }

            spriteBatch.Draw(texture,
                Projectile.Center - Main.screenPosition,
                null,
                lightColor,
                Projectile.rotation,
                drawOrigin,
                Projectile.scale,
                effects,
                0f);

            spriteBatch.Draw(glow,
                Projectile.Center - Main.screenPosition,
                null,
                Color.White,
                Projectile.rotation,
                glow.Size() * 0.5f,
                Projectile.scale,
                effects,
                0f);

            return false;
        }
    }
    public class MalebolgeBoom : ModProjectile
    {
        private ref float Timer => ref Projectile.ai[0];

        public override string Texture => "Synergia/Assets/Textures/Glow";

        public override void SetDefaults()
        {
            Projectile.width = 64;
            Projectile.height = 64;
            Projectile.timeLeft = 16;
            Projectile.penetrate = -1;
            Projectile.damage = 80;
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

            Projectile.scale = MathHelper.Lerp(0.35f, 1.1f, Timer / 5f);
            Projectile.rotation += 0.18f;

            if (Timer == 1)
            {
                Projectile.Damage();

                SoundStyle boom = Reassures.Reassures.RSounds.BigBoom;
                boom.Volume = 0.5f;
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