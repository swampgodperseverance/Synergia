using Avalon.Dusts;
using Avalon.Items.Material.Bars;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Synergia.Content.Items.Weapons.Throwing;

public class NaquadahDagger : ModItem
{
    public override void SetDefaults()
    {
        Item.damage = 26;
        Item.DamageType = DamageClass.Throwing;
        Item.noMelee = true;
        Item.width = 20;
        Item.height = 20;
        Item.useTime = 16;
        Item.useAnimation = 16;
        Item.knockBack = 2;
        Item.value = Item.buyPrice(0, 0, 6, 0);
        Item.rare = 3;
        Item.UseSound = SoundID.Item1;
        Item.autoReuse = true;
        Item.shoot = ModContent.ProjectileType<NaquadahDaggerProj>();
        Item.shootSpeed = 9f;
        Item.useTurn = true;
        Item.useStyle = 1;
        Item.noUseGraphic = true;
        Item.maxStack = 9999;
        Item.consumable = true;
    }

    private int shootCount = 0;

    public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
    {
        shootCount++;

        int projectilesToShoot = (shootCount % 2 == 1) ? 1 : 2;

        if (projectilesToShoot == 1)
        {
            Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, type, damage, knockback, player.whoAmI);
        }
        else
        {
            float spread = 0.2f;
            float speed = velocity.Length();

            Vector2 vel1 = velocity.RotatedBy(-spread / 2);
            Projectile.NewProjectile(source, position.X, position.Y, vel1.X, vel1.Y, type, damage, knockback, player.whoAmI);

            Vector2 vel2 = velocity.RotatedBy(spread / 2);
            Projectile.NewProjectile(source, position.X, position.Y, vel2.X, vel2.Y, type, damage, knockback, player.whoAmI);
        }

        return false;
    }

    public override void AddRecipes()
    {
        CreateRecipe(50)
            .AddIngredient(ModContent.ItemType<NaquadahBar>(), 1)
            .AddTile(TileID.MythrilAnvil)
            .Register();
    }
}

public class NaquadahDaggerProj : ModProjectile
{
    private int trailTimer = 0;
    private const int TRAIL_LENGTH = 5;

    public override void SetDefaults()
    {
        Projectile.width = 24;
        Projectile.height = 40;
        Projectile.friendly = true;
        Projectile.DamageType = DamageClass.Throwing;
        Projectile.penetrate = 1;
        Projectile.extraUpdates = 1;
        Projectile.aiStyle = 1;
        AIType = ProjectileID.ThrowingKnife;
        Projectile.tileCollide = true;
        Projectile.CloneDefaults(ProjectileID.ThrowingKnife);
    }

    public override bool PreDraw(ref Color lightColor)
    {
        Texture2D texture = ModContent.Request<Texture2D>(Texture).Value;
        Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
        Vector2 drawPos = Projectile.Center - Main.screenPosition;

        for (int i = 1; i <= TRAIL_LENGTH; i++)
        {
            if (i > trailTimer) break;

            float opacity = 1f - (float)i / TRAIL_LENGTH;
            float scale = 1f - (float)i / TRAIL_LENGTH * 0.3f;
            Vector2 offset = -Projectile.velocity * i * 0.5f;

            Main.EntitySpriteDraw(
                texture,
                drawPos + offset,
                null,
                lightColor * opacity * 0.3f,
                Projectile.rotation,
                drawOrigin,
                Projectile.scale * scale,
                SpriteEffects.None,
                0
            );
        }

        Main.EntitySpriteDraw(
            texture,
            drawPos,
            null,
            lightColor,
            Projectile.rotation,
            drawOrigin,
            Projectile.scale,
            SpriteEffects.None,
            0
        );

        return false;
    }

    public override void AI()
    {
        trailTimer++;
        if (trailTimer > TRAIL_LENGTH)
            trailTimer = TRAIL_LENGTH;
    }

    const int dust_count = 10;

    public override bool OnTileCollide(Vector2 oldVelocity)
    {
        for (int counter = 0; counter < dust_count; counter++)
        {
            Vector2 velocity = Projectile.velocity * ((float)Main.rand.Next(20, 140) / 100f);
            Dust.NewDust(
                Projectile.Center,
                1,
                1,
                ModContent.DustType<NaquadahDust>(),
                velocity.X,
                velocity.Y
            );
        }
        SoundEngine.PlaySound(SoundID.Dig, Projectile.position);
        return true;
    }
}