using Microsoft.Xna.Framework;
using ParticleLibrary.Examples.V3;
using ParticleLibrary.Utilities;
using Terraria;

namespace Synergia.Helpers {
    public static class ParticleHelper {
        // базовый функции для партиклов!
        public static void CreateParticle(System.Numerics.Vector2 pos, Color color, int lifeParticle = 120, float scale = 64f, float scale2 = 16f) {
            ExampleParticleSystemManagerV3.CreateDataParticle(pos, (Main.rand.NextVector2Unit() * Main.rand.NextFloat(2f, 2f + float.Epsilon)).ToNumerics(), Main.GlobalTimeWrappedHourly, new Vector2(scale, scale2).ToNumerics(), 1f, new(175, 137, 241, 0), lifeParticle, color);
        }
        public static void CreateParticle(System.Numerics.Vector2 pos, Color baseColor, Color otherColor, int lifeParticle = 120, float scale = 64f, float scale2 = 16f) {
            ExampleParticleSystemManagerV3.CreateDataParticle(pos, (Main.rand.NextVector2Unit() * Main.rand.NextFloat(2f, 2f + float.Epsilon)).ToNumerics(), Main.GlobalTimeWrappedHourly, new Vector2(scale, scale2).ToNumerics(), 1f, baseColor, lifeParticle, otherColor);
        }
        public static void CreateParticle(System.Numerics.Vector2 pos, System.Numerics.Vector2 velocity, Color baseColor, Color otherColor, int lifeParticle = 120, float scale = 64f, float scale2 = 16f) {
            ExampleParticleSystemManagerV3.CreateDataParticle(pos, velocity, Main.GlobalTimeWrappedHourly, new Vector2(scale, scale2).ToNumerics(), 1f, baseColor, lifeParticle, otherColor);
        }
        // Создано нейросетью для партиклов в одну сторону 
        public static void CreateParticleInOnePos(System.Numerics.Vector2 pos, Color color) {
            var dir = new Vector2(1f, Main.rand.NextFloat(-0.3f, 0.3f));
            dir = Vector2.Normalize(dir);

            var vel = (dir * Main.rand.NextFloat(2f, 3f)).ToNumerics();

            ExampleParticleSystemManagerV3.CreateDataParticle(pos, vel, Main.GlobalTimeWrappedHourly, new Vector2(64f, 16f).ToNumerics(), 1f, new Color(175, 137, 241, 0), 120, color);
        }
        // Создано нейросетью для партиклов в разный стороны 
        public static void CreateSideParticle(Rectangle hitbox, Color color) {
            // выбираем сторону
            int side = Main.rand.Next(4);

            Vector2 pos;
            Vector2 dir;

            switch (side) {
                // верх
                case 0: pos = new Vector2(hitbox.Center.X, hitbox.Top); dir = new Vector2(0f, -1f); break;
                // низ
                case 1: pos = new Vector2(hitbox.Center.X, hitbox.Bottom); dir = new Vector2(0f, 1f); break;
                // лево
                case 2: pos = new Vector2(hitbox.Left, hitbox.Center.Y); dir = new Vector2(-1f, 0f); break;
                // право
                default: pos = new Vector2(hitbox.Right, hitbox.Center.Y); dir = new Vector2(1f, 0f); break;
            }

            // небольшой рандом вдоль стороны
            pos += Main.rand.NextVector2Circular(6f, 6f);

            // скорость от центра наружу
            var vel = (dir + Main.rand.NextVector2Circular(0.2f, 0.2f)).SafeNormalize(Vector2.Zero);
            vel *= Main.rand.NextFloat(2f, 3f);

            ExampleParticleSystemManagerV3.CreateDataParticle(pos.ToNumerics(), vel.ToNumerics(), Main.GlobalTimeWrappedHourly, new Vector2(64f, 16f).ToNumerics(), 1f, new Color(175, 137, 241, 0), 120, color);
        }
    }
}