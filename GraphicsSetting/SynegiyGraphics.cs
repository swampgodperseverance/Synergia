using ReLogic.Content;
using Synergia.GraphicsSetting.SynergiaHeaven;
using System;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;

namespace Synergia.GraphicsSetting;

public class SynegiyGraphics {
    public const string ARMORRADAR = nameof(ARMORRADAR);
    public const string COGWORMSHADER = nameof(COGWORMSHADER);
    public const string PRESENTSKY = nameof(PRESENTSKY);
    public const string BASEFILTER = "FilterMiniTower";
    public static Asset<Effect> MyEffect;
    public static string MY;

    public static string FilePath(string name) => $"Synergia/Assets/Effects/{name}";
    static string GetShaderNeme(ref string saveShaderName, string shaderName) {
        saveShaderName = $"{nameof(Synergia)}:{shaderName}";
        return saveShaderName;
    }
    public static void Init(AssetRepository Assets) {
        if (!Main.dedServ) {
            LoadFiltersIntensity(COGWORMSHADER, BASEFILTER, 0.7f, 0.2f, 0.2f, 0.4f, new SynergiaSky()); // 1.0, 0.2, 0.2
            LoadFiltersOpacity(PRESENTSKY, BASEFILTER, 0f, 0f, 0f, 0f, new PresentSky());
            //LoadShader(ref MyEffect, "DistortionShader", ref MY, "DistortPixel");
        }
    }
    [Obsolete("Лучше не трогать!!!!!")]
    static void LoadEffects(AssetRepository Assets, string fileName, string passName, string name) {
        Ref<Effect> screenRef = new(Assets.Request<Effect>(fileName, AssetRequestMode.ImmediateLoad).Value);
        Filters.Scene[name] = new Filter(new ScreenShaderData(screenRef, passName), EffectPriority.VeryHigh);
        Filters.Scene[name].Load();
    }
    static void LoadFiltersIntensity(string name, string passName, float r, float g, float b, float intensity, CustomSky sky, EffectPriority priority = EffectPriority.VeryHigh) {
        Filters.Scene[name] = new Filter(new ScreenShaderData(passName).UseColor(r, g, b).UseIntensity(intensity), priority);
        SkyManager.Instance[name] = sky;
    }
    static void LoadFiltersOpacity(string name, string passName, float r, float g, float b, float intensity, CustomSky sky, EffectPriority priority = EffectPriority.VeryHigh) {
        Filters.Scene[name] = new Filter(new ScreenShaderData(passName).UseColor(r, g, b).UseOpacity(intensity), priority);
        SkyManager.Instance[name] = sky;
    }
    static void LoadImages(string name, string passName, string texture, float intensity, EffectPriority priority) {
        Filters.Scene[name] = new Filter(new ScreenShaderData(passName).UseImage(texture, 0, null).UseIntensity(intensity), priority);
    }
    static void LoadShader(ref Asset<Effect> shader, string shaderName, ref string saveShaderName, string pass) {
        shader = ModContent.Request<Effect>(FilePath(shaderName), AssetRequestMode.ImmediateLoad);
        GameShaders.Misc.Add(GetShaderNeme(ref saveShaderName, pass), new MiscShaderData(shader, pass));
    }
}