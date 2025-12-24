using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.GameContent;
using Microsoft.Xna.Framework.Graphics;
using System;
using ReLogic.Content;
using static Synergia.Common.SUtils.LocUtil;    

namespace Synergia.Content.NPCs
{
    public class HellheartMonolith : ModNPC
    {
        public override string LocalizationCategory => Category(CategoryName.NPC);

        private float breathTimer;
        private float strongBreathTimer;
        private static Asset<Texture2D> rayTexture;

        public override void Load()
        {
            rayTexture = ModContent.Request<Texture2D>("Synergia/Assets/Textures/Ray");
        }

        public override void Unload()
        {
            rayTexture = null;
        }

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 1;
        }

        public override void SetDefaults()
        {
            NPC.width = 95;
            NPC.height = 170;
            NPC.alpha = 15;
            NPC.lifeMax = 25000;
            NPC.damage = 0;
            NPC.defense = 18;
            NPC.aiStyle = -1;
            NPC.knockBackResist = 0f;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.lavaImmune = true;
            NPC.HitSound = SoundID.Tink;
            NPC.DeathSound = SoundID.NPCDeath14;
            NPC.value = 0f;
        }

        public override void AI()
        {
            breathTimer += 0.025f;
            strongBreathTimer += 0.01f;

            float basePulse = (float)Math.Sin(breathTimer) * 0.04f;
            float strongPulse = (float)Math.Sin(strongBreathTimer * 0.7f) * 0.12f;
            NPC.scale = 1f + basePulse + strongPulse;

            float currentPulse = basePulse + strongPulse;
            bool isInhaling = currentPulse < -0.04f;

            if (isInhaling)
            {
                float attractRange = 380f;
                float attractStrength = 0.28f + Math.Abs(currentPulse) * 2.2f;

                foreach (Dust dust in Main.dust)
                {
                    if (!dust.active) continue;

                    // Притягиваем ВСЕ дасты (кроме тех, что явно не должны двигаться, типа некоторых эффектов)
                    // Исключаем только очень "статичные" или системные, если нужно — можно добавить исключения
                    float dist = Vector2.Distance(dust.position, NPC.Center);
                    if (dist < attractRange && dist > 40f)
                    {
                        Vector2 direction = NPC.Center - dust.position;
                        direction.Normalize();

                        // Чем дальше — тем слабее влияние, но всё равно тянет
                        float distanceFactor = 1f - (dist / attractRange);

                        dust.velocity = Vector2.Lerp(dust.velocity, direction * (6f + distanceFactor * 4f), attractStrength * 0.04f);

                        dust.scale *= 0.990f;

                        if (dist < 100f)
                        {
                            dust.velocity *= 0.5f;
                            dust.alpha += 12 + (int)(distanceFactor * 20);
                            if (dust.alpha > 210) dust.active = false;
                        }
                    }
                }
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (rayTexture != null && rayTexture.IsLoaded)
            {
                Texture2D rayTex = rayTexture.Value;
                Vector2 rayPosition = NPC.Center - screenPos;
                Vector2 rayOrigin = new Vector2(rayTex.Width / 2f, rayTex.Height);
                const int RAY_COUNT = 8;
                float globalRotation = Main.GlobalTimeWrappedHourly * 0.25f;

                float extraRayPulse = (float)Math.Sin(breathTimer) * 0.06f + (float)Math.Sin(strongBreathTimer * 0.7f) * 0.15f;
                float rayScaleBonus = 1f + extraRayPulse * 1.2f;

                for (int k = 0; k < RAY_COUNT; k++)
                {
                    float seed = k + NPC.whoAmI * 13.37f;
                    float angleOffset = (float)Math.Sin(seed) * 0.3f + (float)Math.Cos(seed * 1.7f) * 0.2f;
                    float angle = (float)k / RAY_COUNT * MathHelper.TwoPi + globalRotation + angleOffset;
                    float breathSpeed = 1.2f + (float)Math.Sin(seed) * 0.8f;
                    float breathPhase = seed * 1.5f;
                    float breathAmp = 0.4f + (float)Math.Cos(seed) * 0.3f;
                    float breath = (float)Math.Sin(Main.GlobalTimeWrappedHourly * breathSpeed + breathPhase);
                    float breathFactor = 1f + breath * breathAmp;
                    float baseWidth = 0.45f + (float)Math.Sin(seed * 2.3f) * 0.4f;
                    float baseLength = 0.9f + (float)Math.Cos(seed * 1.8f) * 0.4f;

                    Vector2 rayScale = new Vector2(baseWidth, baseLength * breathFactor) * NPC.scale * rayScaleBonus;

                    float opacityBase = 0.2f + (float)Math.Sin(seed * 3.1f) * 0.15f;
                    float opacityPulse = 0.3f + breath * 0.3f;
                    float finalOpacity = opacityBase + opacityPulse * 0.6f;
                    float colorShift = (float)Math.Sin(seed) * 0.5f + 0.5f;
                    Color baseColor = Color.Lerp(new Color(255, 80, 40), new Color(255, 180, 100), colorShift);
                    Color rayColor = baseColor * finalOpacity;

                    spriteBatch.Draw(
                        rayTex,
                        rayPosition,
                        null,
                        rayColor,
                        angle,
                        rayOrigin,
                        rayScale,
                        SpriteEffects.None,
                        0f
                    );
                }
            }

            Texture2D texture = TextureAssets.Npc[NPC.type].Value;
            Vector2 origin = texture.Size() / 2f;
            Vector2 drawPos = NPC.Center - screenPos;
            Color crystalColor = Color.White * 0.92f;

            spriteBatch.Draw(
                texture,
                drawPos,
                null,
                crystalColor,
                0f,
                origin,
                NPC.scale,
                SpriteEffects.None,
                0f
            );

            return false;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D glow = ModContent.Request<Texture2D>(Texture + "_Glow").Value;
            Vector2 origin = glow.Size() / 2f;
            float glowPulse = 0.65f + (float)Math.Sin(Main.GlobalTimeWrappedHourly * 2.2f) * 0.25f;

            spriteBatch.Draw(
                glow,
                NPC.Center - screenPos,
                null,
                Color.White * glowPulse,
                0f,
                origin,
                NPC.scale,
                SpriteEffects.None,
                0f
            );
        }

        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            return false;
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.frame.Y = 0;
        }
    }
}