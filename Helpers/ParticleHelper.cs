
using Microsoft.Xna.Framework;
using ParticleLibrary.Core;
using ParticleLibrary.Core.V3;
using ParticleLibrary.Core.V3.Particles;
using ParticleLibrary.Utilities;
using ReLogic.Content;
using Terraria;
using Terraria.ModLoader;
using SysVec2 = System.Numerics.Vector2;

namespace Synergia.Helpers {
    public static class ParticleHelper
    {
        public static void CreateParticle(System.Numerics.Vector2 pos, Color color, int lifeParticle = 120, float scale = 64f, float scale2 = 16f)
        {
            SynergiaParticleSystem.CreateDataParticle(
                pos,
                (Main.rand.NextVector2Unit() * Main.rand.NextFloat(2f, 2f + float.Epsilon)).ToNumerics(),
                Main.GlobalTimeWrappedHourly,
                new Vector2(scale, scale2).ToNumerics(),
                1f,
                new Color(175, 137, 241, 0),
                lifeParticle,
                color
            );
        }

        public static void CreateParticle(System.Numerics.Vector2 pos, Color baseColor, Color otherColor, int lifeParticle = 120, float scale = 64f, float scale2 = 16f)
        {
            SynergiaParticleSystem.CreateDataParticle(
                pos,
                (Main.rand.NextVector2Unit() * Main.rand.NextFloat(2f, 2f + float.Epsilon)).ToNumerics(),
                Main.GlobalTimeWrappedHourly,
                new Vector2(scale, scale2).ToNumerics(),
                1f,
                baseColor,
                lifeParticle,
                otherColor
            );
        }

        public static void CreateParticle(System.Numerics.Vector2 pos, System.Numerics.Vector2 velocity, Color baseColor, Color otherColor, int lifeParticle = 120, float scale = 64f, float scale2 = 16f)
        {
            SynergiaParticleSystem.CreateDataParticle(
                pos,
                velocity,
                Main.GlobalTimeWrappedHourly,
                new Vector2(scale, scale2).ToNumerics(),
                1f,
                baseColor,
                lifeParticle,
                otherColor
            );
        }
    }

    public class SynergiaParticleSystem : ModSystem
    {
        internal static ParticleBuffer<SynergiaParticleBehavior> DataParticleBuffer;

        public override void OnModLoad()
        {
            if (Main.dedServ)
                return;

            DataParticleBuffer = new ParticleBuffer<SynergiaParticleBehavior>(256);

            ParticleManagerV3.RegisterUpdatable(DataParticleBuffer);
            ParticleManagerV3.RegisterRenderable(Layer.BeforeSolidTiles, DataParticleBuffer);
        }

        internal static void CreateDataParticle(
            SysVec2 position,
            SysVec2 velocity,
            float rotation,
            SysVec2 scale,
            float depth,
            Color color,
            int duration,
            Color otherColor)
        {
            if (Main.dedServ)
                return;

            float packed = System.BitConverter.UInt32BitsToSingle(otherColor.PackedValue);

            DataParticleBuffer.Create(
                new ParticleInfo(
                    position,
                    velocity,
                    rotation,
                    scale,
                    depth,
                    color,
                    duration,
                    packed
                )
            );
        }
    }
    public class SynergiaParticleBehavior : Behavior<ParticleInfo>
    {
        public override string Texture =>
            "Synergia/Assets/Textures/Star";
        public override void Update(ref ParticleInfo info)
        {
            info.Position += info.Velocity;
            info.Rotation += 0.05f;

            info.Velocity *= 0.97f;
            info.Scale *= 0.96f;
        }
    }
}
