namespace Dxx.Util
{
    using System;
    using UnityEngine;

    public class MathDxx
    {
        public static int Abs(int value) => 
            Mathf.Abs(value);

        public static long Abs(long value)
        {
            if (value > 0L)
            {
                return value;
            }
            return -value;
        }

        public static float Abs(float value) => 
            Mathf.Abs(value);

        public static int CeilBig(float value) => 
            (GetSymbol(value) * Mathf.CeilToInt(Mathf.Abs(value)));

        public static int CeilToInt(float value) => 
            Mathf.CeilToInt(value);

        public static int Clamp(int value, int min, int max) => 
            Mathf.Clamp(value, min, max);

        public static long Clamp(long value, long min, long max)
        {
            if (value < min)
            {
                value = min;
                return value;
            }
            if (value > max)
            {
                value = max;
            }
            return value;
        }

        public static float Clamp(float value, float min, float max) => 
            Mathf.Clamp(value, min, max);

        public static float Clamp01(float value) => 
            Mathf.Clamp01(value);

        public static float Cos(float angle) => 
            Mathf.Cos((angle * 3.141593f) / 180f);

        public static int FloorToInt(float value) => 
            Mathf.FloorToInt(value);

        public static int GetSymbol(int value) => 
            ((value <= 0) ? -1 : 1);

        public static int GetSymbol(long value) => 
            ((value <= 0L) ? -1 : 1);

        public static int GetSymbol(float value) => 
            ((value <= 0f) ? -1 : 1);

        public static string GetSymbolString(long value) => 
            ((value < 0L) ? "-" : "+");

        public static float MoveTowardsAngle(float current, float target, float maxDelta) => 
            Mathf.MoveTowardsAngle(current, target, maxDelta);

        public static float Pow(float f, float p) => 
            Mathf.Pow(f, p);

        public static bool RandomBool() => 
            (GameLogic.Random(0, 2) == 0);

        public static int RandomSymbol() => 
            ((GameLogic.Random(0, 2) != 0) ? -1 : 1);

        public static int RoundToInt(float value) => 
            Mathf.RoundToInt(value);

        public static float Sin(float angle) => 
            Mathf.Sin((angle * 3.141593f) / 180f);
    }
}

