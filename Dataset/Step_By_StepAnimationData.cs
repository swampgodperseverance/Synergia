using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace Synergia.Dataset;

public struct Step_By_StepAnimationData {
    public SpriteFrame Frame;

    public void Init(byte columns, byte rows) {
        if (Frame.ColumnCount == 0 || Frame.RowCount == 0) {
            Frame = new SpriteFrame(columns, rows) {
                CurrentColumn = 0,
                CurrentRow = 0
            };
        }
    }
    public Rectangle GetSource(Texture2D texture) => Frame.GetSourceRectangle(texture);
    public void Update(byte currentFrame) {
        Frame.CurrentRow = currentFrame;
    }
}
