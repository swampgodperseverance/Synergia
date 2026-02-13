using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class NoxYoyo : GlobalProjectile
{
    public override bool InstancePerEntity => true;

    private int ringIndex = -1;

    public override void AI(Projectile projectile)
    {
        if (projectile.ModProjectile != null &&
            projectile.ModProjectile.Mod.Name == "Avalon" &&
            projectile.ModProjectile.Name == "NoxiousProj")
        {
            if (ringIndex == -1 || !Main.projectile[ringIndex].active)
            {
                ringIndex = Projectile.NewProjectile(
                    projectile.GetSource_FromAI(),
                    projectile.Center,
                    Vector2.Zero,
                    ModContent.ProjectileType<NoxiousRing>(),
                    projectile.damage / 2,
                    0f,
                    projectile.owner,
                    projectile.whoAmI
                );
            }
        }
    }

    public class NoxiousRing : ModProjectile
    {
        public override string Texture => "Synergia/Assets/Textures/Ring";

        public override void SetDefaults()
        {
            Projectile.width = 100;
            Projectile.height = 100;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 2;
            Projectile.penetrate = -1;
            Projectile.scale = 0.9f;
        }

        public override bool? CanDamage() => true;

        public override void AI()
        {
            int parentIndex = (int)Projectile.ai[0];

            if (parentIndex < 0 || parentIndex >= Main.maxProjectiles)
            {
                Projectile.Kill();
                return;
            }

            Projectile parent = Main.projectile[parentIndex];

            if (!parent.active)
            {
                Projectile.Kill();
                return;
            }

            Projectile.timeLeft = 2;
            Projectile.Center = parent.Center;
            Projectile.rotation += 0.13f;

            float pulse = (float)Main.timeForVisualEffects * 0.05f;
            Projectile.scale = 0.6f + (float)System.Math.Sin(pulse) * 0.05f;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Poisoned, 180);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = ModContent.Request<Texture2D>(Texture).Value;
            Vector2 origin = texture.Size() / 2f;
            Vector2 screenPos = Projectile.Center - Main.screenPosition;

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

            Color drawColor = new Color(0, 180, 30) * 0.7f;

            Main.EntitySpriteDraw(texture, screenPos, null, drawColor, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

            return false;
        }
    }
}