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
    public class ParadoxSwordRework : ModProjectile
    {
        private int localHitCount = 0;
        private bool localEmpowered = false;
        private Vector2 dir = Vector2.Zero;
        private Vector2 hlende = Vector2.Zero; 
        private int a;

        public static int hitcount = 0;
        public static bool empowered = false;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10; 
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 80;
            Projectile.height = 74;
            Projectile.friendly = true;
            Projectile.timeLeft = 12;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.ownerHitCheck = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float rotationFactor = Projectile.rotation + MathF.PI / 4f;
            float scaleFactor = 120f;
            float widthMultiplier = 23f;
            float collisionPoint = 0f;

            Rectangle lanceHitboxBounds = new Rectangle(0, 0, 284, 284);
            lanceHitboxBounds.X = (int)Projectile.position.X - lanceHitboxBounds.Width / 2;
            lanceHitboxBounds.Y = (int)Projectile.position.Y - lanceHitboxBounds.Height / 2;

            Vector2 hitLineEnd = Projectile.Center + rotationFactor.ToRotationVector2() * -scaleFactor;
            hlende = hitLineEnd;

            if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), 
                Projectile.Center, hitLineEnd, widthMultiplier * Projectile.scale, ref collisionPoint))
            {
                return true;
            }
            return false;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Main.instance.LoadProjectile(Projectile.type);
            Texture2D proj = TextureAssets.Projectile[Type].Value;
            Vector2 drawOrigin = new Vector2(proj.Width * 0.5f, Projectile.height * 0.5f);

            if (localEmpowered)
            {
                float trailAlpha = 0.8f;
                
                for (int k = 0; k < Projectile.oldPos.Length; k++)
                {
                    if (k == 0) continue; 
                    
                    float alpha = trailAlpha * (1f - (float)k / Projectile.oldPos.Length);
                    
                    Color trailColor = new Color(255, 50, 50) * alpha;
                    trailColor.A = 0;
                    
                    Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);

                    if (k < 3)
                    {
                        trailColor = new Color(255, 100, 100) * (alpha * 1.5f);
                        trailColor.A = 0;
                    }
                    
                    if (Projectile.ai[2] == 1)
                        Main.EntitySpriteDraw(proj, drawPos, null, trailColor, 
                            Projectile.oldRot[k] - 0.78f, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
                    else
                        Main.EntitySpriteDraw(proj, drawPos, null, trailColor, 
                            Projectile.oldRot[k] - 0.78f, drawOrigin, Projectile.scale, SpriteEffects.FlipHorizontally, 0);
                }
                
                if (Main.rand.NextBool(3))
                {
                    Color glowColor = new Color(255, 100, 100, 128);
                    Vector2 glowPos = Projectile.Center - Main.screenPosition;
                    float glowScale = Projectile.scale * 1.1f;
                    
                    if (Projectile.ai[2] == 1)
                        Main.EntitySpriteDraw(proj, glowPos, null, glowColor, 
                            Projectile.rotation - 0.78f, proj.Size() / 2, glowScale, SpriteEffects.None, 0);
                    else
                        Main.EntitySpriteDraw(proj, glowPos, null, glowColor, 
                            Projectile.rotation - 0.78f, proj.Size() / 2, glowScale, SpriteEffects.FlipHorizontally, 0);
                }
            }
            
            if (Projectile.ai[2] == 1)
                Main.EntitySpriteDraw(proj, Projectile.Center - Main.screenPosition, null, 
                    localEmpowered ? new Color(255, 200, 200) : lightColor, 
                    Projectile.rotation - 0.78f, proj.Size() / 2, Projectile.scale, SpriteEffects.None, 0);
            else
                Main.EntitySpriteDraw(proj, Projectile.Center - Main.screenPosition, null, 
                    localEmpowered ? new Color(255, 200, 200) : lightColor, 
                    Projectile.rotation - 0.78f, proj.Size() / 2, Projectile.scale, SpriteEffects.FlipHorizontally, 0);

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
            hitcount = localHitCount; 

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
                    
                    if (localEmpowered)
                    {
                        dd = Dust.NewDust(npc.position, npc.width, npc.height, DustID.RedTorch,
                            Main.rand.NextFloat(-5, 5), Main.rand.NextFloat(-5, 5));
                        Main.dust[dd].scale = Main.rand.NextFloat(1.2f, 2f);
                        Main.dust[dd].noGravity = true;
                    }
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
                empowered = true; 
                modp.everIceEmpowered = true;
                
                SoundEngine.PlaySound(SoundID.Item45 with { Volume = 1.3f, Pitch = 0.2f }, Projectile.Center);
                
                for (int d = 0; d < 30; d++)
                {
                    int dd = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 
                        DustID.RedTorch, Main.rand.NextFloat(-5, 5), Main.rand.NextFloat(-5, 5));
                    Main.dust[dd].scale = Main.rand.NextFloat(1.5f, 2.5f);
                    Main.dust[dd].noGravity = true;
                    Main.dust[dd].velocity *= 2f;
                }
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
                            Main.rand.NextFloat(-4, 4), Main.rand.NextFloat(-4, 4));
                        Main.dust[dd].noGravity = true;
                        Main.dust[dd].scale = 1.5f;
                        
                        dd = Dust.NewDust(spawn, 10, 10, DustID.RedTorch,
                            Main.rand.NextFloat(-6, 6), Main.rand.NextFloat(-6, 6));
                        Main.dust[dd].noGravity = true;
                        Main.dust[dd].scale = 2f;
                    }

                    // SnowCrystal projectile removed
                    // Projectile.NewProjectile code commented out
                }
                
                for (int i = 0; i < 20; i++)
                {
                    int dust = Dust.NewDust(npc.position, Projectile.width, Projectile.height, 
                        DustID.Blood, Main.rand.NextFloat(-8, 8), Main.rand.NextFloat(-8, 8));
                    Main.dust[dust].velocity *= 4f;
                    Main.dust[dust].scale = Main.rand.NextFloat(1.5f, 2.5f);
                }
                
                SoundEngine.PlaySound(SoundID.Item4 with { Volume = 1.5f, Pitch = 0.3f }, Projectile.Center);

                localHitCount = 0;
                localEmpowered = false;
                empowered = false; 
                hitcount = 0; 
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
            writer.WriteVector2(hlende);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            localHitCount = reader.ReadInt32();
            localEmpowered = reader.ReadBoolean();
            a = reader.ReadInt32();
            hlende = reader.ReadVector2();
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

            Projectile.ai[0] += Projectile.ai[2] <= 8 ? 0.15f : 0.3f;
            Projectile.ai[1] += Projectile.ai[2] <= 8 ? 0.15f : 0.3f;

            if (a >= 6 && a <= 10) Projectile.scale += localEmpowered ? 0.25f : 0.15f;  
            if (a >= 10 && a <= 12) Projectile.scale -= localEmpowered ? 0.35f : 0.3f;

            if (a == 10 && !localEmpowered) 
                SoundEngine.PlaySound(SoundID.Item1 with { Volume = 1.1f, Pitch = -0.2f }, Projectile.Center);
            if (a == 10 && localEmpowered) 
                SoundEngine.PlaySound(SoundID.Item71 with { Volume = 1.4f, Pitch = 0.2f }, Projectile.Center);

            float rotationMultiplier = localEmpowered ? 2.0f : 1.5f;
            
            if (Projectile.ai[2] == 1)
                Projectile.rotation += Projectile.ai[1] * MathHelper.ToRadians(12 - Projectile.ai[0]) * rotationMultiplier;
            else
                Projectile.rotation += Projectile.ai[1] * MathHelper.ToRadians(-12 - Projectile.ai[0]) * rotationMultiplier;
                
            if (localEmpowered && Main.rand.NextBool(2))
            {
                int dustType = Main.rand.NextBool() ? DustID.RedTorch : DustID.Blood;
                int d = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 
                    dustType, Main.rand.NextFloat(-2, 2), Main.rand.NextFloat(-2, 2));
                Main.dust[d].scale = Main.rand.NextFloat(1f, 1.8f);
                Main.dust[d].noGravity = true;
                Main.dust[d].velocity *= 0.5f;
            }
        }
    }
}