using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Synergia.Common.Rarities;
using Synergia.Content.Projectiles.Other;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using ValhallaMod.Projectiles.AI;

namespace Synergia.Content.Items.Weapons.Melee
{
    public class Rhabdomyolysis : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 54;
            Item.height = 58;
            Item.useStyle = ItemUseStyleID.Rapier;
            Item.useTime = 5;
            Item.useAnimation = 18;
            Item.UseSound = SoundID.Item1;
            Item.damage = 36;
            Item.DamageType = DamageClass.MeleeNoSpeed;
            Item.ArmorPenetration = 10;
            Item.rare = ModContent.RarityType<CoreburnedRarity>();
            Item.value = Item.sellPrice(0, 2, 0, 0);
            Item.shoot = ModContent.ProjectileType<RhabdomyolysisProjectile>();
            Item.shootSpeed = 5f;
            Item.knockBack = 5f;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            ItemID.Sets.ItemNoGravity[Item.type] = true;
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            velocity = velocity.RotatedByRandom(0.2f);
            float healthPercent = (float)player.statLife / player.statLifeMax2;
            float damageMultiplier = 1f + (1f - healthPercent) * 2f;
            damage = (int)(damage * damageMultiplier);
        }

        public override void HoldItem(Player player)
        {
            if (player.HeldItem.type == Item.type && !player.dead)
            {
                bool hasManager = false;
                for (int i = 0; i < Main.maxProjectiles; i++)
                {
                    Projectile p = Main.projectile[i];
                    if (p.active && p.type == ModContent.ProjectileType<MeatknifeManager>() && p.owner == player.whoAmI)
                    {
                        hasManager = true;
                        break;
                    }
                }
                if (!hasManager)
                {
                    Projectile.NewProjectile(player.GetSource_ItemUse(Item), player.Center, Vector2.Zero,
                        ModContent.ProjectileType<MeatknifeManager>(), 0, 0f, player.whoAmI);
                }
            }
        }

        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            Texture2D texture = ModContent.Request<Texture2D>(Texture).Value;
            Player player = Main.LocalPlayer;
            float healthPercent = (float)player.statLife / player.statLifeMax2;
            float strength = (1f - healthPercent) * 0.6f;
            float final = MathHelper.Clamp(0.03f + strength * 0.22f, 0.03f, 0.25f);
            Color blood = new Color(200, 30, 30, (int)(50 * final));

            for (int i = 0; i < 8; i++)
            {
                float angle = MathHelper.TwoPi / 8f * i;
                Vector2 offset = new Vector2(1.5f + final * 2f, 0).RotatedBy(angle);
                spriteBatch.Draw(texture, position + offset * scale, frame, blood, 0f, origin, scale, SpriteEffects.None, 0f);
            }
            spriteBatch.Draw(texture, position, frame, drawColor, 0f, origin, scale, SpriteEffects.None, 0f);
            return false;
        }

        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            return true;
        }
    }

    public class RhabdomyolysisProjectile : ShortSwordAI
    {
        public override string Texture => ModContent.GetInstance<Rhabdomyolysis>().Texture;

        public override void SafeSetDefaults()
        {
            base.Projectile.Size = new Vector2(54f);
            base.Projectile.scale = 1f;
            base.Projectile.extraUpdates = 0;
            this.FadeInDuration = 7;
            this.FadeOutDuration = 4;
            this.TotalDuration = 12;
            this.ShortSwordWidth = 54;
            this.ShortSwordHeight = 58;
        }

        protected override void SetVisualOffsets()
        {
            int num = this.ShortSwordWidth / 2;
            int num2 = this.ShortSwordHeight / 2;
            int num3 = base.Projectile.width / 2;
            int num4 = base.Projectile.height / 2;
            base.DrawOriginOffsetX = 0f;
            base.DrawOffsetX = -(num - num3);
            base.DrawOriginOffsetY = -(num2 - num4);
        }

        public override void AI()
        {
            base.AI();

            if (Main.rand.NextBool(3))
            {
                Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Blood,
                    Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f, 60, default, 1.1f);
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Player player = Main.player[Projectile.owner];
            float healPercent = 0.10f;
            int healAmount = (int)(damageDone * healPercent);

            if (healAmount < 1) healAmount = 1;
            if (healAmount > 25) healAmount = 25;
            player.statLife += healAmount;
            if (player.statLife > player.statLifeMax2)
                player.statLife = player.statLifeMax2;
            if (Main.myPlayer == player.whoAmI)
            {
                player.HealEffect(healAmount, true);
            }

            for (int i = 0; i < 15; i++)
            {
                Vector2 offset = Main.rand.NextVector2Circular(30f, 30f);
                Dust.NewDustDirect(target.Center + offset, 8, 8, DustID.Blood,
                    offset.X * 0.3f, offset.Y * 0.3f - 1f, 80, default, 1.2f);
            }

            for (int i = 0; i < 10; i++)
            {
                Dust.NewDustDirect(target.position, target.width, target.height, DustID.Blood,
                    Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-2f, 2f), 60, default, 1.3f);
            }
            if (Main.rand.NextBool(3)) target.AddBuff(BuffID.Bleeding, 480);
            if (Main.rand.NextBool(5)) target.AddBuff(BuffID.Weak, 240);
        }

        public override void PostDraw(Color lightColor)
        {
            Texture2D texture = ModContent.Request<Texture2D>(Texture).Value;
            Vector2 drawPos = Projectile.Center - Main.screenPosition;
            Rectangle frame = texture.Frame();
            Vector2 origin = frame.Size() / 2f;
            SpriteEffects se = Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

            Color glow = new Color(180, 20, 20, 45);

            for (int i = 0; i < 6; i++)
            {
                Vector2 offset = new Vector2(2.5f, 0).RotatedBy(MathHelper.TwoPi / 6 * i);
                Main.EntitySpriteDraw(texture, drawPos + offset, frame, glow, Projectile.rotation, origin, Projectile.scale, se, 0);
            }

            Main.EntitySpriteDraw(texture, drawPos, frame, lightColor, Projectile.rotation, origin, Projectile.scale, se, 0);
        }
    }

    public class MeatknifeManager : ModProjectile
    {
        public override string Texture => "Terraria/Images/Projectile_0";
        private int knife1 = -1, knife2 = -1, knife3 = -1;

        public override void SetDefaults()
        {
            Projectile.width = 10; Projectile.height = 10;
            Projectile.aiStyle = -1;
            Projectile.friendly = Projectile.hostile = false;
            Projectile.tileCollide = false;
            Projectile.hide = true;
            Projectile.timeLeft = 999999;
        }

        public override void AI()
        {
            Player owner = Main.player[Projectile.owner];
            if (!owner.active || owner.dead || owner.HeldItem.type != ModContent.ItemType<Rhabdomyolysis>())
            {
                DespawnKnives();
                Projectile.Kill();
                return;
            }

            if (knife1 < 0 || !Main.projectile[knife1].active) knife1 = SpawnKnife<Meatknife1>(owner);
            if (knife2 < 0 || !Main.projectile[knife2].active) knife2 = SpawnKnife<Meatknife2>(owner);
            if (knife3 < 0 || !Main.projectile[knife3].active) knife3 = SpawnKnife<Meatknife3>(owner);
        }

        private int SpawnKnife<T>(Player owner) where T : ModProjectile
        {
            return Projectile.NewProjectile(Projectile.GetSource_FromThis(), owner.Center, Vector2.Zero, ModContent.ProjectileType<T>(), 0, 0, owner.whoAmI);
        }

        private void DespawnKnives()
        {
            KillIfValid(knife1); KillIfValid(knife2); KillIfValid(knife3);
        }

        private void KillIfValid(int index)
        {
            if (index >= 0 && index < Main.maxProjectiles)
                Main.projectile[index].Kill();
        }
    }
}