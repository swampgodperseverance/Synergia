using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Vanilla.Content.Projectiles.Aura;
using Vanilla.Common.ModSystems;
using static Terraria.ModLoader.ModContent;

namespace Vanilla.Content.Items.Weapons.AuraStaff
{
    [ExtendsFromMod("ValhallaMod")]
    public class UnderwaterAuraScythe : ValhallaMod.Items.AI.ValhallaAuraItem
    {
        private int mode = 0; // 0 = Attack, 1 = Support
        private int modeSwitchCooldown = 0;

        public override string Texture => "Vanilla/Content/Items/Weapons/AuraStaff/UnderwaterAuraScythe";

        public override void SetStaticDefaults()
        {
            Item.staff[Item.type] = true;
            ValhallaMod.Values.AuraScythe[Item.type] = true;
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            
            Item.width = 38;
            Item.height = 38;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.UseSound = SoundID.Item82;
            Item.noMelee = true;
            Item.useTurn = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Blue;
            Item.mana = 20;
            Item.damage = 4;
            Item.DamageType = DamageClass.Summon;
            Item.shoot = ProjectileType<UnderwaterAuraScytheAttack>(); // Атакующий режим по умолчанию
            Item.shootSpeed = 1f;

            typeScythe = ProjectileType<UnderwaterAuraScytheCut>();
            scytheDamageScale = 0.75f;
        }
    
        public override void HoldItem(Player player)
        {
            if (modeSwitchCooldown > 0)
                modeSwitchCooldown--;

            if (modeSwitchCooldown <= 0 && VanillaKeybinds.ToggleAuraModeKeybind.JustPressed)
            {
                mode = 1 - mode;
                modeSwitchCooldown = 30;
                
                string modeText = mode == 0 ? "Attack Mode" : "Support Mode";
                Color textColor = mode == 0 ? Color.OrangeRed : Color.LightBlue;
                CombatText.NewText(player.getRect(), textColor, modeText, dramatic: true);

                // Удаляем старые ауры перед созданием новых
                RemoveAllAuras(player);

                if (mode == 0) // Атакующий режим
                {
                    CreateAura(player, ProjectileType<UnderwaterAuraScytheAttack>());
                }
                else // Режим поддержки
                {
                    CreateAura(player, ProjectileType<UnderwaterAuraScytheSupport>());
                }
            }

            // Автоматическое воссоздание ауры, если она исчезла
            int expectedProjType = mode == 0 ? 
                ProjectileType<UnderwaterAuraScytheAttack>() : 
                ProjectileType<UnderwaterAuraScytheSupport>();

            if (player.ownedProjectileCounts[expectedProjType] < 1)
            {
                CreateAura(player, expectedProjType);
            }
        }

        private void CreateAura(Player player, int projType)
        {
            Projectile.NewProjectile(
                player.GetSource_ItemUse(Item),
                player.Center,
                Vector2.Zero,
                projType,
                (int)(Item.damage * scytheDamageScale),
                Item.knockBack,
                player.whoAmI
            );
        }

        private void RemoveAllAuras(Player player)
        {
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile proj = Main.projectile[i];
                if (proj.active && proj.owner == player.whoAmI && 
                   (proj.type == ProjectileType<UnderwaterAuraScytheAttack>() || 
                    proj.type == ProjectileType<UnderwaterAuraScytheSupport>()))
                {
                    proj.Kill();
                }
            }
        }

        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            string texturePath = $"{Texture}{(mode == 0 ? "_Attack" : "_Support")}";
            Texture2D texture = ModContent.Request<Texture2D>(texturePath).Value;
            spriteBatch.Draw(texture, position, frame, drawColor, 0f, origin, scale, SpriteEffects.None, 0f);
            return false;
        }

        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            string texturePath = $"{Texture}{(mode == 0 ? "_Attack" : "_Support")}";
            Texture2D texture = ModContent.Request<Texture2D>(texturePath).Value;
            Vector2 position = Item.position - Main.screenPosition + Item.Size / 2f;
            Vector2 origin = new(texture.Width / 2f, texture.Height / 2f);
            spriteBatch.Draw(texture, position, null, lightColor, rotation, origin, scale, SpriteEffects.None, 0f);
            return false;
        }
    }
}