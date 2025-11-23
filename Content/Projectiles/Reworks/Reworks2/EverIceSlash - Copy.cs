using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Synergia.Common.Players;

namespace Synergia.Content.Projectiles.Reworks.Reworks2
{
    public class EverIceSlash : ModProjectile
    {
        private int localHitCount = 0;
        private bool localEmpowered = false;
        Vector2 dir = Vector2.Zero;
        int a;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 120;
            Projectile.height = 110;
            Projectile.friendly = true;
            Projectile.timeLeft = 20;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.ownerHitCheck = true;
            Projectile.DamageType = DamageClass.Melee;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float rot = Projectile.rotation + MathF.PI / 4f;
            Vector2 end = Projectile.Center + rot.ToRotationVector2() * -110f;
            float pt = 0;
            if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(),
                Projectile.Center, end, 23f * Projectile.scale, ref pt))
                return true;
            return false;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Main.instance.LoadProjectile(Projectile.type);
            Texture2D tex = TextureAssets.Projectile[Type].Value;
            Vector2 orig = tex.Size() / 2;

            if (localEmpowered)
            {
                for (int k = 0; k < Projectile.oldPos.Length; k++)
                {
                    Color c = lightColor;
                    c.A = 0;
                    Vector2 pos = Projectile.oldPos[k] - Main.screenPosition + orig + new Vector2(0, Projectile.gfxOffY);
                    Main.EntitySpriteDraw(tex, pos, null, c, Projectile.oldRot[k] - 0.78f, orig, Projectile.scale, Projectile.ai[2] == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
                }
            }

            Main.EntitySpriteDraw(tex,
                Projectile.Center - Main.screenPosition,
                null,
                lightColor,
                Projectile.rotation - 0.78f,
                orig,
                Projectile.scale,
                Projectile.ai[2] == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None,
                0);

            return false;
        }

        public override void OnHitNPC(NPC npc, NPC.HitInfo hit, int damageDone)
        {
            int nearby = 0;
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC n = Main.npc[i];
                if (n.active && !n.friendly && Vector2.Distance(n.Center, Projectile.Center) < 300f)
                    nearby++;
            }
            float scale = 1f / Math.Max(1f, 1f + (nearby - 1) * 0.4f);
            hit.Damage = (int)(hit.Damage * scale);

            Player player = Main.player[Projectile.owner];
            localHitCount++;

            var modp = player.GetModPlayer<EverIcePlayer>();
            modp.everIceHitCount = localHitCount;

            if (!localEmpowered && localHitCount < 3)
            {
                int count = Main.rand.Next(2, 4);

                for (int d = 0; d < 15; d++)
                {
                    int dd = Dust.NewDust(npc.position, npc.width, npc.height, DustID.Snow,
                        Main.rand.NextFloat(-3, 3), Main.rand.NextFloat(-3, 3));
                    Main.dust[dd].scale = Main.rand.NextFloat(1f, 1.6f);
                    Main.dust[dd].noGravity = true;
                }

                Vector2 dirToNPC = Vector2.Normalize(npc.Center - player.Center);

                float dist = Main.rand.NextFloat(180f, 220f);
                Vector2 basePos = npc.Center + dirToNPC * dist;

                for (int i = 0; i < count; i++)
                {
                    Vector2 offset = dirToNPC.RotatedBy(Main.rand.NextFloat(-0.75f, 0.75f)) 
                                     * Main.rand.NextFloat(80f, 140f);

                    Vector2 spawn = basePos + offset;

                    int c = Projectile.NewProjectile(Projectile.GetSource_FromThis(), spawn, Vector2.Zero,
                        ModContent.ProjectileType<SnowCrystal>(), Projectile.damage, Projectile.knockBack,
                        player.whoAmI, Main.rand.NextFloat(0f, MathHelper.TwoPi), npc.whoAmI);
                    Main.projectile[c].timeLeft = 70;
                }
            }

            if (localHitCount >= 3 && !localEmpowered)
            {
                localEmpowered = true;
                modp.everIceEmpowered = true;
                SoundEngine.PlaySound(SoundID.Item45, Projectile.Center);
            }

            if (localHitCount >= 4)
            {
                Vector2 dirToNPC = Vector2.Normalize(npc.Center - player.Center);
                Vector2 behind = npc.Center + dirToNPC * 120f;

                for (int i = 0; i < 3; i++)
                {
                    float off = (i - 1) * 90f;
                    Vector2 spawn = behind + dirToNPC.RotatedBy(MathHelper.PiOver2) * off;

                    for (int d = 0; d < 10; d++)
                    {
                        int dd = Dust.NewDust(spawn, 10, 10, DustID.IceTorch,
                            Main.rand.NextFloat(-2, 2), Main.rand.NextFloat(-2, 2));
                        Main.dust[dd].noGravity = true;
                        Main.dust[dd].scale = 1.2f;
                    }

                    int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), spawn, Vector2.Zero,
                        ModContent.ProjectileType<SnowCrystal>(), Projectile.damage * 2, Projectile.knockBack,
                        player.whoAmI, i, npc.whoAmI);

                    Main.projectile[proj].timeLeft = 90;
                }



                localHitCount = 0;
                localEmpowered = false;
                modp.everIceHitCount = 0;
                modp.everIceEmpowered = false;
            }

            Projectile.friendly = false;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(localHitCount);
            writer.Write(localEmpowered);
            writer.Write(a);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            localHitCount = reader.ReadInt32();
            localEmpowered = reader.ReadBoolean();
            a = reader.ReadInt32();
        }

        public override void AI()
        {
            if (dir == Vector2.Zero)
            {
                dir = Main.MouseWorld;
                Projectile.rotation =
                    (MathHelper.PiOver2 * Projectile.ai[2]) - MathHelper.PiOver4 +
                    Projectile.DirectionTo(Main.MouseWorld).ToRotation();
            }

            Projectile.Center = Main.player[Projectile.owner].Center;
            a++;

            Player plr = Main.player[Projectile.owner];
            plr.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full,
                Projectile.rotation + 90);

            Projectile.ai[0] += Projectile.ai[2] <= 8 ? 0.1f : 0.2f;
            Projectile.ai[1] += Projectile.ai[2] <= 8 ? 0.1f : 0.2f;

            if (a >= 9 && a <= 15) Projectile.scale += 0.1f;
            if (a >= 15 && a <= 20) Projectile.scale -= 0.2f;

            if (a == 10 && !localEmpowered) SoundEngine.PlaySound(SoundID.Item1, Projectile.Center);
            if (a == 10 && localEmpowered) SoundEngine.PlaySound(SoundID.Item71, Projectile.Center);

            if (Projectile.ai[2] == 1)
                Projectile.rotation += Projectile.ai[1] * MathHelper.ToRadians(10 - Projectile.ai[0]);
            else
                Projectile.rotation += Projectile.ai[1] * MathHelper.ToRadians(-10 - Projectile.ai[0]);
        }
    }

         public class SnowCrystal : ModProjectile
    {
        int timer = 0;
        int targetNPC = -1;
        float initialRot = 0f;

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 8;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 3;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 28;
            Projectile.height = 42;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 90;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.alpha = 255;
            Projectile.scale = 1f;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
        }

        public override void OnSpawn(IEntitySource source)
        {
            initialRot = Projectile.ai[0];
            targetNPC = (int)Projectile.ai[1];
        }

        public override void AI()
        {
            timer++;
            Projectile.alpha -= 18;
            if (Projectile.alpha < 0) Projectile.alpha = 0;

            Projectile.velocity = new Vector2(
                MathF.Sin(timer * 0.08f + initialRot) * 0.6f,
                MathHelper.Clamp(timer * 0.05f, 1f, 6f));

            if (timer % 4 == 0)
            {
                Projectile.frame++;
                if (Projectile.frame >= Main.projFrames[Projectile.type])
                    Projectile.frame = 0;
            }

            Lighting.AddLight(Projectile.Center, new Vector3(0.6f, 0.85f, 1f) * 0.5f);

            if (timer >= 40 ||
                (targetNPC >= 0 &&
                 (!Main.npc[targetNPC].active || Main.npc[targetNPC].life <= 0)))
            {
                Explode();
            }
        }

        private void Explode()
        {
            if (!Projectile.active) return;

            for (int i = 0; i < 20; i++)
            {
                int d = Dust.NewDust(Projectile.position - new Vector2(8, 8),
                    Projectile.width + 16, Projectile.height + 16,
                    DustID.Snow, Main.rand.NextFloat(-6, 6), Main.rand.NextFloat(-6, 6));
                Main.dust[d].scale = Main.rand.NextFloat(0.8f, 1.6f);
                Main.dust[d].noGravity = true;
                Main.dust[d].velocity *= 1.4f;
            }

            SoundEngine.PlaySound(new SoundStyle("Synergia/Assets/Sounds/CrystallBroke"), Projectile.Center);

            NPC target = null;

            if (targetNPC >= 0 && Main.npc.IndexInRange(targetNPC) && Main.npc[targetNPC].active)
                target = Main.npc[targetNPC];
            else
            {
                float md = 800f;
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC n = Main.npc[i];
                    if (n.active && !n.friendly)
                    {
                        float dist = Vector2.Distance(n.Center, Projectile.Center);
                        if (dist < md)
                        {
                            md = dist;
                            target = n;
                        }
                    }
                }
            }

            if (target != null)
            {
                Vector2 dir = target.Center - Projectile.Center;
                dir.Normalize();
                dir *= 12f;
                int p = Projectile.NewProjectile(Projectile.GetSource_FromThis(),
                    Projectile.Center, dir,
                    ModContent.ProjectileType<SmallIceSword>(),
                    Projectile.damage, Projectile.knockBack, Projectile.owner);
                Main.projectile[p].timeLeft = 60;
            }
            else
            {
                for (int i = 0; i < 6; i++)
                {
                    Vector2 vel = new Vector2(Main.rand.NextFloat(-3f, 3f),
                                              Main.rand.NextFloat(-6f, -1f));
                    int d = Projectile.NewProjectile(Projectile.GetSource_FromThis(),
                        Projectile.Center, vel,
                        ModContent.ProjectileType<SmallIceSword>(), 0, 0, Projectile.owner);
                    Main.projectile[d].timeLeft = 30;
                }
            }

            Projectile.Kill();
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 origin = new(tex.Width / 2f, tex.Height / 8f / 2f);
            Rectangle src = tex.Frame(1, 8, 0, Projectile.frame);

            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                float alpha = (Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length;
                Color c = new Color(100, 170, 255) * alpha * 0.6f;
                Vector2 pos = Projectile.oldPos[k] - Main.screenPosition + origin;
                Main.spriteBatch.Draw(tex, pos, src, c, Projectile.oldRot[k], origin, Projectile.scale, SpriteEffects.None, 0);
            }

            Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, src,
                Projectile.GetAlpha(lightColor), Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0);

            return false;
        }
    }

        public class SmallIceSword : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 8;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }
        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 40;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 60;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.alpha = 255;
            Projectile.scale = 1f;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            // fade in
            if (Projectile.alpha > 0) Projectile.alpha -= 25;
            if (Projectile.alpha < 0) Projectile.alpha = 0;

            // dust trail
            if (Main.rand.NextBool(2))
            {
                int d = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Snow);
                Main.dust[d].velocity *= 0.2f;
                Main.dust[d].scale = Main.rand.NextFloat(0.8f, 1.2f);
                Main.dust[d].noGravity = true;
            }

            Lighting.AddLight(Projectile.Center, new Vector3(0.6f, 0.8f, 1f) * 0.3f);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 origin = new Vector2(tex.Width / 2f, tex.Height / 2f);
            // draw trail
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + origin + new Vector2(0f, Projectile.gfxOffY);
                Color c = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
                Main.EntitySpriteDraw(tex, drawPos, null, c, Projectile.oldRot[k], origin, Projectile.scale * (1f - k * 0.06f), SpriteEffects.None, 0);
            }
            return true;
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 12; i++)
            {
                int d = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Snow);
                Main.dust[d].velocity *= 2f;
                Main.dust[d].scale = Main.rand.NextFloat(0.8f, 1.4f);
                Main.dust[d].noGravity = true;
            }
            SoundEngine.PlaySound(SoundID.Item27, Projectile.position);
            Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
        }
    }
}