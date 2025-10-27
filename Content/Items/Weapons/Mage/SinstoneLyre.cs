using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Synergia.Content.Projectiles.Friendly;

namespace Synergia.Content.Items.Weapons.Mage
{
    public class SinstoneLyre : ModItem
    {
        private int noteCooldown = 0;

        public override void SetStaticDefaults()
        {
        }

        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 120;
            Item.useAnimation = 120;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.value = Item.buyPrice(0, 5, 0, 0);
            Item.rare = ItemRarityID.Red;
            Item.mana = 10;
            Item.DamageType = DamageClass.Magic;
            Item.damage = 60;
            Item.knockBack = 2f;
            Item.noMelee = true;
            Item.UseSound = SoundID.Item20;
            Item.autoReuse = true;
        }

        public override void HoldItem(Player player)
        {
            if (noteCooldown > 0)
                noteCooldown--;

            if (noteCooldown <= 0 && Main.myPlayer == player.whoAmI)
            {
                if (Main.mouseLeft) 
                {
                    Vector2 spawnPos = Main.MouseWorld;

                    Projectile.NewProjectile(
                        player.GetSource_ItemUse(Item),
                        spawnPos,
                        Vector2.Zero,
                        ModContent.ProjectileType<HeatNote>(),
                        Item.damage,
                        Item.knockBack,
                        player.whoAmI
                    );

                    // Тише звук
                    SoundEngine.PlaySound(new SoundStyle("Synergia/Assets/Sounds/SinstoneLyre")
                    {
                        Volume = 0.3f,
                        Pitch = 0f
                    }, spawnPos);

                    noteCooldown = 120; 
                }
            }
        }
    }
}
