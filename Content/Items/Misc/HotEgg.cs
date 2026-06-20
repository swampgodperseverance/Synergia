using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Synergia.Common.Rarities;
using Synergia.Content.Buffs;
using Synergia.Content.Projectiles.Other;
using Synergia.UIs;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using ValhallaMod;
using ValhallaMod.Buffs.Pet;
using ValhallaMod.Projectiles.Pets;

namespace Synergia.Content.Items.Misc
{
    public class HotEgg : ModItem
    {
        public override void SetDefaults()
        {
            base.Item.DefaultToVanitypet(ModContent.ProjectileType<HotEggProj>(), ModContent.BuffType<HotEggBuff>());
            base.Item.width = 36;
            base.Item.height = 36;
            base.Item.value = Item.sellPrice(0, 1, 50, 0);
            base.Item.rare = ModContent.RarityType<LavaGradientRarity>();
        }

        // Token: 0x06001490 RID: 5264 RVA: 0x000A4A32 File Offset: 0x000A2C32
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            player.AddBuff(base.Item.buffType, 2, true, false);
            return false;
        }
    }
}