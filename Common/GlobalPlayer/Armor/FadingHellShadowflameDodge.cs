using Avalon.Particles;
using Synergia.Common.ModSystems.Netcode;
using Synergia.Common.ModSystems.Netcode.Packets;
using Synergia.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.Graphics;
using Terraria.ID;

namespace Synergia.Common.GlobalPlayer.Armor
{
    public class FadingHellShadowflameDodge : ModPlayer
    {
        public bool IsActive = false;
        public int PartsCount = 0;
        internal int PrevPartsCount = 0;
        public override void Initialize()
        {
            IsActive = false;
            PartsCount = 0;
            PrevPartsCount = 0;
        }
        public override void ResetEffects()
        {
            IsActive = false;
        }
        public override void UpdateDead()
        {
            PartsCount = 0;
            PrevPartsCount = 0;
        }
        public override void UpdateEquips()
        {
            if (!IsActive) return;

            if (PrevPartsCount == PartsCount) return;
            PrevPartsCount = PartsCount;
            if (PartsCount != 3) return;

            PartsCount = 0;
            PrevPartsCount = 0;
            Vector2 velocity;
            for(int i = 0; i < 30; i++)
            {
                velocity = Vector2.UnitX.RotatedBy(MathHelper.ToRadians(i * 12)) * 8f;
                ParticleSystem.AddParticle(new ShadowflameParticle(), Player.Center, velocity, Color.Black, 1f);
            }
        }
        public override bool FreeDodge(Player.HurtInfo info)
        {
            if (!IsActive) return false;
            //if (!Main.rand.NextBool(4)) return false;

            DodgeEffect();
            Vector2 velocity;
            for(int i = 0; i < 3; i++)
            {
                velocity.Y = (float)Main.rand.Next(-40, -10) * 0.01f;
                velocity.X = (float)Main.rand.Next(-20, 21) * 0.01f + (0.2f * info.HitDirection);
                Projectile.NewProjectile(
                    Player.GetSource_FromAI(),
                    Player.Center,
                    velocity,
                    ModContent.ProjectileType<ShadowPlayerGore>(),
                    0,
                    0,
                    Main.myPlayer,
                    i
                );
            }
            return true;
        }
        internal void DodgeEffect()
        {
            Player.SetImmuneTimeForAllTypes(60);

            if (Main.myPlayer == Player.whoAmI && Main.netMode != NetmodeID.SinglePlayer)
                MultiplayerSystem.SendPacket(new ShadowflameDodgePacket(Player), ignoreClient: Main.myPlayer);
        }
    }
    public class ShadowPlayerGore : ModProjectile
    {
        internal const int ExtraUpdates = 10;
        public ref float ArmorType => ref Projectile.ai[0];
        public ref float X => ref Projectile.ai[1];
        public ref float Y => ref Projectile.ai[2];
        public override string Texture => "Synergia/Assets/Textures/Blank";
        public override void SetDefaults()
        {
            Projectile.height = 8;
            Projectile.width = 8;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.aiStyle = -1;
            Projectile.extraUpdates = ExtraUpdates;
            Projectile.timeLeft = 95 * ExtraUpdates;
            Projectile.alpha = 0;
        }
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (player == null || player.dead)
                Projectile.Kill();
            Projectile.rotation += Projectile.velocity.X * 0.01f;
            if (Projectile.timeLeft > 35 * ExtraUpdates)
            {
                Projectile.velocity.X *= 0.9995f;
                Projectile.velocity.Y += 0.001f;
                return;
            }
            else if(Projectile.timeLeft > 20 * ExtraUpdates)
            {
                Projectile.velocity *= 0.95f;
                Projectile.velocity *= 0.95f;
                Projectile.Opacity = (Projectile.timeLeft / ExtraUpdates - 20) / 15f;
                X = Projectile.Center.X;
                Y = Projectile.Center.Y;
                return;
            }

            if(Projectile.timeLeft % 3 == 0)
                ParticleSystem.AddParticle(new ShadowflameParticle(), Projectile.Center, Main.rand.NextVector2Unit() * 2f, Color.Black, Main.rand.NextFloat(0.8f, 1.2f));
            float progress = 1f - Projectile.timeLeft / 20f / ExtraUpdates;
            Projectile.Center = Vector2.Lerp(new Vector2(X, Y), player.Center, EaseFunctions.EaseInCubic(progress));
        }
        public override void OnKill(int timeLeft)
        {
            Player player = Main.player[Projectile.owner];
            player.GetModPlayer<FadingHellShadowflameDodge>().PartsCount++;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = ArmorType switch
            {
                //1 => ModContent.Request<Texture2D>("Synergia/Content/Items/Armor/Magic/FadingHell/FadingHellChestplate_Body").Value,
                1 => ModContent.Request<Texture2D>("Synergia/Content/Items/Armor/Magic/FadingHell/FadingHellPants_Legs").Value,
                2 => ModContent.Request<Texture2D>("Synergia/Content/Items/Armor/Magic/FadingHell/FadingHellPants_Legs").Value,
                _ => ModContent.Request<Texture2D>("Synergia/Content/Items/Armor/Magic/FadingHell/FadingHellHat_Head").Value,
            };
            Vector2 position = Projectile.Center - Main.screenPosition;
            int frameHeight = texture.Height / 20;
            Rectangle frame = new Rectangle(0, 0, texture.Width, frameHeight);
            Vector2 origin = frame.Size() / 2f;
            Color color = Color.Lerp(lightColor, Color.Black, 1f - Projectile.Opacity);
            Main.EntitySpriteDraw(
                texture,
                position,
                frame,
                color,
                Projectile.rotation,
                origin,
                1f,
                SpriteEffects.None,
                0
            );
            return false;
        }
    }
    public class ShadowflameParticle : Particle
    {
        internal const int MaxLifetime = 20;
        internal const float Size = 0.1f;
        public override void Update()
        {
            TimeInWorld++;
            if (TimeInWorld > MaxLifetime)
                Active = false;

            Position += Velocity;
            Velocity *= 0.99f;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            Texture2D texture2D = (Texture2D)ModContent.Request<Texture2D>("Synergia/Assets/Textures/Glow");
            Rectangle rectangle = texture2D.Frame();
            Vector2 origin = rectangle.Size() / 2f;
            Vector2 position = Position - Main.screenPosition;
            spriteBatch.Draw(
                texture2D,
                position,
                rectangle,
                Color.Black,
                0f,
                origin,
                (1f - EaseFunctions.EaseOutCubic((float)TimeInWorld / MaxLifetime)) * Size * ai1,
                SpriteEffects.None,
                0f);
        }
    }
}
