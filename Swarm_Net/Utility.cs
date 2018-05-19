using System;

namespace Swarm_Net
{
    static class Utility
    {
        private static Random random = new Random();

        public static double NextDouble()
        {
            return random.NextDouble();
        }

        public static int Next()
        {
            return random.Next();
        }

        public static int Next(int maxValue)
        {
            return random.Next(maxValue);
        }

        public static int Next(int minValue, int maxValue)
        {
            return random.Next(minValue,maxValue);
        }
    }
}