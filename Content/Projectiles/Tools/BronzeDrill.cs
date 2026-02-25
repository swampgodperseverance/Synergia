using Synergia.Common.Rarities;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using ValhallaMod.Items.Material;
using Synergia.Content.Projectiles.Tools;
using Synergia.Content.Items.Materials;

namespace Synergia.Content.Projectiles.Tools
{
    // Token: 0x0200015C RID: 348
    public class BronzeDrillProj : ModProjectile
    {
        // Token: 0x060006CE RID: 1742 RVA: 0x00024646 File Offset: 0x00022846
        public override void SetStaticDefaults()
        {
            Main.projFrames[base.Projectile.type] = 4;
        }

        // Token: 0x060006CF RID: 1743 RVA: 0x00043F6C File Offset: 0x0004216C
        public override void SetDefaults()
        {
            base.AIType = 252;
            base.Projectile.width = 22;
            base.Projectile.height = 22;
            base.Projectile.aiStyle = 20;
            base.Projectile.friendly = true;
            base.Projectile.penetrate = -1;
            base.Projectile.tileCollide = false;
            base.Projectile.hide = true;
            base.Projectile.ownerHitCheck = true;
            base.Projectile.DamageType = DamageClass.Melee;
            base.Projectile.scale = 1f;
            base.Projectile.ArmorPenetration = 5;
        }

        // Token: 0x060006D0 RID: 1744 RVA: 0x00044014 File Offset: 0x00042214
        public override void AI()
        {
            int dust = Dust.NewDust(base.Projectile.position, base.Projectile.width, base.Projectile.height, 31, base.Projectile.velocity.X * 0.1f, base.Projectile.velocity.Y * 0.1f, 100, default(Color), 1f);
            Main.dust[dust].noGravity = true;
        }
    }
}
