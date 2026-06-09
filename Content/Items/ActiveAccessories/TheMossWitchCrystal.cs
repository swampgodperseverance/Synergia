using System;
using Bismuth.Content.Buffs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Synergia.Common;
using Synergia.Content.Projectiles.ActiveAccessoriesProjectiles;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using ValhallaMod.DamageClasses;
using ValhallaMod.Items.AI;
using static Terraria.ModLoader.ModContent;

namespace Synergia.Content.Items.ActiveAccessories
{
    [AutoloadEquip(EquipType.Neck)]
    public class TheMossWitchCrystal : ValhallaMod.Items.AI.ActiveAccessoryItem
    {
        public override void SetStaticDefaults()
        {
            ValhallaMod.Values.ActiveAccessoryItems[Item.type] = true;
        }

        public override void SafeSetDefaults()
        {
            Item.width = 28;
            Item.height = 34;
            Item.value = Item.sellPrice(0, 2, 25, 0);
            Item.rare = ItemRarityID.Purple;
            Item.accessory = true;
            Item.DamageType = GetInstance<AccessoryDamageClass>();
            Item.damage = 0;

            cooldown = 35 * 60;
        }

        public override bool Use(Player player, ref int time, ref int damage, ref bool silent)
        {
            for (int i = 0; i < Player.MaxBuffs; i++)
            {
                if (player.buffType[i] > 0 && Main.debuff[player.buffType[i]])
                {
                    player.DelBuff(i);
                    i--;
                }
            }

            int radius = 1500;
            foreach (NPC npc in Main.npc)
            {
                if (npc.active && !npc.friendly && npc.lifeMax > 5 && Vector2.Distance(player.Center, npc.Center) < radius)
                {
                    npc.AddBuff(BuffID.Poisoned, 300);
                }
            }

            SoundEngine.PlaySound(SoundID.Item43, player.Center);
            SoundEngine.PlaySound(SoundID.Item104, player.Center);

            for (int i = 0; i < 50; i++)
            {
                Dust dust = Dust.NewDustDirect(player.position, player.width, player.height,
                    DustID.GrassBlades, Main.rand.NextFloat(-3f, 3f), Main.rand.NextFloat(-3f, 3f),
                    100, Color.LimeGreen, 1.5f);
                dust.noGravity = true;

                Dust dust2 = Dust.NewDustDirect(player.position, player.width, player.height,
                    DustID.GreenMoss, Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-2f, 2f),
                    80, Color.Green, 1.2f);
                dust2.noGravity = true;
            }

            for (int i = 0; i < 20; i++)
            {
                Dust dust = Dust.NewDustDirect(player.Center - new Vector2(50, 50), 100, 100,
                    DustID.PlanteraBulb, Main.rand.NextFloat(-4f, 4f), Main.rand.NextFloat(-4f, 4f),
                    120, Color.ForestGreen, 1.8f);
                dust.noGravity = true;
            }

            for (int i = 0; i < 360; i += 15)
            {
                Vector2 offset = new Vector2(0, 60).RotatedBy(MathHelper.ToRadians(i));
                Dust dust = Dust.NewDustDirect(player.Center + offset - new Vector2(4, 4), 8, 8,
                    DustID.GreenMoss, 0, 0, 100, Color.LimeGreen, 1.3f);
                dust.noGravity = true;
                dust.velocity = offset * 0.1f;
            }

            var modPlayer = player.GetModPlayer<TheMossWitchCrystalVisual>();
            modPlayer.StartVisualEffect();

            return true;
        }

        public override void SafeUpdateAccessory(Player player, bool hideVisual)
        {
            player.buffImmune[BuffID.Poisoned] = true;
            player.buffImmune[BuffID.Venom] = true;

            int swampDebuffType = ModContent.BuffType<SwampQuagmire>();
            if (swampDebuffType > 0)
            {
                player.buffImmune[swampDebuffType] = true;
            }

            if (!hideVisual)
            {
                Lighting.AddLight(player.Center, 0.2f, 0.5f, 0.2f);
            }
        }
    }

    public class TheMossWitchCrystalVisual : ModPlayer
    {
        private bool effectActive = false;
        private int effectTimer = 0;

        public override void PostUpdate()
        {
            if (effectActive)
            {
                effectTimer--;
                if (effectTimer <= 0)
                {
                    effectActive = false;
                }
            }
        }

        public void StartVisualEffect()
        {
            effectActive = true;
            effectTimer = 60;
        }

        public override void ModifyDrawInfo(ref PlayerDrawSet drawInfo)
        {
            if (effectActive)
            {
                float progress = 1f - (effectTimer / 60f);
                float scale = 1f + progress * 2f;
                float opacity = 1f - progress;

                Texture2D ringTexture = ModContent.Request<Texture2D>("Synergia/Assets/Textures/Ring", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;

                Vector2 position = drawInfo.Position + new Vector2(drawInfo.drawPlayer.width / 2, drawInfo.drawPlayer.height / 2) - Main.screenPosition;

                float pulse = (float)(Math.Sin(Main.GameUpdateCount * 0.3f) * 0.3f + 0.7f);

                Color ringColor = Color.LimeGreen * opacity * 0.7f;
                Color ringColor2 = Color.ForestGreen * opacity * 0.5f;
                Color ringColor3 = Color.White * opacity * 0.3f;

                drawInfo.DrawDataCache.Add(new DrawData(
                    ringTexture,
                    position,
                    null,
                    ringColor,
                    0f,
                    new Vector2(ringTexture.Width / 2, ringTexture.Height / 2),
                    scale * pulse * 0.8f,
                    SpriteEffects.None,
                    0
                ));

                drawInfo.DrawDataCache.Add(new DrawData(
                    ringTexture,
                    position,
                    null,
                    ringColor2,
                    MathHelper.ToRadians(45f),
                    new Vector2(ringTexture.Width / 2, ringTexture.Height / 2),
                    scale * pulse * 0.6f,
                    SpriteEffects.None,
                    0
                ));

                drawInfo.DrawDataCache.Add(new DrawData(
                    ringTexture,
                    position,
                    null,
                    ringColor3,
                    MathHelper.ToRadians(90f),
                    new Vector2(ringTexture.Width / 2, ringTexture.Height / 2),
                    scale * pulse * 1.2f,
                    SpriteEffects.None,
                    0
                ));

                for (int i = 0; i < 8; i++)
                {
                    float angle = MathHelper.ToRadians(i * 45 + Main.GameUpdateCount * 6);
                    Vector2 offset = new Vector2(0, 50 * (1f - progress)).RotatedBy(angle);
                    Color glowColor = Color.LimeGreen * opacity * 0.6f;

                    drawInfo.DrawDataCache.Add(new DrawData(
                        ringTexture,
                        position + offset,
                        null,
                        glowColor,
                        angle,
                        new Vector2(ringTexture.Width / 2, ringTexture.Height / 2),
                        0.3f * pulse,
                        SpriteEffects.None,
                        0
                    ));
                }
            }
        }
    }
}