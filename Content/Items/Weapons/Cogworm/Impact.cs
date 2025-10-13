using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Synergia.Content.Projectiles.Friendly;
using Synergia.Common;

namespace Synergia.Content.Items.Weapons.Cogworm
{
    public class Impact : ModItem
    {
        private int shotCounter = 0;

        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 20;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = SoundID.Item1;
            Item.DamageType = DamageClass.Throwing;
            Item.damage = 130; 
            Item.knockBack = 4f;
            Item.autoReuse = true;
            Item.shootSpeed = 12f;
            Item.useAnimation = 25;
            Item.useTime = 25;
            Item.rare = ModContent.RarityType<LavaGradientRarity>();

            Item.value = Item.sellPrice(0, 5, 0, 0);
            Item.shoot = ModContent.ProjectileType<CogwormProj1>();
        }

        public override bool CanUseItem(Player player)
        {
            if (player.HasBuff(ModContent.BuffType<Buffs.Hellborn>()))
                shotCounter = 4; 
            return base.CanUseItem(player);
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            switch (shotCounter % 5)
            {
                case 0: type = ModContent.ProjectileType<CogwormProj1>(); break;
                case 1: type = ModContent.ProjectileType<CogwormProj2>(); break;
                case 2: type = ModContent.ProjectileType<CogwormProj3>(); break;
                case 3: type = ModContent.ProjectileType<CogwormProj4>(); break;
                case 4: type = ModContent.ProjectileType<CogwormProj5>(); break;
            }
            shotCounter++;
        }
    }
}
