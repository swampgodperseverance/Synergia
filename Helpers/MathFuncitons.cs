using System;

namespace Synergia.Helpers
{
    public static class MathFunctions
    {
        public static double SineWave(double x, double amplitude, double phase, double frequency)
        {
            double w = 2 * Math.PI * frequency;
            return amplitude * Math.Sin(w * x + phase);
        }
         public static float EaseInBackForFlamingSword(float x)
        {
                double c1 = 1.70158;
                float c3 = (float)c1 + 1;

                return (float)(c3 * x * x * x - c1 * x * x);
        }
    }
}