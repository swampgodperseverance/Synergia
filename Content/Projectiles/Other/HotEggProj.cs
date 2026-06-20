using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Synergia.Content.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ValhallaMod;
using ValhallaMod.Buffs.Pet;
using ValhallaMod.Projectiles.AI;

namespace Synergia.Content.Projectiles.Other
{
    public class HotEggProj : ValhallaWormPet
    {
        // Массив для хранения кадров для каждого сегмента
        private int[] segmentFrames;

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 4; // 4 кадра: голова, тело1, тело2, хвост
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 14;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            Main.projPet[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.aiStyle = 0;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 2;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.netImportant = true;

            // Определяем, какой кадр использовать для каждого сегмента
            // Индексы: 0-голова, 1-тело1, 2-тело2, 3-хвост
            // Для 14 сегментов распределяем кадры
            segmentFrames = new int[]
            {
                0,  // Сегмент 0: голова (кадр 0)
                1,  // Сегмент 1: тело (кадр 1)
                1,  // Сегмент 2: тело (кадр 1)
                2,  // Сегмент 3: тело (кадр 2)
                2,  // Сегмент 4: тело (кадр 2)
                1,  // Сегмент 5: тело (кадр 1)
                1,  // Сегмент 6: тело (кадр 1)
                2,  // Сегмент 7: тело (кадр 2)
                2,  // Сегмент 8: тело (кадр 2)
                1,  // Сегмент 9: тело (кадр 1)
                1,  // Сегмент 10: тело (кадр 1)
                2,  // Сегмент 11: тело (кадр 2)
                2,  // Сегмент 12: тело (кадр 2)
                3   // Сегмент 13: хвост (кадр 3)
            };

            // Высота каждого кадра (все кадры одинаковой высоты)
            TextureSizeY1 = new int[]
            {
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
            };

            TextureSizeY2 = new int[]
            {
                24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24, 24
            };

            minVelocity = 9f;
            maxVelocity = 16f;
        }

        public override void CheckActive()
        {
            Player player = Main.player[Projectile.owner];
            player.GetModPlayer<ValhallaPlayer>();

            if (!player.dead && player.HasBuff(ModContent.BuffType<HotEggBuff>()))
            {
                Projectile.timeLeft = 2;
            }

            if (player.position.X < Projectile.position.X)
            {
                Projectile.localAI[1] += 1f;
                if (Projectile.localAI[1] >= 15f)
                {
                    Projectile.localAI[1] = 15f;
                    Projectile.spriteDirection = 1;
                }
            }
            else
            {
                Projectile.localAI[1] -= 1f;
                if (Projectile.localAI[1] <= -15f)
                {
                    Projectile.localAI[1] = -15f;
                    Projectile.spriteDirection = -1;
                }
            }

            if (Projectile.spriteDirection == -1)
            {
                Projectile.rotation += 0.05f;
            }
            else
            {
                Projectile.rotation -= 0.05f;
            }
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    int num = Dust.NewDust(Projectile.oldPos[i], Projectile.width, Projectile.height, 76, 0f, 0f, 100, default(Color), 1f);
                    Main.dust[num].noGravity = true;
                }
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D asset = ModContent.Request<Texture2D>("Synergia/Content/Projectiles/Other/HotEggProj").Value;

            // Высота одного кадра
            int frameHeight = asset.Height / 4; // 4 кадра

            // Центр для каждого сегмента
            Vector2 vector = new Vector2(asset.Width * 0.5f, Projectile.height * 0.5f);

            float[] array = new float[Projectile.oldPos.Length];
            array[0] = Projectile.rotation;

            for (int i = 1; i < Projectile.oldPos.Length; i++)
            {
                Vector2 vector2 = Projectile.oldPos[i - 1];
                Vector2 vector3 = Projectile.oldPos[i];
                array[i] = (float)(Math.Atan2(vector2.Y - vector3.Y, vector2.X - vector3.X) + Math.PI / 2);
            }

            for (int j = Projectile.oldPos.Length - 1; j >= 0; j--)
            {
                Vector2 vector4 = Projectile.oldPos[j] - Main.screenPosition + vector + new Vector2(0f, Projectile.gfxOffY);
                Color alpha = Projectile.GetAlpha(lightColor);

                // Получаем кадр для текущего сегмента
                int frame = segmentFrames[j];
                Rectangle sourceRect = new Rectangle(0, frame * frameHeight, asset.Width, frameHeight);

                Main.spriteBatch.Draw(asset, vector4, sourceRect, alpha, array[j], vector, Projectile.scale,
                    Projectile.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
            }

            return false;
        }
    }
}