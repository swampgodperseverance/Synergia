using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace Synergia.Content.Projectiles.Reworks
{
    public class FeroziumIcicle : ModProjectile
    {   
        public override void SetStaticDefaults() 
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 7;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }
        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 18;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.tileCollide = true;
            Projectile.aiStyle = -1;
            Projectile.timeLeft = 600;
            Projectile.penetrate = 3;
            Projectile.Opacity = 0f;
            Projectile.extraUpdates = 1;
        }

        public override void OnSpawn(IEntitySource source)
        {
            Projectile.ai[2] = Projectile.velocity.Length();
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = height = 4;
            return true;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

            if (Projectile.Opacity < 1f)
            {
                Projectile.Opacity += 0.1f;
            }

            const float homingRange = 800f;
            //const float homingStrength = 0.09f;
            const float maxSpeedMultiplier = 1.6f;

            NPC closestNPC = null;
            float closestDistance = homingRange;

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                if (npc.active && npc.CanBeChasedBy() && !npc.friendly && !npc.immortal && !npc.dontTakeDamage)
                {
                    float dist = Vector2.Distance(Projectile.Center, npc.Center);
                    if (dist < closestDistance)
                    {
                        closestDistance = dist;
                        closestNPC = npc;
                    }
                }
            }

            float maxSpeed = Projectile.ai[2] * maxSpeedMultiplier;

            if (closestNPC != null)
            {
                Vector2 directionToTarget = closestNPC.Center - Projectile.Center;
                directionToTarget.Normalize();
                Projectile.velocity = (Projectile.velocity * 20f + directionToTarget * maxSpeed) / 21f;
                if (Projectile.velocity.Length() > maxSpeed)
                {
                    Projectile.velocity = Vector2.Normalize(Projectile.velocity) * maxSpeed;
                }
            }
            else
            {
                Projectile.velocity.Y += Projectile.ai[2] * 0.04f;
                Projectile.velocity.Y = MathHelper.Min(18f, Projectile.velocity.Y);
                Projectile.velocity.X *= 0.98f;
            }

   
            if (Main.rand.NextBool(2))
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.IceTorch, 0f, 0f, 150, default, 1.3f);
                dust.velocity *= 0.4f;
                dust.noGravity = true;
                dust.fadeIn = 1.2f;

                if (Main.rand.NextBool(4))
                {
                    Dust frost = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.FrostStaff, 0f, 0f, 100, default, 0.8f);
                    frost.velocity *= 0.2f;
                    frost.noGravity = true;
                }
            }
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item27 with { Volume = 0.7f, Pitch = 0.2f }, Projectile.position);

            for (int i = 0; i < 15; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.IceTorch,
                    Main.rand.NextFloat(-3f, 3f), Main.rand.NextFloat(-3f, 3f), 100, default, Main.rand.NextFloat(1.2f, 1.8f));
                dust.noGravity = true;
                dust.velocity *= 0.8f;

                if (Main.rand.NextBool(3))
                {
                    Dust sparkle = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.FrostStaff,
                        Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-2f, 2f), 50, default, 1f);
                    sparkle.noGravity = true;
                    sparkle.velocity *= 0.5f;
                }
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = ModContent.Request<Texture2D>("Synergia/Assets/Textures/LightTrail_1").Value;
            Vector2 origin = texture.Size() * 0.5f;
            SpriteEffects effects = Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                if (Projectile.oldPos[k] == Vector2.Zero) continue;

                Vector2 drawPos = Projectile.oldPos[k] + Projectile.Size / 2f - Main.screenPosition;
     
                Color color = Color.Lerp(
                    new Color(100, 200, 255),  
                    new Color(180, 240, 255),    
                    k / (float)Projectile.oldPos.Length
                ) * (1f - k / (float)Projectile.oldPos.Length) * 0.7f;

                float scale = Projectile.scale * (1f - k * 0.06f);

                Main.spriteBatch.Draw(
                    texture,
                    drawPos,
                    null,
                    color,
                    Projectile.oldRot[k] + MathHelper.PiOver2,
                    origin,
                    scale,
                    effects,
                    0f
                );
            }

            return true;
        }
    }
}