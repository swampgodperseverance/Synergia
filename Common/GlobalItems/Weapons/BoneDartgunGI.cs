using System;
using Microsoft.Xna.Framework;
using ReLogic.Content;
using Synergia.Content.Projectiles.Reworks.AltUse;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Content.GlobalItems.Weapons
{
    public class bdGI : GlobalItem
    {
        public override bool AppliesToEntity(Item item, bool lateInstantiation)
        {
            if (item.ModItem == null) return false;
            return string.Equals(item.ModItem.Mod?.Name, "ValhallaMod", StringComparison.OrdinalIgnoreCase)
                && string.Equals(item.ModItem.Name, "BoneDartgun", StringComparison.OrdinalIgnoreCase);
        }

        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int reducedDamage = (int)(damage * 0.6f);
            if (reducedDamage < 1) reducedDamage = 1;
            Projectile.NewProjectile(source, position, velocity, ProjectileID.Bone, reducedDamage, knockback * 0.8f, player.whoAmI);
            return true;
        }
    }
    public class BoneProjectileVisuals : GlobalProjectile
    {
        public override bool AppliesToEntity(Projectile projectile, bool lateInstantiation)
        {
            return projectile.type == ProjectileID.Bone;
        }

        public override void SetDefaults(Projectile projectile)
        {
            projectile.alpha = 40; 
            projectile.light = 0.5f; 
        }
        public override void PostDraw(Projectile projectile, Color lightColor)
        {
           
            Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[projectile.type].Value;
            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, projectile.height * 0.5f);

           
            Color outlineColor = new Color(120, 120, 120, 60);
            Main.spriteBatch.Draw(
                texture,
                projectile.Center - Main.screenPosition,
                new Rectangle(0, 0, texture.Width, texture.Height),
                outlineColor,
                projectile.rotation,
                drawOrigin,
                projectile.scale,
                SpriteEffects.None,
                0f
            );
        }
    }
}