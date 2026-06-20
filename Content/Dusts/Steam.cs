using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace Synergia.Content.Dusts
{
    public class Steam : ModDust
    {
        public override void OnSpawn(Dust dust)
        {
            int frame = Main.rand.Next(0, 3);
            dust.frame = new Rectangle(0, frame * 24, 24, 24);

            dust.noGravity = true;
            dust.noLight = false;
            dust.scale *= Main.rand.NextFloat(0.95f, 1.35f);
            dust.rotation = Main.rand.NextFloat(MathHelper.TwoPi);
            dust.color = new Color(95, 95, 100, 0);
        }

        public override Color? GetAlpha(Dust dust, Color lightColor)
        {
            return dust.color;
        }

        public override bool Update(Dust dust)
        {
            float lifeProgress = 1f - (float)dust.alpha / 255f;     
            float opacity = Utils.GetLerpValue(0f, 0.25f, lifeProgress, true); 

            Color baseColor = new Color(72, 74, 78);

            Color litColor = Lighting.GetColor((int)(dust.position.X / 16f), (int)(dust.position.Y / 16f));

            dust.color = litColor.MultiplyRGB(baseColor) * opacity * 0.92f;
            dust.color.A = (byte)(110 * opacity);
            dust.position += dust.velocity * 0.85f;       
            dust.velocity *= 0.96f;
            dust.velocity.Y -= 0.085f;
            dust.velocity.X *= 0.93f;

            if (Main.rand.NextBool(5))
                dust.velocity.X += Main.rand.NextFloat(-0.12f, 0.12f);

            dust.scale *= 0.978f;

            dust.alpha += 2; 

            if (dust.alpha >= 245 || dust.scale < 0.35f)
                dust.active = false;

            return false;
        }
    }
}