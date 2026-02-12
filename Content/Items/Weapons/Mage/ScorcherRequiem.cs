using System;
using System.Collections.Generic;
using System.IO;
using Avalon.Common;
using Avalon.Common.Extensions;
using Avalon.Common.Templates;
using Avalon.Particles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Synergia.Helpers;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Drawing;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using static Synergia.Content.Items.Weapons.Mage.ScorcherLaser;

namespace Synergia.Content.Items.Weapons.Mage;

public class ScorcherRequiem : ModItem
{
    public override void SetDefaults()
    {
        Item.width = 40;
        Item.height = 40;
        Item.damage = 70;
        Item.DamageType = DamageClass.Magic;
        Item.mana = 10;
        Item.knockBack = 2f;
        Item.crit = 6;
        Item.useTime = 20;
        Item.useAnimation = 20;
        Item.useStyle = ItemUseStyleID.Shoot;
        Item.noMelee = true;
        Item.shoot = ModContent.ProjectileType<ScorcherHeldItemProj>();
        Item.noUseGraphic = true;
        Item.shootSpeed = 10f;
        Item.autoReuse = true;
        Item.channel = true;
        Item.rare = ItemRarityID.Lime;
        Item.value = Item.sellPrice(90, 4);
    }

    public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) { }

    public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
    {
        type = Item.shoot;
        Projectile.NewProjectile(source, position, Vector2.Zero, type, damage, knockback, Main.myPlayer);
        return false;
    }

    public override void AddRecipes() { }
}

public class ScorcherHeldItemProj : ModProjectile
{
    private const float HoldOffset = 20f;
    private const float AttackSpeed = 30f;
    private const float RotationOffset = 60f / 180f * MathHelper.Pi;
    private const float RotationOffsetSpeed = 15f;

    public ref float AttackTimer => ref Projectile.ai[0];
    public ref float RotationOffsetTimer => ref Projectile.ai[1];
    public ref float HeatBuff => ref Projectile.ai[2];

    public override void SetDefaults()
    {
        Projectile.width = 48;
        Projectile.height = 28;
        Projectile.aiStyle = -1;
        Projectile.friendly = false;
        Projectile.hostile = false;
        Projectile.penetrate = -1;
        Projectile.tileCollide = false;
        Projectile.timeLeft = 3;
    }

    public override bool? CanDamage() => false;

    public override void OnSpawn(IEntitySource source)
    {
        Player player = Main.player[Projectile.owner];
        Item item = player.HeldItem;
        player.statMana += player.GetManaCost(item);
        base.OnSpawn(source);
    }

    public override void AI()
    {
        Player player = Main.player[Projectile.owner];
        Item item = player.HeldItem;
        Vector2 playerCenter = player.RotatedRelativePoint(player.MountedCenter);

        if (RotationOffsetTimer > 0)
            RotationOffsetTimer--;

        AttackTimer++;

        if (Main.myPlayer == Projectile.owner)
        {
            if (!player.channel || !player.CheckMana(player.GetManaCost(item)))
            {
                Projectile.Kill();
                return;
            }

            Vector2 direction = HoldOffset * Vector2.Normalize(Main.MouseWorld - playerCenter);
            if (direction.X != Projectile.velocity.X || direction.Y != Projectile.velocity.Y)
                Projectile.netUpdate = true;
            Projectile.velocity = direction;

            if (AttackTimer >= AttackSpeed)
            {
                AttackTimer = (int)HeatBuff;
                RotationOffsetTimer = RotationOffsetSpeed;
                HeatBuff = Math.Clamp(HeatBuff + 0.75f, 0, 12f);
                SpawnLaser(playerCenter, player);
                player.CheckMana(player.GetManaCost(item), true);
                Projectile.netUpdate = true;
            }
        }

        Projectile.direction = Projectile.velocity.X < 0 ? -1 : 1;
        Projectile.spriteDirection = Projectile.direction;
        player.ChangeDir(Projectile.direction);
        player.heldProj = Projectile.whoAmI;
        player.SetDummyItemTime(2);
        Projectile.Center = playerCenter;
        Projectile.timeLeft = 2;

        float rotationOffset = Projectile.spriteDirection == -1 ? MathHelper.Pi : 0;
        Projectile.rotation = Projectile.velocity.ToRotation() + rotationOffset;
        player.itemRotation = (Projectile.velocity * Projectile.direction).ToRotation();
    }

    private void SpawnLaser(Vector2 center, Player player)
    {
        Vector2 velocity = Vector2.Normalize(Projectile.velocity) * 10f;
        int type = ModContent.ProjectileType<ScorcherLaser>();
        Vector2 muzzleOffset = center + Projectile.velocity + new Vector2(Projectile.width / 2, -7 * Projectile.direction).RotatedBy(velocity.ToRotation());
        Vector2 beamStartPos = muzzleOffset;

        Projectile.NewProjectile(Projectile.GetSource_FromAI(), muzzleOffset, velocity, type, Projectile.damage, Projectile.knockBack, player.whoAmI);

        Projectile.NewProjectile(
            Projectile.GetSource_FromAI(),
            muzzleOffset,
            Vector2.Zero,
            ModContent.ProjectileType<ScorcherMuzzleFlash>(),
            0,
            0f,
            player.whoAmI
        );

        ParticleSystem.AddParticle(new EnergyRevolverParticle(), beamStartPos, Vector2.Normalize(velocity) * 2, new Color(255, 140, 0, 0), 0, 0.8f, 14);
        ParticleSystem.AddParticle(new EnergyRevolverParticle(), beamStartPos, default, new Color(255, 69, 0, 0), 0, 1, 20);

        var settings = new ParticleOrchestraSettings
        {
            PositionInWorld = muzzleOffset,
            MovementVector = velocity * 0.4f,
            UniqueInfoPiece = 1,
            IndexOfPlayerWhoInvokedThis = (byte)player.whoAmI
        };

        ParticleOrchestrator.RequestParticleSpawn(clientOnly: false, ParticleOrchestraType.FlameWaders, settings, player.whoAmI);

        SoundEngine.PlaySound(new SoundStyle("Synergia/Assets/Sounds/Lasershot"), player.Center);
    }

    public override bool PreDraw(ref Color lightColor)
    {
        Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
        float progress = (AttackTimer - 10) / (AttackSpeed - 10);
        Vector2 shakeOffset = AttackTimer < 10 ? Vector2.Zero : Main.rand.NextVector2Unit() * 2f * progress;
        Vector2 position = Projectile.Center - Main.screenPosition + shakeOffset;

        float rotationOffset = RotationOffsetTimer > 0 ? RotationOffset - RotationOffset * EaseFunctions.EaseOutBack((RotationOffsetSpeed - RotationOffsetTimer) / RotationOffsetSpeed) : 0f;
        float rotation = Projectile.rotation - rotationOffset * Projectile.direction;
        Vector2 origin = texture.Size() / 2;
        float scale = Projectile.scale;
        Color color = AttackTimer < 10 ? lightColor : Color.Lerp(lightColor, new Color(240, 150, 80), progress);

        Main.EntitySpriteDraw(
            texture,
            position,
            null,
            color,
            rotation,
            origin,
            scale,
            Projectile.direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None,
            0
        );
        return false;
    }
}

public class ScorcherLaser : ModProjectile
{
    public ref float Lifetime => ref Projectile.ai[0];
    public ref float HitCount => ref Projectile.ai[1];

    private readonly List<int> hittedTargets = new();
    private readonly List<Vector2> hitPositions = new();
    private Vector2 spawnPos;

    public override void SetStaticDefaults()
    {
        ProjectileID.Sets.DrawScreenCheckFluff[Type] = 4800;
    }

    public override void SetDefaults()
    {
        Projectile.width = 9;
        Projectile.height = 9;
        Projectile.aiStyle = -1;
        Projectile.friendly = true;
        Projectile.penetrate = -1;
        Projectile.tileCollide = true;
        Projectile.DamageType = DamageClass.Magic;
        Projectile.usesLocalNPCImmunity = true;
        Projectile.localNPCHitCooldown = -1;
        Projectile.timeLeft = 6600;
        Projectile.extraUpdates = 200;
        Projectile.alpha = 0;
    }

    public override void OnSpawn(IEntitySource source)
    {
        spawnPos = Projectile.Center;
        base.OnSpawn(source);
    }

    public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
    {
        width = 0; height = 0;
        return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
    }

    public override void AI()
    {
        Lifetime++;

        if (Lifetime == 200)
        {
            Projectile.damage = 0;
            hitPositions.Add(Projectile.Center);
            ParticleSystem.AddParticle(new EnergyRevolverParticle(), Projectile.Center, default, new Color(64, 255, 255, 0), 0, 1, 14);
            Projectile.velocity = Vector2.Zero;
            for (int i = 0; i < 10; i++)
            {
                Dust d = Dust.NewDustPerfect(Projectile.Center, DustID.Torch, Main.rand.NextVector2Circular(3, 3), 24);
                d.scale *= 2;
                d.noGravity = true;
            }
            Projectile.netUpdate = true;
        }

        if (Projectile.timeLeft % 20 == 0)
            Projectile.alpha += 1;

        if (Projectile.alpha >= 255)
            Projectile.Kill();

        if (Projectile.position.X <= 16 || Projectile.position.X >= (Main.maxTilesX - 1) * 16 ||
            Projectile.position.Y <= 16 || Projectile.position.Y >= (Main.maxTilesY - 1) * 16)
        {
            Projectile.velocity = Vector2.Zero;
        }
    }

    public override void SendExtraAI(BinaryWriter writer)
    {
        writer.WriteVector2(spawnPos);
        int hittedTagretsCount = hittedTargets.Count;
        writer.Write(hittedTagretsCount);
        for (int i = 0; i < hittedTagretsCount; i++)
            writer.Write(hittedTargets[i]);
        int hitPositionsCount = hitPositions.Count;
        writer.Write(hitPositionsCount);
        for (int i = 0; i < hitPositionsCount; i++)
            writer.WriteVector2(hitPositions[i]);
    }

    public override void ReceiveExtraAI(BinaryReader reader)
    {
        spawnPos = reader.ReadVector2();
        int hittedTagretsCount = reader.ReadInt32();
        hittedTargets.Clear();
        for (int i = 0; i < hittedTagretsCount; i++)
            hittedTargets.Add(reader.ReadInt32());
        int hitPositionsCount = reader.ReadInt32();
        hitPositions.Clear();
        for (int i = 0; i < hitPositionsCount; i++)
            hitPositions.Add(reader.ReadVector2());
    }

    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        if (Main.rand.NextBool(6))
            target.AddBuff(BuffID.OnFire3, 60 * 3);
        OnHit();
        hittedTargets.Add(target.whoAmI);
        HitCount++;
        if (HitCount > 6) return;

        if (Main.myPlayer == Projectile.owner)
        {
            int? nextTarget = NPCHelper.FindClosestNPC(Projectile.Center, 0, 480, false, true, hittedTargets);
            if (nextTarget == null) return;
            NPC npc = Main.npc[nextTarget.Value];
            Vector2 velocity = Vector2.Normalize(npc.Center - Projectile.Center) * 10f;
            Projectile.velocity = velocity;
        }
    }

    public override void OnHitPlayer(Player target, Player.HurtInfo info)
    {
        if (Main.rand.NextBool(6))
            target.AddBuff(BuffID.OnFire3, 60 * 3);
        OnHit();
    }

    private void OnHit()
    {
        hitPositions.Add(Projectile.Center);
        Projectile.damage = (int)(Projectile.damage * 0.95f);
        ParticleSystem.AddParticle(new EnergyRevolverParticle(), Projectile.Center, default, new Color(64, 128, 255, 0), 0, Main.rand.NextFloat(0.9f, 1.1f), 14);
        for (int i = 0; i < 10; i++)
        {
            Dust d = Dust.NewDustPerfect(Projectile.Center, DustID.Torch, Main.rand.NextVector2Circular(3, 3), 24);
            d.scale *= 2;
            d.noGravity = true;
        }
    }

    public override bool OnTileCollide(Vector2 oldVelocity)
    {
        Projectile.velocity = Vector2.Zero;
        return false;
    }

    public override bool PreDraw(ref Color lightColor)
    {
        if (hitPositions.Count == 0) return false;

        Texture2D texture = TextureAssets.Projectile[Type].Value;
        Vector2 startPos = spawnPos;

        for (int i = 0; i < hitPositions.Count; i++)
        {
            Vector2 endPos = hitPositions[i];
            Main.EntitySpriteDraw(
                texture: texture,
                position: endPos - Main.screenPosition,
                sourceRectangle: null,
                color: new Color(Projectile.Opacity, Projectile.Opacity, 1f, 0),
                rotation: endPos.DirectionTo(startPos).ToRotation() + MathHelper.PiOver2,
                origin: new Vector2(texture.Width / 2f, texture.Height),
                scale: new Vector2(Projectile.Opacity * 3f, endPos.Distance(startPos)),
                effects: SpriteEffects.None,
                worthless: 0
            );
            startPos = endPos;
        }
        return false;
    }

    public class ScorcherMuzzleFlash : ModProjectile
    {
        public override string Texture => "Synergia/Assets/Textures/LightTrail_2";

        public override void SetDefaults()
        {
            Projectile.width = 64;
            Projectile.height = 64;
            Projectile.aiStyle = -1;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 24;
            Projectile.alpha = 0;
        }

        public override void AI()
        {
            if (Projectile.timeLeft > 16)
            {
                Projectile.scale = 1.1f + Main.rand.NextFloat(-0.15f, 0.25f);
                Projectile.rotation = Main.rand.NextFloat(-0.4f, 0.4f);
            }

            Projectile.alpha += 11;
            if (Projectile.alpha > 255) Projectile.alpha = 255;

            Projectile.scale *= 0.74f;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Type].Value;

            Color mainColor = new Color(255, 190, 70, 240) * (1f - Projectile.alpha / 255f * 0.92f);
            Color glowColor = new Color(255, 130, 30, 200) * (1f - Projectile.alpha / 255f);

            float baseScale = Projectile.scale * (1.2f + Main.rand.NextFloat(-0.2f, 0.2f));

            Main.EntitySpriteDraw(
                texture,
                Projectile.Center - Main.screenPosition,
                null,
                mainColor,
                Projectile.rotation,
                texture.Size() / 2f,
                baseScale * 1.3f,
                SpriteEffects.None,
                0
            );

            Main.EntitySpriteDraw(
                texture,
                Projectile.Center - Main.screenPosition,
                null,
                glowColor,
                Projectile.rotation + 0.5f,
                texture.Size() / 2f,
                baseScale * 0.9f,
                SpriteEffects.None,
                0
            );

            Main.EntitySpriteDraw(
                texture,
                Projectile.Center - Main.screenPosition,
                null,
                glowColor * 0.6f,
                Projectile.rotation - 0.4f,
                texture.Size() / 2f,
                baseScale * 1.7f,
                SpriteEffects.None,
                0
            );

            return false;
        }
    }
}