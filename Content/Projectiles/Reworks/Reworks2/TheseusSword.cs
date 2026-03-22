using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using NewHorizons.Content.Projectiles.Melee;

namespace Synergia.Content.Projectiles.Reworks.Reworks2
{
    // Token: 0x0200004A RID: 74
    public class TheseusSword : SlashKatana
    {
        // Token: 0x0600019D RID: 413 RVA: 0x000091C4 File Offset: 0x000073C4
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[base.Projectile.type] = 2;
            ProjectileID.Sets.TrailCacheLength[base.Projectile.type] = 6;
        }

        // Token: 0x0600019E RID: 414 RVA: 0x0000E23F File Offset: 0x0000C43F
        public override void SetDefaults()
        {
            base.SetDefaults();
            base.Projectile.width = 52;
            base.Projectile.height = 58;
            base.Projectile.scale = 1f;
            this.AttackAI[0] = 32f;
        }

        // Token: 0x0600019F RID: 415 RVA: 0x0000E280 File Offset: 0x0000C480
        public override bool PreDraw(ref Color lightColor)
        {
            if (this.AttackAI[1] == 1f)
            {
                Color[] LColor = new Color[]
                {
                    Color.Lerp(new Color(145, 152, 175), new Color(77, 79, 100), 0.5f)
                };
                base.AttackEffects(LColor);
                base.AttackEffects(lightColor, null);
            }
            return false;
        }

        // Token: 0x060001A0 RID: 416 RVA: 0x0000E2E8 File Offset: 0x0000C4E8
        public override void AI()
        {
            float num = this.AttackAI[1];
            Player player = Main.player[base.Projectile.owner];
            if ((float)player.itemAnimation > (float)player.itemAnimationMax * 0.75f)
            {
                base.Projectile.ai[0] += 20f / (float)player.itemAnimationMax * 2f;
            }
            base.Attack();
        }

        // Token: 0x060001A1 RID: 417 RVA: 0x0000E35A File Offset: 0x0000C55A
        public override void AttackPro()
        {
            base.AttackPro();
        }

    }
}
