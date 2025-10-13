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
    }
}