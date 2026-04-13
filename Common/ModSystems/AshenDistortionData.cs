using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;

namespace Synergia.Common.ModSystems
{
    public class AshenDistortionData : ScreenShaderData
    {
        private float intensity = 0f;
        private float targetIntensity = 0f;

        public AshenDistortionData(Ref<Effect> shader, string passName) : base(shader, passName) { }

        public override void Update(GameTime gameTime)
        {
            intensity = MathHelper.Lerp(intensity, targetIntensity, 0.1f);
            Shader.Parameters["uTime"]?.SetValue((float)Main.GameUpdateCount * 0.05f);
            Shader.Parameters["uIntensity"]?.SetValue(intensity);
        }

        public override void Apply()
        {
            Shader.Parameters["uTime"]?.SetValue((float)Main.GameUpdateCount * 0.05f);
            Shader.Parameters["uIntensity"]?.SetValue(intensity);
            base.Apply();
        }

        public void SetIntensity(float value)
        {
            targetIntensity = MathHelper.Clamp(value, 0f, 0.18f);
        }
    }
    
        public class ShaderLoader : ModSystem
        {
            public static Effect AshenDistortionEffect;

            public override void Load()
            {
                if (Main.dedServ) return;

                AshenDistortionEffect = ModContent.Request<Effect>(
                    "Synergia/Assets/Effects/AshenDistortion",
                    ReLogic.Content.AssetRequestMode.ImmediateLoad
                ).Value;
            }

            public override void Unload()
            {
                AshenDistortionEffect = null;
            }
        }
    
}