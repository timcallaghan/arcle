using System;

namespace Arbaureal.Arcle.Utilities
{
    public static class MathHelpers
    {
        public static double DegreesToRadians(double degrees)
        {
            double radians = ((degrees / 360) * 2 * Math.PI);
            return radians;
        }
    }
}
