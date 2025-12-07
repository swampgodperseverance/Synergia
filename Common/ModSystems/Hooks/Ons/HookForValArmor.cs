using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Synergia.Content.Projectiles.Armor;
using Synergia.Common.ModSystems;

namespace Synergia.Common.ModSystems.Hooks.Ons
{
    public class ArmorSetKeybindHook : ModSystem
    {
        private int cooldownTimer = 0; 

        public override void PostUpdatePlayers()
        {
            cooldownTimer++;
            if (cooldownTimer > 1800) 
                cooldownTimer = 1800; 

            foreach (Player player in Main.player)
            {
                if (!player.active || player.dead)
                    continue;

                bool hasHelmet = player.armor[0].type == ModContent.ItemType<ValhallaMod.Items.Armor.ValhalliteHead>();
                bool hasChest = player.armor[1].type == ModContent.ItemType<ValhallaMod.Items.Armor.ValhalliteBody>();
                bool hasLegs = player.armor[2].type == ModContent.ItemType<ValhallaMod.Items.Armor.ValhalliteLegs>();

                if (hasHelmet && hasChest && hasLegs)
                {
                    if (VanillaKeybinds.ArmorSetBonusActivation.JustPressed && cooldownTimer >= 1800)
                    {
                        if (Main.myPlayer == player.whoAmI)
                        {
                            Projectile.NewProjectile(
                                player.GetSource_Misc("ArmorSetBonus"),
                                player.Center,
                                Vector2.Zero,
                                ModContent.ProjectileType<ValhalliteKnight>(),
                                20,
                                0f,
                                player.whoAmI
                            );

                            cooldownTimer = 0; 
                        }
                    }
                }
            }
        }
    }
}
