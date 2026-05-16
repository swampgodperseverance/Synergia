using System;
using Avalon.Dusts;
using Avalon.Items.Material.Bars;
using Avalon.Items.Weapons.Melee.PreHardmode.Snotsabre;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using ValhallaMod.Projectiles.AI;

namespace Synergia.Content.Items.Weapons.Melee {

    public class Snotknife : ModItem {

        public override void SetStaticDefaults()
            => Item.ResearchUnlockCount = 1;

        public override void SetDefaults()
        {
            base.Item.damage = 21;
            base.Item.knockBack = 5f;
            base.Item.useAnimation = 18;
            base.Item.useTime = 9;
            base.Item.useStyle = 13;
            base.Item.width = 32;
            base.Item.height = 32;
            base.Item.UseSound = new SoundStyle?(SoundID.Item1);
            base.Item.DamageType = DamageClass.MeleeNoSpeed;
            base.Item.autoReuse = true;
            base.Item.noUseGraphic = true;
            base.Item.noMelee = true;
            base.Item.rare = 1;
            base.Item.value = Item.sellPrice(0, 0, 25, 0);
            base.Item.shoot = ModContent.ProjectileType<SnotknifeP>();
            base.Item.shootSpeed = 3f;
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