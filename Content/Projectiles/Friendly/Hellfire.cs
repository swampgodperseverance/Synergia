using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using static Synergia.Helpers.NPCHelper;

namespace Synergia.Content.Projectiles.Friendly
{
    public class Hellfire : ModProjectile
    {
        private const float AccelerationSpeed = 1.75f;
        private const float MaxSpeed = 20f;
        public override string Texture => "Synergia/Assets/Textures/LightTrail_1";
        public override void SetDefaults()
        {
            Projectile.width = 6;
            Projectile.height = 6;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = false;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.aiStyle = -1;
            Projectile.timeLeft = 300;
            //Projectile.usesLocalNPCImmunity = true;
            //Projectile.localNPCHitCooldown = -1;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 1;
        }
        public override bool? CanHitNPC(NPC target)
        {
            if (300 - Projectile.timeLeft < 20) return false;
            return true;
        }
        public override void OnSpawn(IEntitySource source)
        {
            Projectile.rotation = Main.rand.NextFloat() * MathHelper.TwoPi;
        }
        public override void AI()
        {
            if (Projectile.timeLeft % 2 == 0)
                Dust.NewDust(Projectile.position, 6, 6, DustID.Torch, Main.rand.NextFloat(-1f, 1f), Main.rand.NextFloat(-1f, 1f), 120, default, 1.75f);

            if (Projectile.wet && !Projectile.lavaWet)
                Projectile.Kill();

            if (300 - Projectile.timeLeft < 20)
            {
                Projectile.velocity *= 0.85f;
                return;
            }

            int? findTarget = FindClosestNPC(Projectile.Center, 0, -1, false, true);
            if (findTarget == null) return;

            NPC target = Main.npc[findTarget.Value];
            Vector2 direction = target.Center - Projectile.Center;
            direction.Normalize();
            Vector2 acceleration = direction * AccelerationSpeed;
            Projectile.velocity += acceleration;
            if (Projectile.velocity.LengthSquared() > MaxSpeed * MaxSpeed)
                Projectile.velocity = direction * MaxSpeed;

        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Type].Value;

            Color mainColor = new Color(255, 238, 145, 240);
            Color glowColor = new Color(255, 199, 121, 200);

            float baseScale = 0.4f + Main.rand.NextFloat(-0.1f, 0.1f);

            Main.EntitySpriteDraw(
                texture,
                Projectile.Center - Main.screenPosition,
                null,
                mainColor,
                Projectile.rotation,
                texture.Size() / 2f,
                baseScale * 1.3f,
                SpriteEffects.None,
                0
            );
            Main.EntitySpriteDraw(
                texture,
                Projectile.Center - Main.screenPosition,
                null,
                mainColor,
                Projectile.rotation + 1.3f,
                texture.Size() / 2f,
                baseScale * 1.3f,
                SpriteEffects.None,
                0
            );

            Main.EntitySpriteDraw(
                texture,
                Projectile.Center - Main.screenPosition,
                null,
                glowColor,
                Projectile.rotation + 0.5f,
                texture.Size() / 2f,
                baseScale * 0.9f,
                SpriteEffects.None,
                0
            );

            Main.EntitySpriteDraw(
                texture,
                Projectile.Center - Main.screenPosition,
                null,
                glowColor * 0.6f,
                Projectile.rotation - 0.4f,
                texture.Size() / 2f,
                baseScale * 1.7f,
                SpriteEffects.None,
                0
            );
            return false;
        }
        public override void OnKill(int timeLeft)
        {
            Vector2 velocity = Projectile.oldVelocity;
            for (int i = 0; i < 10; i++)
                Dust.NewDust(Projectile.Center, 6, 6, DustID.Torch, velocity.X, velocity.Y, 120, default, 2f);
            if (Main.myPlayer == Projectile.owner)
                Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<HellFireExplosion>(), 50, 0f, Projectile.owner);
        }
    }
    public class HellFireExplosion : ModProjectile
    {
        public override string Texture => "Synergia/Assets/Textures/Glow";

        public override void SetDefaults()
        {
            Projectile.width = 64;
            Projectile.height = 64;
            Projectile.aiStyle = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.aiStyle = -1;
            Projectile.timeLeft = 10;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.penetrate = -1;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            float progress = 1f - Projectile.timeLeft / 10f;
            Vector2 origin = texture.Size() / 2f;
            Color color = Color.Lerp(new Color(255, 238, 145, 240), Color.Transparent, progress);
            Main.EntitySpriteDraw(
                texture,
                Projectile.Center - Main.screenPosition,
                null,
                color,
                0f,
                origin,
                0.6f * progress,
                SpriteEffects.None,
                0
            );
            return false;
        }
    }
}
