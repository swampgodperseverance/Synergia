using System.Collections.Generic;
using Bismuth.Content.Items.Weapons.Ranged;
using Microsoft.Xna.Framework;
using Synergia.Content.Items.Weapons.Ranged;
using Synergia.Content.Projectiles.Friendly;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;


namespace Synergia.Content.Items.Weapons.Melee
{
    public class StoneAge : ModItem
    {
        public override void SetStaticDefaults()
        {
            ItemID.Sets.Yoyo[Item.type] = true;
            ItemID.Sets.GamepadExtraRange[Item.type] = 10;
            ItemID.Sets.GamepadSmartQuickReach[Item.type] = true;
            Item.ResearchUnlockCount = 1;
            ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<Anaconda>();
        }

        public override void SetDefaults()
        {
            Item.damage = 24;
            Item.DamageType = DamageClass.Melee;
            Item.useTime = 14;
            Item.useAnimation = 14;
            Item.useStyle = 5;
            Item.channel = true;
            Item.knockBack = 3.5f;
            Item.value = Item.sellPrice(0, 0, 80, 0);
            Item.rare = 1;
            Item.autoReuse = false;
            Item.shoot = ModContent.ProjectileType<StoneAgeProj>();
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.UseSound = SoundID.Item1;
        }

        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
        {
            damage.Base += 0.1f * (player.yoyoString ? 1 : 0);
        }


        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (hit.Crit)
            {
                for (int i = 0; i < 8; i++)
                {
                    Dust.NewDust(target.position, target.width, target.height,
                        DustID.Stone, hit.HitDirection * 2, -2f, Scale: 1.2f);
                }
                SoundEngine.PlaySound(SoundID.Item14, target.Center);
            }
        }


    }
}