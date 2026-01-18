using Avalon;
using Avalon.Achievements;
using Avalon.Common.Players;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.IO;
using Terraria.GameContent;
using Synergia.Trails;
using System.Collections.Generic;
using System.Linq;

namespace Synergia.Content.Projectiles.Reworks.Reworks2
{
    public class SunRework : ModProjectile
    {
        private Texture2D backTexture;
        private float backRotation = 0f;
        private float backSpinSpeed = 0.034f;
        private float frontSpinSpeed = 0.05f;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 8;
        }

        public override void OnSpawn(IEntitySource source)
        {
            backTexture = ModContent.Request<Texture2D>("Synergia/Content/Projectiles/Reworks/Reworks2/SunRework_Back").Value;
        }

        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.aiStyle = 1;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = 4;
            Projectile.timeLeft = 60;
            Projectile.scale = 0.8f;
            Projectile.alpha = 255;
        }

        public override void AI()
        {
            if (Projectile.alpha > 0) Projectile.alpha -= 20;
            Projectile.alpha = Math.Max(0, Projectile.alpha);

            Lighting.AddLight(Projectile.Center, 0.9f, 0.75f, 0.3f);

            Projectile.rotation += frontSpinSpeed;
            backRotation += backSpinSpeed;

            frontSpinSpeed *= 1.04f;
            backSpinSpeed *= 1.057f;

            Projectile.scale *= 0.984f;

            if (Main.rand.NextBool(3))
            {
                int d = Dust.NewDust(
                    Projectile.position,
                    Projectile.width,
                    Projectile.height,
                    DustID.GoldFlame,
                    Projectile.velocity.X * 0.2f,
                    Projectile.velocity.Y * 0.2f,
                    100,
                    new Color(255, 240, 100),
                    1.3f
                );
                Main.dust[d].noGravity = true;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (backTexture == null || backTexture.IsDisposed)
                return true;

            Texture2D mainTex = TextureAssets.Projectile[Type].Value;
            Rectangle mainFrame = mainTex.Frame();
            Rectangle backFrame = backTexture.Frame();

            Vector2 origin = mainFrame.Size() / 2f;
            Vector2 backOrigin = backFrame.Size() / 2f;

            Vector2 drawPos = Projectile.Center - Main.screenPosition;
            SpriteEffects effects = Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

            Main.EntitySpriteDraw(backTexture, drawPos, backFrame,
                new Color(100, 70, 20, 90),
                backRotation + MathHelper.PiOver4 * 0.4f,
                backOrigin, Projectile.scale * 1.7f, effects, 0);

            Main.EntitySpriteDraw(backTexture, drawPos, backFrame,
                new Color(255, 220, 100, 200),
                backRotation,
                backOrigin, Projectile.scale * 1.3f, effects, 0);

            Main.EntitySpriteDraw(mainTex, drawPos, mainFrame,
                Projectile.GetAlpha(lightColor),
                Projectile.rotation,
                origin, Projectile.scale * 0.95f, effects, 0);

            return false;
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item14, Projectile.position);

            for (int i = 0; i < 40; i++)
            {
                Vector2 speed = Main.rand.NextVector2Circular(4f, 4f);
                int dust = Dust.NewDust(
                    Projectile.position,
                    10,
                    10,
                    DustID.GoldFlame,
                    speed.X,
                    speed.Y,
                    100,
                    new Color(255, 220, 80),
                    1.7f
                );
                Main.dust[dust].noGravity = true;
            }

            int amount = Main.rand.Next(3, 6);
            for (int i = 0; i < amount; i++)
            {
                Vector2 vel = Main.rand.NextVector2Circular(7f, 7f);
                Projectile.NewProjectile(
                    Projectile.GetSource_FromThis(),
                    Projectile.Center,
                    vel,
                    ProjectileID.ApprenticeStaffT3Shot,
                    Projectile.damage,
                    Projectile.knockBack,
                    Projectile.owner
                );
            }


        }

        private NPC FindClosestNPC(float maxDetectDistance)
        {
            NPC closest = null;
            float sqDist = maxDetectDistance * maxDetectDistance;

            foreach (NPC npc in Main.npc.Where(n => n.CanBeChasedBy()))
            {
                float dist = Vector2.DistanceSquared(npc.Center, Projectile.Center);
                if (dist < sqDist)
                {
                    sqDist = dist;
                    closest = npc;
                }
            }
            return closest;
        }
    }
}
