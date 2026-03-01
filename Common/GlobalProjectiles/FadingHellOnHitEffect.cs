using Synergia.Common.GlobalPlayer.Armor;
using Synergia.Content.Projectiles.Friendly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using static Synergia.Common.GlobalItems.FadingHellData;

namespace Synergia.Common.GlobalProjectiles
{
    public class FadingHellOnHitEffect : GlobalProjectile
    {
        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (projectile.type == ModContent.ProjectileType<Hellfire>()) return;
            if (!(projectile.DamageType == DamageClass.Magic || projectile.DamageType == DamageClass.MagicSummonHybrid)) return;

            Player player = Main.player[projectile.owner];
            FadingHellPlayer fadingHellPlayer = player.GetModPlayer<FadingHellPlayer>();
            if (!fadingHellPlayer.isOnFire) return;

            FadingHellFireData? fireData = GetFireData(fadingHellPlayer.currentFireType);
            if (fireData == null) return;

            target.AddBuff(fireData.Value.VanillaDebuffID, 180, false);
            switch (fadingHellPlayer.currentFireType)
            {
                case FireType.OnFire:
                    if (Main.myPlayer != projectile.owner) break;
                    if (Main.rand.NextBool(3)) break;
                    Projectile.NewProjectile(
                        projectile.GetSource_FromAI(),
                        target.Center,
                        Vector2.UnitX.RotateRandom(MathHelper.TwoPi) * 20f,
                        ModContent.ProjectileType<Hellfire>(),
                        70,
                        0.25f,
                        projectile.owner
                    );
                    break;
            }
        }
    }
}
