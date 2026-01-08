using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Synergia.Content.Items;
using ValhallaMod.Items.Consumable.Bag;
using System.Collections.Generic;
using Avalon.Items.Placeable.Statue;

namespace Synergia.Content.Projectiles.Other
{
    public class GreenPresentProj : ModProjectile
    {
        private bool landed;
        private bool initialized;
        private int groundTime;

        private readonly List<Vector2> oldPositions = new();

        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.width = 44;
            Projectile.height = 36;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 2500;
        }

        public override void AI()
        {
            Player player = Main.LocalPlayer;

            if (!initialized)
            {
                initialized = true;
                Projectile.Center = new Vector2(
                    Projectile.Center.X,
                    Main.screenPosition.Y - 120f
                );
                Projectile.velocity.Y = 0.5f; // скорость падения
            }

            if (!landed)
            {
                Projectile.velocity.Y += 0.04f; // ускорение
                Projectile.rotation = (float)System.Math.Sin(Main.GameUpdateCount * 0.06f) * 0.25f;
                Projectile.frame = 0;
            }
            else
            {
                groundTime++;

                if (Projectile.frame < 3 && groundTime % 8 == 0)
                    Projectile.frame++;

                float dist = Vector2.Distance(player.Center, Projectile.Center);
                float shake = MathHelper.Clamp(1f - dist / 200f, 0f, 1f);

                Projectile.position += Main.rand.NextVector2Circular(1.2f * shake, 1.2f * shake);

                if (dist < 80f)
                    Explode();
            }

            if (landed)
            {
                oldPositions.Insert(0, Projectile.Center + Main.rand.NextVector2Circular(0.8f, 0.8f));
                if (oldPositions.Count > 6)
                    oldPositions.RemoveAt(oldPositions.Count - 1);
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (!landed)
            {
                landed = true;
                Projectile.velocity = Vector2.Zero;
                Projectile.rotation = 0f;
                groundTime = 0;

                SoundEngine.PlaySound(
                    new SoundStyle("Synergia/Assets/Sounds/SilentPunch")
                    {
                        Volume = 0.35f
                    },
                    Projectile.Center
                );
            }
            return false;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (!landed || oldPositions.Count == 0)
                return true;

            Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Type].Value;

            int frameHeight = texture.Height / Main.projFrames[Type];
            Rectangle frameRect = new Rectangle(
                0,
                Projectile.frame * frameHeight,
                texture.Width,
                frameHeight
            );

            Vector2 origin = frameRect.Size() / 2f;

            for (int i = 0; i < oldPositions.Count; i++)
            {
                float alpha = 0.35f * (1f - i / 5f);

                Vector2 drawPos = oldPositions[i]
                    + Main.rand.NextVector2Circular(0.6f, 0.6f)
                    - Main.screenPosition;

                Main.EntitySpriteDraw(
                    texture,
                    drawPos,
                    frameRect,
                    Projectile.GetAlpha(lightColor) * alpha,
                    Projectile.rotation,
                    origin,
                    Projectile.scale,
                    SpriteEffects.None,
                    0
                );
            }

            return true;
        }

        private void Explode()
        {
            SoundEngine.PlaySound(SoundID.Item14 with
            {
                Volume = 0.45f,
                Pitch = -0.3f
            }, Projectile.Center);

            for (int i = 0; i < 28; i++)
            {
                Dust d = Dust.NewDustDirect(
                    Projectile.position,
                    Projectile.width,
                    Projectile.height,
                    DustID.GreenTorch
                );
                d.velocity *= 2.8f;
                d.noGravity = true;
                d.scale = 1.5f;
            }

            if (Main.myPlayer == Projectile.owner)
            {
                if (Main.expertMode || Main.masterMode)
                {
                    if (Main.rand.NextFloat() < 0.2f)
                        Item.NewItem(Projectile.GetSource_FromThis(), Projectile.getRect(), ModContent.ItemType<GreatGift>());
                    else
                        DropTrash();
                }
                else
                {
                    if (Main.rand.NextFloat() < 0.2f)
                    {
                        int[] good =
                        {
                            ModContent.ItemType<IceSculpture>(),
                            ItemID.IceFeather
                        };

                        Item.NewItem(Projectile.GetSource_FromThis(), Projectile.getRect(), Main.rand.Next(good));
                    }
                    else
                        DropTrash();
                }
            }

            Projectile.Kill();
        }

        private void DropTrash()
        {
            int[] trash =
            {
                ItemID.RottenEgg,
                ItemID.MudBlock,
                ItemID.SlushBlock,
                ItemID.SnowBlock,
                ItemID.FlinxFur,
                ItemID.IceBlock
            };

            Item.NewItem(
                Projectile.GetSource_FromThis(),
                Projectile.getRect(),
                Main.rand.Next(trash)
            );
        }
    }
}
