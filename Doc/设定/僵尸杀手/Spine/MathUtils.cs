namespace Spine
{
    using System;

    public static class MathUtils
    {
        public const float PI = 3.141593f;
        public const float PI2 = 6.283185f;
        public const float RadDeg = 57.29578f;
        public const float DegRad = 0.01745329f;
        private const int SIN_BITS = 14;
        private const int SIN_MASK = 0x3fff;
        private const int SIN_COUNT = 0x4000;
        private const float RadFull = 6.283185f;
        private const float DegFull = 360f;
        private const float RadToIndex = 2607.594f;
        private const float DegToIndex = 45.51111f;
        private static float[] sin = new float[0x4000];

        static MathUtils()
        {
            for (int i = 0; i < 0x4000; i++)
            {
                sin[i] = (float) Math.Sin((double) (((i + 0.5f) / 16384f) * 6.283185f));
            }
            for (int j = 0; j < 360; j += 90)
            {
                sin[((int) (j * 45.51111f)) & 0x3fff] = (float) Math.Sin((double) (j * 0.01745329f));
            }
        }

        public static float Atan2(float y, float x)
        {
            float num;
            if (x == 0f)
            {
                if (y > 0f)
                {
                    return 1.570796f;
                }
                if (y == 0f)
                {
                    return 0f;
                }
                return -1.570796f;
            }
            float num2 = y / x;
            if (Math.Abs(num2) < 1f)
            {
                num = num2 / (1f + ((0.28f * num2) * num2));
                if (x < 0f)
                {
                    return (num + ((y >= 0f) ? 3.141593f : -3.141593f));
                }
                return num;
            }
            num = 1.570796f - (num2 / ((num2 * num2) + 0.28f));
            return ((y >= 0f) ? num : (num - 3.141593f));
        }

        public static float Clamp(float value, float min, float max)
        {
            if (value < min)
            {
                return min;
            }
            if (value > max)
            {
                return max;
            }
            return value;
        }

        public static float Cos(float radians) => 
            sin[((int) ((radians + 1.570796f) * 2607.594f)) & 0x3fff];

        public static float CosDeg(float degrees) => 
            sin[((int) ((degrees + 90f) * 45.51111f)) & 0x3fff];

        public static float Sin(float radians) => 
            sin[((int) (radians * 2607.594f)) & 0x3fff];

        public static float SinDeg(float degrees) => 
            sin[((int) (degrees * 45.51111f)) & 0x3fff];
    }
}

