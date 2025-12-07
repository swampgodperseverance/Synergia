using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;

namespace Synergia.GraphicsSetting;

public class SynegiyGraphics {
    public const string COGWORMSHADER = nameof(COGWORMSHADER);
    public const string BASEFILTER = "FilterMiniTower";

    static string FilePath(string name) => $"Assets/Effects/{name}";
    public static void Init(AssetRepository Assets) {
        if (!Main.dedServ) {
            LoadFilters(COGWORMSHADER, BASEFILTER, 0.7f, 0.2f, 0.2f, 0.4f, EffectPriority.VeryHigh, new SynergiaSky()); // 1.0, 0.2, 0.2
        }
    }
    static void LoadEffects(AssetRepository Assets, string fileName, string passName, string name) {
        Ref<Effect> screenRef = new(Assets.Request<Effect>(fileName, AssetRequestMode.ImmediateLoad).Value);
        Filters.Scene[name] = new Filter(new ScreenShaderData(screenRef, passName), EffectPriority.VeryHigh);
        Filters.Scene[name].Load();
    }
    static void LoadFilters(string name, string passName, float r, float g, float b, float intensity, EffectPriority priority, CustomSky sky) {
        Filters.Scene[name] = new Filter(new ScreenShaderData(passName).UseColor(r, g, b).UseIntensity(intensity), priority);
        SkyManager.Instance[name] = sky;
    }
    static void LoadImages(string name, string passName, string texture, float intensity, EffectPriority priority) {
        Filters.Scene[name] = new Filter(new ScreenShaderData(passName).UseImage(texture, 0, null).UseIntensity(intensity), priority);
    }
}