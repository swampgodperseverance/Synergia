using Avalon.Particles;
using Microsoft.Xna.Framework;
using Synergia.Content.Projectiles.Other;
using Synergia.Helpers;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Content.Items.Weapons.Melee
{
    public class Damasque : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            Item.staff[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.damage = 98;
            Item.DamageType = DamageClass.Melee;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAnimation = 18;
            Item.useTime = 6;
            Item.reuseDelay = 4;
            Item.knockBack = 3f;
            Item.value = Item.sellPrice(0, 0, 20, 0);
            Item.rare = ItemRarityID.White;
            Item.autoReuse = false;

            Item.noUseGraphic = false;
            Item.noMelee = true;

            Item.shoot = ModContent.ProjectileType<BaseShortswordCommon>();
            Item.shootSpeed = 25f;
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            velocity = velocity.RotatedByRandom(MathHelper.ToRadians(23)) * Main.rand.NextFloat(0.75f, 1.25f);
        }

        public override bool Shoot(Player player, Terraria.DataStructures.EntitySource_ItemUse_WithAmmo source,
            Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int combo = player.itemAnimation % 4;

        
            int proj = Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
            Main.projectile[proj].ai[0] = combo;

         
            Vector2 shootVel = velocity * 0.6f; 

            Projectile.NewProjectile(
                source,
                position,
                shootVel,
                ModContent.ProjectileType<DamasqueProj>(),
                damage / 2, 
                knockback * 0.5f,
                player.whoAmI
            );

            return false;
        }

        public override void AddRecipes()
        {
        }
    }

    class DamasqueProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 6;
            ProjectileID.Sets.TrailingMode[Type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.Size = new Vector2(18);
            Projectile.scale = 1f;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 60;
            Projectile.extraUpdates = 1;
        }

        public override void AI()
        {
            if (Projectile.velocity.Length() > 3f)
                Projectile.velocity *= 0.97f;

            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

            Projectile.ai[0] += 0.015f;
            if (Projectile.ai[0] > 1f)
                Projectile.ai[0] = 1f;

            if (Projectile.ai[0] < 0.75f && Main.rand.NextBool(4))
            {
                Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Torch);
                d.noGravity = true;
                d.scale = 1.1f;
                d.velocity *= 0.4f;
            }
        }

        void Explosion()
        {
            for (int i = 0; i < 3; i++)
                ParticleSystem.AddParticle(new EnergyRevolverParticle(), Projectile.Center, Main.rand.NextVector2Circular(1.5f, 1.5f), new Color(15, 15, 15), 0, 0.9f, 18);

            for (int i = 0; i < 4; i++)
                ParticleSystem.AddParticle(new EnergyRevolverParticle(), Projectile.Center, Main.rand.NextVector2Circular(2f, 2f), new Color(40, 40, 40), 0, 0.7f, 15);

            SoundEngine.PlaySound(SoundID.Item14, Projectile.Center);
        }

        public override void OnKill(int timeLeft)
        {
            Explosion();
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Explosion();
            Projectile.Kill();
            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Explosion();
            target.AddBuff(BuffID.OnFire, 240);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 origin = texture.Size() / 2;

            float t = Projectile.ai[0];

            Color lava = new Color(255, 120, 40);
            Color black = new Color(15, 15, 15);

            Color drawColor = Color.Lerp(lava, black, t * t);

            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                float progress = (Projectile.oldPos.Length - i) / (float)Projectile.oldPos.Length;

                Color color = drawColor * progress * 0.6f;

                Vector2 drawPos =
                    Projectile.oldPos[i] -
                    Main.screenPosition +
                    origin;

                Main.EntitySpriteDraw(
                    texture,
                    drawPos,
                    null,
                    color,
                    Projectile.rotation,
                    origin,
                    Projectile.scale,
                    SpriteEffects.None,
                    0
                );
            }

            Main.EntitySpriteDraw(
                texture,
                Projectile.Center - Main.screenPosition,
                null,
                drawColor,
                Projectile.rotation,
                origin,
                Projectile.scale,
                SpriteEffects.None,
                0
            );

            return false;
        }
    }
}
