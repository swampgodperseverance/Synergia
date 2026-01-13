using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace Synergia.Content.Dusts
{
    public class ElectricityDust : ModDust
    {
        public override Color? GetAlpha(Dust dust, Color lightColor) => Color.White;
        public override void OnSpawn(Dust dust)
        {
            dust.noGravity = true;
            dust.noLight = false;
            dust.scale = Main.rand.NextFloat(1.1f, 1.6f);
            dust.velocity *= 0f;
        }

        public override bool Update(Dust dust)
        {
            Lighting.AddLight(dust.position, 0.25f, 0.2412f, 0.1441f);

            dust.scale *= 0.97f;
            dust.velocity *= 0.96f;

            if (dust.scale < 0.5f)
                dust.active = false;

            return false;
        }
    }
}
