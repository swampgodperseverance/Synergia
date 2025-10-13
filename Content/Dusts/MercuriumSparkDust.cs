using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace Synergia.Content.Dusts
{
    public class MercuriumSparkDust : ModDust
    {
        public override void OnSpawn(Dust dust)
        {
            dust.noGravity = true;
            dust.noLight = false;
            dust.scale = Main.rand.NextFloat(1.1f, 1.6f);
            dust.velocity *= 0.5f;
        }

        public override bool Update(Dust dust)
        {

            Lighting.AddLight(dust.position, 0.2f, 0.3f, 0.7f);

            dust.rotation += 0.1f; 
            dust.scale *= 0.97f;   
            dust.velocity *= 0.96f; 

            if (dust.scale < 0.5f)
                dust.active = false;  // а

            return false;
        }
    }
}