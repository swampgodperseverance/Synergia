using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader.IO; 
using Terraria.ModLoader;
using Synergia.Content.Projectiles.Reworks.AltUse;
using Synergia.Common.Players;

namespace Synergia.Content.GlobalItems.Weapons
{
    public class TentacleSpikeGI : GlobalItem
    {
        public override bool InstancePerEntity => true;
        public int rightClickCooldown = 0;
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {

            return entity.type == ItemID.TentacleSpike;
        }
        public override void SetDefaults(Item item)
        {

            item.autoReuse = true;
            item.useStyle = ItemUseStyleID.Shoot;
            item.shoot = ModContent.ProjectileType<TentacleSpikeRework>();
            item.channel = true;
            item.noUseGraphic = true;
            item.noMelee = true;
            item.useTurn = false;
            item.UseSound = null;
        }

        public override bool CanUseItem(Item item, Player player)
        {
            return player.ownedProjectileCounts[ModContent.ProjectileType<TentacleSpikeRework>()] < 1;
        }

        private bool flip = true;
        public override bool Shoot(Item item, Player player, Terraria.DataStructures.EntitySource_ItemUse_WithAmmo source, Microsoft.Xna.Framework.Vector2 position, Microsoft.Xna.Framework.Vector2 velocity, int type, int damage, float knockback)
        {
            int idx = Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<TentacleSpikeRework>(), damage, knockback, player.whoAmI);
            var proj = Main.projectile[idx];
            proj.ai[2] = flip ? -1 : 1;
            flip = !flip;
            proj.rotation = proj.DirectionTo(Main.MouseWorld).ToRotation();
            return false;
        }

        public override void HoldItem(Item item, Player player)
        {
            var mp = player.GetModPlayer<TentacleSpikePlayer>();
            if (mp != null && mp.TentacleSpikeEmpowered)
                item.damage = 80;
            else
                item.damage = 35;

            if (rightClickCooldown > 0)
                rightClickCooldown--;
        }

        public override bool PreDrawInWorld(Item item, SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            if (Main.rand.NextFloat() < 0.12f)
            {
                Dust.NewDust(item.position, item.width, item.height, DustID.IceTorch, 0f, -10f, 0, new Color(180, 220, 255), 1.2f);
            }
            return true;
        }


        public override void PostDrawInInventory(Item item, SpriteBatch spriteBatch, Microsoft.Xna.Framework.Vector2 position, Microsoft.Xna.Framework.Rectangle frame, Microsoft.Xna.Framework.Color drawColor, Microsoft.Xna.Framework.Color itemColor, Microsoft.Xna.Framework.Vector2 origin, float scale)
        {
            Texture2D texture = TextureAssets.Item[item.type].Value;
            if (Main.itemAnimations[item.type] != null)
                frame = Main.itemAnimations[item.type].GetFrame(texture, Main.itemFrameCounter[0]);
            else
                frame = texture.Frame();

            float time = Main.GlobalTimeWrappedHourly;
            float timer = item.timeSinceItemSpawned / 240f + time * 0.04f;
            time %= 4f;
            time /= 2f;
            if (time >= 1f) time = 2f - time;
            time = time * 0.5f + 0.5f;
        }

    }
    public class TentacleSpikePlayer : ModPlayer
    {

        public int TentacleSpikeHitCount = 0;
        public bool TentacleSpikeEmpowered = false;

        public override void ResetEffects()
        {

        }


        public override void SaveData(TagCompound tag)
        {
            tag["TentacleSpikeHitCount"] = TentacleSpikeHitCount;
            tag["TentacleSpikeEmpowered"] = TentacleSpikeEmpowered;
        }

        public override void LoadData(TagCompound tag)
        {
            if (tag.ContainsKey("TentacleSpikeHitCount")) TentacleSpikeHitCount = tag.GetInt("TentacleSpikeHitCount");
            if (tag.ContainsKey("TentacleSpikeEmpowered")) TentacleSpikeEmpowered = tag.GetBool("TentacleSpikeEmpowered");
        }
    }
}
