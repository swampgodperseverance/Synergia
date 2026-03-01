using Avalon.Projectiles;
using Bismuth.Content.Items.Armor;
using Microsoft.CodeAnalysis;
using ParticleLibrary.Utilities;
using ReLogic.Content;
using Synergia.Common.ModSystems;
using Synergia.Content.Buffs.Debuff.FadingHellFires;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.Graphics;
using Terraria.ID;
using Terraria.Utilities.Terraria.Utilities;
using static Synergia.Common.GlobalItems.FadingHellData;

namespace Synergia.Common.GlobalPlayer.Armor
{
    public class FadingHellPlayer : ModPlayer
    {
        public bool isSetBonus = false;
        public bool isOnFire = false;
        public bool isCastingFire = false;
        public FireType currentFireType = FireType.None;
        public override void Initialize()
        {
            isSetBonus = false;
            isOnFire = false;
            isCastingFire = false;
            currentFireType = FireType.None;
        }
        public override void ResetEffects()
        {
            isSetBonus = false;
            isOnFire = false;
            currentFireType = FireType.None;
        }
        public override void UpdateDead()
        {
            isCastingFire = false;
        }
        public override bool ModifyNurseHeal(NPC nurse, ref int health, ref bool removeDebuffs, ref string chatText)
        {
            if(!(isSetBonus && isOnFire))
                return base.ModifyNurseHeal(nurse, ref health, ref removeDebuffs, ref chatText);
            chatText = "OH GOD YOU'RE BURNING! I can't help you until you do something with it!";
            return false;
        }
        public override void PostUpdateEquips()
        {
            if (!isSetBonus)
            {
                if(!isOnFire)
                    return;
                ClearFire();
                return;
            }
            if (!isOnFire)
                KeybindCheck();
            else
            {
                Lighting.AddLight(Player.Center + new Vector2(0f, -20f), GetFireData(currentFireType).Value.Color.WithAlpha(1f).ToVector3());
                switch (currentFireType)
                {
                    case FireType.Frostburn:
                        Player.GetModPlayer<FadingHellFrostburnShield>().IsActive = true;
                        break;
                    case FireType.Shadowflame:
                        Player.GetModPlayer<FadingHellShadowflameDodge>().IsActive = true;
                        break;
                }
            }
            CheckOtherFireDebuffs();
        }
        public override void UpdateBadLifeRegen()
        {
            if (!isOnFire)
                return;

            if (Player.lifeRegen > 0)
                Player.lifeRegen = 0;
            Player.lifeRegenTime = 0;
            Player.lifeRegen -= 8;
        }

        private void KeybindCheck()
        {
            FireType fireType = FireType.None;
            if (VanillaKeybinds.ArmorSetBonusActivation.JustPressed && IsSuitableItem(Player.HeldItem, ref fireType) && !Player.wet && !isCastingFire)
            {
                if (Main.myPlayer != Player.whoAmI)
                    return;
                Projectile.NewProjectile(Player.GetSource_FromAI(), Player.MountedCenter, Vector2.Zero, ProjectileType<FadingHellHeldItem>(), 0, 0, Main.myPlayer, (float)fireType, Player.direction);
            }
        }
        public void ApplyFire(FireType fireType)
        {
            FadingHellFireData? fireData = GetFireData(fireType);
            int? buff = fireData?.ModdedDebuffID;
            if (!buff.HasValue) return;
            Player.AddBuff(buff.Value, 18000, false, false);
        }
        public void ClearFire()
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
                return;

            FadingHellFireData? fireData = GetFireData(currentFireType);
            int? buff = fireData?.ModdedDebuffID;
            if (!buff.HasValue) return;
            Player.ClearBuff(buff.Value);
        }
        private void CheckOtherFireDebuffs()
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
                return;

            FireType fireType = FireType.None;

            if (Player.HasBuff(BuffID.OnFire))
            {
                Player.ClearBuff(BuffID.OnFire);
                fireType = FireType.OnFire;
            }
            else if (Player.HasBuff(BuffID.CursedInferno))
            {
                Player.ClearBuff(BuffID.CursedInferno);
                fireType = FireType.CursedInferno;
            }
            else if (Player.HasBuff(BuffID.Frostburn))
            {
                Player.ClearBuff(BuffID.Frostburn);
                fireType = FireType.Frostburn;
            }
            else if (Player.HasBuff(BuffID.Frostburn2))
            {
                Player.ClearBuff(BuffID.Frostburn2);
                fireType = FireType.Frostburn;
            }
            else if (Player.HasBuff(BuffID.ShadowFlame))
            {
                Player.ClearBuff(BuffID.ShadowFlame);
                fireType = FireType.Shadowflame;
            }

            if (!isOnFire)
                ApplyFire(fireType);
        }
        public override void DrawEffects(PlayerDrawSet drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
        {
            if (!isOnFire)
                return;

            if (Player.miscCounter % 25 != 0)
                return;
            ulong seed = (ulong)Player.miscCounter;
            Vector2 velocity = new Vector2(Utils.RandomFloat(ref seed) * 0.5f - 0.25f, -(Utils.RandomFloat(ref seed) * 0.75f + 0.25f));
            int dustIndex = Dust.NewDustPerfect(
                Player.Center + new Vector2(0, -20f),
                DustID.Smoke,
                velocity,
                160,
                default,
                1.25f
            ).dustIndex;
            drawInfo.DustCache.Add(dustIndex);
        }
        public override void HideDrawLayers(PlayerDrawSet drawInfo)
        {
            if (isCastingFire)
                PlayerDrawLayers.HeldItem.Hide();
        }
    }
    public class FadingHellHeldItem : ModProjectile
    {
        public ref float FireType => ref Projectile.ai[0];
        public override void SetDefaults()
        {
            Projectile.damage = 0;
            Projectile.aiStyle = -1;
            Projectile.timeLeft = 90;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.width = 1;
            Projectile.height = 1;
        }
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Vector2 playerCenter = player.RotatedRelativePoint(player.MountedCenter);

            if(Main.myPlayer == Projectile.owner)
            {
                if (Projectile.timeLeft == 35)
                {
                    player.GetModPlayer<FadingHellPlayer>().ApplyFire((FireType)FireType);
                    EmitDust();
                    SoundEngine.PlaySound(SoundID.Item20, Projectile.Center);
                }
            }

            Projectile.direction = (int)Projectile.ai[1];
            Projectile.spriteDirection = Projectile.direction;
            Projectile.Center = playerCenter;
            player.ChangeDir(Projectile.direction);
            player.heldProj = Projectile.whoAmI;
            player.SetDummyItemTime(2);
            player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, MathHelper.PiOver2 * -Projectile.direction);
            player.GetModPlayer<FadingHellPlayer>().isCastingFire = true;
        }
        private void EmitDust()
        {
            if (Main.myPlayer != Projectile.owner) return;
            FadingHellFireData? fireData = GetFireData((FireType)FireType);
            int? dust = fireData.Value.DustID;
            if (dust == null) dust = DustID.Torch;
            for(int i = 0; i < 20; i++)
            {
                float seed = i * 102.481f + Main.GlobalTimeWrappedHourly;
                Vector2 velocity = Vector2.UnitX.RotatedBy(MathHelper.TwoPi / 20 * i) * (2.5f + (float)Math.Sin(seed) * 0.5f);
                Dust.NewDustPerfect(
                    Projectile.Center,
                    dust.Value,
                    velocity,
                    120,
                    default,
                    1.25f
                );
            }
        }
        public override void OnKill(int timeLeft)
        {
            Player player = Main.player[Projectile.owner];
            player.SetDummyItemTime(0);
            player.GetModPlayer<FadingHellPlayer>().isCastingFire = false;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Player player = Main.player[Projectile.owner];
            Item item = player.HeldItem;

            Texture2D texture = TextureAssets.Item[item.type].Value;
            //float diagonalOffset = 6f;
            //float offsetX = 8f - diagonalOffset;
            //float offsetY = diagonalOffset;
            Vector2 offset = new Vector2(2f * player.direction, 6f);
            Vector2 position = Projectile.position + offset - Main.screenPosition;
            Vector2 origin = new Vector2(player.direction == 1 ? 0 : texture.Width, texture.Height);

            Color color = Color.Transparent;
            Vector2 shake = Vector2.Zero;
            float progress = 0f;
            Asset<Texture2D> rayAsset = ModContent.Request<Texture2D>("Synergia/Assets/Textures/Ray");
            Vector2 rayOffset = offset + new Vector2(texture.Width * player.direction, -texture.Height) / 2;
            FadingHellFireData? fireData = GetFireData((FireType)FireType);
            Color? fireColor = fireData?.Color;
            if (!fireColor.HasValue) fireColor = Color.White;

            if(Projectile.timeLeft > 70)
            {
                progress = (90 - Projectile.timeLeft) / 20f;
                color = lightColor;
                shake = Vector2.Zero;

                if (rayAsset != null && rayAsset.IsLoaded)
                    RaysDraw(rayAsset.Value, progress, rayOffset, fireColor, item, 0);
            }
            else if (Projectile.timeLeft > 35)
            {
                progress = (70 - Projectile.timeLeft) / 35f;
                color = Color.Lerp(lightColor, Color.Black, progress);
                shake = Vector2.UnitX.RotatedByRandom(MathHelper.TwoPi) * progress * 4f;

                if (rayAsset != null && rayAsset.IsLoaded)
                    RaysDraw(rayAsset.Value, progress, rayOffset, fireColor, item, 1);
            }
            else
            {
                progress = Math.Clamp((35 - Projectile.timeLeft) / 25f, 0, 1);
                color = Color.Lerp(new Color(255, 255, 255, 40), lightColor, progress);
                shake = Vector2.UnitX.RotatedByRandom(MathHelper.TwoPi) * (1f - progress) * 2f;

                if (rayAsset != null && rayAsset.IsLoaded)
                    RaysDraw(rayAsset.Value, progress, rayOffset, fireColor, item, 2);
                Asset<Texture2D> glowAsset = ModContent.Request<Texture2D>("Synergia/Assets/Textures/Glow");
                if (glowAsset != null && glowAsset.IsLoaded)
                {
                    Texture2D glowTexture = glowAsset.Value;
                    Color newGlowColor = Color.Lerp(fireColor.Value, Color.Transparent, progress);
                    Vector2 glowPosition = Projectile.Center + new Vector2(0, -20f) - Main.screenPosition;
                    Vector2 glowOrigin = glowTexture.Size() / 2f;
                    float glowScale = 0.4f * progress;
                    Main.EntitySpriteDraw(
                        glowTexture,
                        glowPosition,
                        null,
                        newGlowColor,
                        0,
                        glowOrigin,
                        glowScale,
                        SpriteEffects.None,
                        0
                    );
                }
            }

            Main.EntitySpriteDraw(
                texture,
                position + shake,
                null,
                color,
                player.bodyRotation,
                origin,
                1f,
                player.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally,
                0
            );
            return false;
        }
        private void RaysDraw(Texture2D rayTexture, float progress, Vector2 offset, Color? fireColor, Item item, int type)
        {
            Vector2 position = Projectile.Center + offset - Main.screenPosition;
            Vector2 rayOrigin = new Vector2(rayTexture.Width / 2f, rayTexture.Height);
            Color newFireColor = fireColor.Value;
            float rayScaleMultiplier = 1f;
            switch (type)
            {
                case 0:
                    newFireColor = Color.Lerp(Color.Transparent, fireColor.Value, progress);
                    break;
                case 1:
                    rayScaleMultiplier = 1f - progress;
                    break;
                case 2:
                    rayScaleMultiplier = progress * 1.75f;
                    newFireColor = Color.Lerp(fireColor.Value, Color.Transparent, progress);
                    break;
            }
            for (int i = 0; i < 13; i++)
            {
                float seed = i + item.type * 17.581f;
                float rotation = (float)Math.Sin(seed) * Main.GlobalTimeWrappedHourly;
                Vector2 rayScale = new Vector2(0.35f, 0.40f + (float)Math.Cos(seed + Main.GlobalTimeWrappedHourly * 0.02f) * 0.2f) * rayScaleMultiplier;
                Main.EntitySpriteDraw(
                    rayTexture,
                    position,
                    null,
                    newFireColor,
                    rotation,
                    rayOrigin,
                    rayScale,
                    SpriteEffects.None,
                    0
                );
            }
        }
        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            overPlayers.Add(index);
        }
    }
}