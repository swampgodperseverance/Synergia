using System;
using Avalon.Dusts;
using Avalon.Items.Material.Bars;
using Avalon.Items.Weapons.Melee.PreHardmode.Snotsabre;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using ValhallaMod.Projectiles.AI;

namespace Synergia.Content.Items.Weapons.Melee {

    public class Snotknife : ModItem {
        private bool hasHit;

        public override void SetStaticDefaults()
            => Item.ResearchUnlockCount = 1;

        public override void SetDefaults() {
            int width = 30; int height = width;
            Item.Size = new Vector2(width, height);

            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = Item.useAnimation = 29;

            Item.noMelee = true;
            Item.autoReuse = true;
            Item.shootsEveryUse = true;

            Item.DamageType = DamageClass.Melee;
            Item.damage = 20;
            Item.knockBack = 5;

            Item.value = Item.buyPrice(gold: 1, silver: 50);
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Item1;

            Item.shoot = ModContent.ProjectileType<SnotknifeP>();
            Item.shootSpeed = 14f;
        }
        public override void UseAnimation(Player player)
        {
            hasHit = false;
        }
        public override void AddRecipes()
        {
            base.CreateRecipe(1).AddIngredient(ModContent.ItemType<BacciliteBar>(), 7).AddTile(TileID.Anvils).Register();
        }

    }
    public class SnotknifeP : ShortSwordAI
    {
        // Token: 0x0600047D RID: 1149 RVA: 0x00030318 File Offset: 0x0002E518
        public override string Texture
        {
            get
            {
                return "Synergia/Content/Items/Weapons/Melee/Snotknife";
            }
        }
        public override void SafeSetDefaults()
        {
            base.Projectile.Size = new Vector2(48f);
            base.Projectile.scale = 1f;
            base.Projectile.extraUpdates = 1;
            this.FadeInDuration = 9;
            this.FadeOutDuration = 4;
            this.TotalDuration = 20;
        }

        // Token: 0x0600047E RID: 1150 RVA: 0x00030370 File Offset: 0x0002E570
        protected override void SetVisualOffsets()
        {
            int num = base.Projectile.width / 2;
            int num2 = base.Projectile.height / 2;
            base.DrawOriginOffsetX = 0f;
            base.DrawOffsetX = -(24 - num);
            base.DrawOriginOffsetY = -(24 - num2);
        }
        bool hasHit = false;
        // Token: 0x0600047F RID: 1151 RVA: 0x000303BA File Offset: 0x0002E5BA
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (!hasHit)
            {
                for (int i = 0; i < 4; i++)
                {
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(),
                        target.Center,
                        Main.rand.NextVector2CircularEdge(6, 6),
                        ModContent.ProjectileType<SnotsabreShot>(),
                        Projectile.damage / 3,  
                        Projectile.knockBack / 2,  
                        Projectile.owner,
                        target.whoAmI);
                }
            }
            target.AddBuff(BuffID.Poisoned, 4 * 60);
            hasHit = true;
            for (int i = 0; i < 15; i++)
            {
                Dust d = Dust.NewDustDirect(target.Center, 0, 0, ModContent.DustType<ContagionWeapons>(), 0, 0, 128);
                d.velocity *= 3;
                d.scale = 1f;
                d.noGravity = true;
            }
        }
    }
}