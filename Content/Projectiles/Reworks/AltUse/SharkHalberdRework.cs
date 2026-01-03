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

namespace Synergia.Content.Projectiles.Reworks.AltUse
{
    public class SharkHalberdRework : ModProjectile
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
                    int dd = Dust.NewDust(npc.position, npc.width, npc.height, DustID.Blood,
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

                    // SnowCrystal projectile removed
                    // Projectile.NewProjectile code commented out
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
                        int dd = Dust.NewDust(spawn, 10, 10, DustID.Blood,
                            Main.rand.NextFloat(-2, 2), Main.rand.NextFloat(-2, 2));
                        Main.dust[dd].noGravity = true;
                        Main.dust[dd].scale = 1.2f;
                    }

                    // SnowCrystal projectile removed
                    // Projectile.NewProjectile code commented out
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
}