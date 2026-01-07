using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.Graphics.Effects;

namespace Synergia.GraphicsSetting.SynergiaHeaven;

public class SynergiaSky : CustomSky {
    private bool isActive;

    private float intensity;

    public override void Update(GameTime gameTime) {
        if (isActive && intensity < 0.5f) {
            intensity += 0.01f;
        }
        else if (!isActive && intensity > 0f) {
            intensity -= 0.01f;
        }
    }
    public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth) {
        if (maxDepth >= 0f && minDepth < 0f) {
            spriteBatch.Draw(TextureAssets.BlackTile.Value, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), new Color(0, 50, 100) * intensity);
        }
    }
    public override float GetCloudAlpha() {
        return 0f;
    }
    public override void Activate(Vector2 position, params object[] args) {
        isActive = true;
    }
    public override void Deactivate(params object[] args) {
        isActive = false;
    }
    public override void Reset() {
        isActive = false;
    }
    public override bool IsActive() {
        if (!isActive) {
            return intensity > 0f;
        }
        return true;
    }
}
