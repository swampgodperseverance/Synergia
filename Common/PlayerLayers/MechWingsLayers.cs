// Code by 𝒜𝑒𝓇𝒾𝓈
using Terraria;
using Terraria.DataStructures;

namespace Synergia.Common.PlayerLayers {
    public class MechWingsLayers : PlayerDrawLayer {
        public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.Wings);
        public override bool GetDefaultVisibility(PlayerDrawSet drawInfo) => drawInfo.drawPlayer.wings == EquipLoader.GetEquipSlot(Mod, "PostMechWings", EquipType.Wings);
        protected override void Draw(ref PlayerDrawSet drawInfo) {
            Player drawPlayer = drawInfo.drawPlayer;

            if (drawPlayer.dead)
                return;

            Vector2 Position = drawInfo.Position;
            Vector2 pos = new((int)(Position.X - Main.screenPosition.X + (drawPlayer.width / 2)), (int)(Position.Y - Main.screenPosition.Y + (drawPlayer.height / 2) - 2f * drawPlayer.gravDir));
            Color lightColor = Lighting.GetColor((int)drawPlayer.Center.X / 16, (int)drawPlayer.Center.Y / 16, Color.White);
            Color color = lightColor * (1 - drawInfo.shadow);

            Vector2 posLeft = new(pos.X - 35, pos.Y - 23);
            Vector2 posLeft1 = new(pos.X - 45, pos.Y - 3);
            Vector2 posLeft2 = new(pos.X - 28, pos.Y + 9);

            Vector2 posRight = new(pos.X + 17, pos.Y - 19);
            Vector2 posRight1 = new(pos.X + 17, pos.Y - 3);
            Vector2 posRight2 = new(pos.X + 17, pos.Y + 11);

            if (drawPlayer.direction == -1) {
                posLeft = new Vector2(pos.X + 17, pos.Y - 22);
                posRight = new Vector2(pos.X - 31, pos.Y - 18);

                posLeft1 = new Vector2(pos.X + 17, pos.Y - 3);
                posRight1 = new Vector2(pos.X - 31.05f, pos.Y-1.5f);

                posLeft2 = new Vector2(pos.X + 17, pos.Y + 11);
                posRight2 = new Vector2(pos.X - 26, pos.Y + 12);
            }

            Vector2[] left = [posLeft, posLeft1, posLeft2];
            Vector2[] right = [posRight, posRight1, posRight2];

            for (int i = 0; i < 3; i++) {
                DrawFeatherTrail(drawInfo, RTextures.FeatherRight[i].Value, right[i], i, color, drawInfo.playerEffect);
                DrawFeatherTrail(drawInfo, RTextures.FeatherLeft [i].Value, left[i],  i, color, drawInfo.playerEffect);
            }
        }
        static void DrawFeatherTrail(PlayerDrawSet drawInfo, Texture2D tex, Vector2 basePos, int i, Color color, SpriteEffects effects) {
            Player drawPlayer = drawInfo.drawPlayer;

            float segments = 4f;
            int index = i % (int)segments;

            Vector2 vel = drawPlayer.velocity * -1.5f;
            Vector2 orbit = new Vector2(0f, 0.5f).RotatedBy((drawPlayer.miscCounterNormalized * (2f + index) + index * 0.5f) * MathHelper.TwoPi) * (index + 1);

            Vector2 pos = basePos;
            pos += orbit;
            pos += vel * (index / segments);

            Vector2 origin = tex.Size() / 2f;
            pos += origin;

            DrawData data = new(tex, pos.Floor(), null, color, drawPlayer.bodyRotation, origin, 1f, effects) {
                shader = drawPlayer.cWings
            };
            drawInfo.DrawDataCache.Add(data);
        }
    }
}