using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Synergia.Content.Buffs; 

namespace Synergia.Content.Projectiles.Reworks.AltUse
{
    public class GloomR : ModProjectile
    {   public static readonly SoundStyle DirtSound = new("Synergia/Assets/Sounds/DirtSound");
        public override void SetDefaults()
        {
            Projectile.width = 26;
            Projectile.height = 26;
            Projectile.aiStyle = -1;
            Projectile.timeLeft = 36000;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.netImportant = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.scale = 1f;
            Projectile.noEnchantmentVisuals = true;
        }

        public ref float Timer => ref Projectile.ai[0];
        public ref float Timer2 => ref Projectile.localAI[1];
        Player Player => Main.player[Projectile.owner];

        public override void AI()
        {
        
            Player.noItems = true;
            Player.controlUseItem = false;

            Timer2--;
            if (Timer2 <= 30f)
                Timer2 = 190f;

            Timer++;

         
            if (Projectile.ai[2] == 0f)
            {
                Projectile.position.Y -= 0.5f; 
            }

          
            if (Timer >= 120 && Projectile.ai[2] == 0f)
            {
                Projectile.ai[2] = 1f;
                Projectile.netUpdate = true;
            }

            if (Projectile.ai[2] == 1f)
            {
                BoomerangBehaviour();
                return;
            }

            NormalBehaviour();
        }

        private void BoomerangBehaviour()
        {
            if (Projectile.soundDelay == 0)
            {
                Projectile.soundDelay = 16;
                SoundEngine.PlaySound(SoundID.Item1, Projectile.position);
            }

            Projectile.tileCollide = false;
            Projectile.rotation += 0.4f * Projectile.direction;
            Projectile.velocity = Projectile.Center.DirectionTo(Player.Center) * 8f + Player.velocity;

            if (Projectile.Hitbox.Intersects(Player.Hitbox))
            {
                Projectile.Kill();
            }
        }

        private void NormalBehaviour()
        {
            Projectile.rotation = 0f;

            Projectile.velocity.Y = Helpers.EaseFunctions.EaseInBack(Timer / 50f) * 20f; 
            if (Projectile.velocity.Y > 10f)
                Projectile.velocity.Y = 10f;

            Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.SpectreStaff, 0, -4f, Scale: 2f);
            dust.noGravity = true;
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            fallThrough = Projectile.Bottom.Y < Player.Top.Y;
            return true;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.ai[2] == 1f)
                return false;

            if (Projectile.ai[1] == 0f && ((Projectile.Bottom.Y > Player.Top.Y && Projectile.tileCollide) ||
                                           (Projectile.tileCollide && Collision.SolidTiles(Projectile.Bottom, 4, 4))))
            {
        
                SoundEngine.PlaySound(DirtSound, Projectile.Center); 

          
                for (int i = 0; i < 30; i++)
                {
                    Dust dust2 = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height * 2, DustID.SpectreStaff, 0f, -4f, Scale: 2.5f);
                    dust2.noGravity = true;
                    dust2.velocity *= 1.2f;
                }

            
                Player.AddBuff(ModContent.BuffType<GloomBuff>(), 1200);

                Projectile.ai[1] = 1f;
            }

            return false;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float _ = 0f;
            Vector2 velocity = Projectile.velocity.SafeNormalize(Vector2.UnitY) * Projectile.scale;
            return Collision.CheckAABBvLineCollision(
                targetHitbox.TopLeft(),
                targetHitbox.Size(),
                Projectile.Center - velocity * 70f,
                Projectile.Center + velocity * 10f,
                24f * Projectile.scale,
                ref _
            );
        }

        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs,
            List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            behindNPCsAndTiles.Add(index);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Type].Value;
            Vector2 origin = new(texture.Width / 2f, texture.Height / 2f);
            Color drawColor = Projectile.GetAlpha(lightColor);
            SpriteEffects spriteEffects = Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

    
            int trailCount = 3;
            for (int i = 0; i < trailCount; i++)
            {
                int index = i * (Projectile.oldPos.Length / trailCount);
                if (index >= Projectile.oldPos.Length) index = Projectile.oldPos.Length - 1;

                Vector2 pos = Projectile.oldPos[index] + Projectile.Size / 2 - Main.screenPosition;
                float scale = Projectile.scale * (0.7f + 0.1f * (trailCount - i));
                float alpha = 0.5f * ((float)(trailCount - i) / trailCount);

                Main.spriteBatch.Draw(texture,
                    pos,
                    null,
                    drawColor * alpha,
                    Projectile.rotation,
                    origin,
                    scale,
                    spriteEffects,
                    0f);
            }

       
            Main.spriteBatch.Draw(texture,
                Projectile.Center - Main.screenPosition,
                null,
                drawColor,
                Projectile.rotation,
                origin,
                Projectile.scale,
                spriteEffects,
                0f);

            return false;
        }

        public override void OnKill(int timeLeft)
        {
          
            Player.noItems = false;
            Player.controlUseItem = true;
        }
    }
}
     