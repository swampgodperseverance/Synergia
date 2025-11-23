using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Synergia.Content.Projectiles.Reworks.AltUse;

namespace Synergia.Content.Buffs
{
    public class GloomBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = false;
            Main.debuff[Type] = false;
            BuffID.Sets.IsATagBuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {

            bool hasProjectile = false;
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile proj = Main.projectile[i];
                if (proj.active && proj.owner == player.whoAmI && proj.type == ModContent.ProjectileType<GloomRing>())
                {
                    hasProjectile = true;
                    break;
                }
            }

        
            if (!hasProjectile)
            {
                Projectile.NewProjectile(
                    player.GetSource_Buff(buffIndex),
                    player.Center,
                    Vector2.Zero,
                    ModContent.ProjectileType<GloomRing>(),
                    0, 
                    0f, 
                    player.whoAmI
                );
            }
        }
    }
}
