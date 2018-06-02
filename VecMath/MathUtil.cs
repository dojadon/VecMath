using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VecMath
{
    public static class MathUtil
    {
        public const float EPS = 1.0e-7F;

        public static float Clamp(float value, float min, float max)
        {
            return value < min ? min : (max < value ? max : value);
        }

        public static bool WithIn(Vector3 value, Vector3 min, Vector3 max)
        {
            return min <= value && value <= max;
        }

        public static Vector3 Min(Vector3 v1, Vector3 v2)
        {
            return new Vector3(Math.Min(v1.x, v2.x), Math.Min(v1.y, v2.y), Math.Min(v1.z, v2.z));
        }

        public static Vector3 Max(Vector3 v1, Vector3 v2)
        {
            return new Vector3(Math.Max(v1.x, v2.x), Math.Max(v1.y, v2.y), Math.Max(v1.z, v2.z));
        }

        public static Quaternion Interpolate(Quaternion q1, Quaternion q2, float t)
        {
            return Quaternion.Interpolate(q1, q2, t);
        }

        public static Vector3 Interpolate(Vector3 v1, Vector3 v2, float t)
        {
            return Vector3.Interpolate(v1, v2, t);
        }

        public static Vector2 Interpolate(Vector2 v1, Vector2 v2, float t)
        {
            return Vector2.Interpolate(v1, v2, t);
        }
    }
}
