using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Synergia.Content.Projectiles.Friendly;
using System;
using Microsoft.Xna.Framework;

namespace Synergia.Content.Items.Accessories
{
    public sealed class ThunderSigil : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 28;
            Item.accessory = true;
            Item.rare = ItemRarityID.Yellow;
            Item.value = Item.buyPrice(gold: 6);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<ThunderSigilPlayer>().hasThunderSigil = true;
        }
    }

    public class ThunderSigilPlayer : ModPlayer
    {
        public bool hasThunderSigil;

        public override void ResetEffects()
        {
            hasThunderSigil = false;
        }

        public override void PostUpdate()
        {
            if (hasThunderSigil)
                SpawnSigil();
            else
                KillSigil();
        }

        private void SpawnSigil()
        {
            bool exists = false;
            foreach (Projectile proj in Main.projectile)
            {
                if (proj.active && proj.owner == Player.whoAmI && proj.type == ModContent.ProjectileType<ThunderSigilProj>())
                {
                    exists = true;
                    break;
                }
            }

            if (!exists && Main.myPlayer == Player.whoAmI)
            {
                Projectile.NewProjectile(
                    Player.GetSource_Accessory(null),   
                    Main.MouseWorld, 
                    Vector2.Zero,
                    ModContent.ProjectileType<ThunderSigilProj>(),
                    120,
                    2f,
                    Player.whoAmI
                );
            }
        }

        private void KillSigil()
        {
            foreach (Projectile proj in Main.projectile)
            {
                if (proj.active && proj.owner == Player.whoAmI && proj.type == ModContent.ProjectileType<ThunderSigilProj>())
                {
                    proj.Kill();
                }
            }
        }
    }
}