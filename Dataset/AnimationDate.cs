using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;

namespace Synergia.Dataset {
    struct AnimationDate {
        public SpriteFrame Frame;
        public bool StartAnimation;
        public int FrameTimer;
        public SpriteEffects Effects;
        public const int FrameDuration = 7;
        public byte RowCount;

        public void Init(byte columns, byte rows) {
            if (Frame.ColumnCount == 0 || Frame.RowCount == 0) {
                Frame = new SpriteFrame(columns, rows) {
                    CurrentColumn = 0,
                    CurrentRow = 0
                };
            }
        }
        public Rectangle GetSource(Texture2D texture) => Frame.GetSourceRectangle(texture);
        public void Update() {
            if (StartAnimation) {
                FrameTimer++;
                if (FrameTimer >= FrameDuration) {
                    FrameTimer = 0;
                    Frame.CurrentRow = (byte)((Frame.CurrentRow + 1) % Frame.RowCount);
                    if (Frame.CurrentRow == Frame.RowCount - RowCount) {
                        StartAnimation = false;
                        Frame.CurrentRow = 0;
                    }
                }
            }
        }
        public void Reset() {
            StartAnimation = false;
            Frame.CurrentRow = 0;
            Frame.CurrentColumn = 0;
            FrameTimer = 0;
        }
    }
}