using Microsoft.Xna.Framework;
using RoA.Common.Crossmod;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Content.Items.Weapons.Druid;

public class DriedBeetle : ModItem
{
    public override void SetDefaults()
    {
        Item.width = 26;  
        Item.height = 26;
        
        Item.damage = 24;
        Item.knockBack = 3f;
        Item.rare = ItemRarityID.Green;
        Item.value = Item.sellPrice(0, 0, 54, 0);
        
        Item.useStyle = ItemUseStyleID.Swing;
        Item.useTime = 16;
        Item.useAnimation = 16;
        Item.autoReuse = true;

        var result = DruidModCalls.Call(
            "MakeItemDruidicWeapon",
            Item
        );

        if (result?.ToString() == "Success")
        {
            DruidModCalls.Call(
                "SetDruidicWeaponValues",
                Item,
                (ushort)27, 
                0.8f 
            );
        }
    }

    public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
    {
        if (player.whoAmI == Main.myPlayer)
        {
            int offset = 30 * player.direction;
            var position = new Vector2(player.Center.X + offset, player.Center.Y - 14f);
            Vector2 pointPosition = Main.MouseWorld;
            Vector2 velocity = Vector2.Normalize(pointPosition - player.Center) * 12f;

            int projectileType;
            if (ModLoader.TryGetMod("RoA", out Mod riseOfAges))
            {
                projectileType = riseOfAges.Find<ModProjectile>("HemorrhageWave").Type;
            }
            else
            {
                // Fallback to a default projectile if RoA isn't loaded
                projectileType = ProjectileID.WoodenArrowFriendly;
            }

            Projectile.NewProjectile(
                player.GetSource_ItemUse(Item),
                position,
                velocity,
                projectileType,
                (int)(hit.Damage * 1.2f),
                hit.Knockback,
                player.whoAmI
            );

            SoundEngine.PlaySound(
                new SoundStyle($"{nameof(Synergia)}/Sounds/Items/ClawsWave") { Volume = 0.75f },
                position
            );
        }
    }
}