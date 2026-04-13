using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace Synergia.Content.Dusts
{
    // Token: 0x020000F1 RID: 241
    public class RingDust : ModDust
    {
        // Token: 0x06000502 RID: 1282 RVA: 0x0001FC05 File Offset: 0x0001DE05
        public override void OnSpawn(Dust dust)
        {
            dust.noGravity = true;
            dust.frame = new Rectangle(0, 0, 64, 64);
        }

        // Token: 0x06000503 RID: 1283 RVA: 0x0001FC1F File Offset: 0x0001DE1F
        public override Color? GetAlpha(Dust dust, Color lightColor)
        {
            return new Color?(dust.color);
        }

        // Token: 0x06000504 RID: 1284 RVA: 0x0001FC2C File Offset: 0x0001DE2C
        public override bool Update(Dust dust)
        {
            if (dust.customData == null)
            {
                dust.position -= Vector2.One * 32f * dust.scale;
                dust.customData = true;
            }
            Vector2 currentCenter = dust.position + Vector2.One.RotatedBy((double)dust.rotation, default(Vector2)) * 32f * dust.scale;
            dust.scale *= 0.95f - dust.fadeIn;
            Vector2 nextCenter = dust.position + Vector2.One.RotatedBy((double)(dust.rotation + 0.06f), default(Vector2)) * 32f * dust.scale;
            dust.rotation += 0.06f;
            dust.position += currentCenter - nextCenter;
            dust.position += dust.velocity;
            if (!dust.noGravity)
            {
                dust.velocity.Y = dust.velocity.Y + 0.1f;
            }
            dust.velocity *= 0.99f;
            dust.color *= 0.95f;
            if (!dust.noLight)
            {
                Lighting.AddLight(dust.position, dust.color.ToVector3());
            }
            if (dust.scale < 0.05f)
            {
                dust.active = false;
            }
            return false;
        }
    }
}
