using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VecMath
{
    public static class MathUtil
    {
        public const float EPS = 1.0e-7F;

        public static float Clamp(float value, float min, float max) => value < min ? min : (max < value ? max : value);
        public static Vector4 Clamp(Vector4 v, Vector4 min, Vector4 max) => Vector4.Clamp(v, min, max);
        public static Vector3 Clamp(Vector3 v, Vector3 min, Vector3 max) => Vector3.Clamp(v, min, max);
        public static Vector2 Clamp(Vector2 v, Vector2 min, Vector2 max) => Vector2.Clamp(v, min, max);

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

        public static Quaternion Interpolate(Quaternion v1, Quaternion v2, float t) => Quaternion.Interpolate(v1, v2, t);
        public static Vector4 Interpolate(Vector4 v1, Vector4 v2, float t) => Vector4.Interpolate(v1, v2, t);
        public static Vector3 Interpolate(Vector3 v1, Vector3 v2, float t) => Vector3.Interpolate(v1, v2, t);
        public static Vector2 Interpolate(Vector2 v1, Vector2 v2, float t) => Vector2.Interpolate(v1, v2, t);

        public static float Dot(Quaternion v1, Quaternion v2) => Quaternion.Dot(v1, v2);
        public static float Dot(Vector4 v1, Vector4 v2) => Vector4.Dot(v1, v2);
        public static float Dot(Vector3 v1, Vector3 v2) => Vector3.Dot(v1, v2);
        public static float Dot(Vector2 v1, Vector2 v2) => Vector2.Dot(v1, v2);

        public static Vector3 Cross(Vector3 v1, Vector3 v2) => Vector3.Cross(v1, v2);

        public static Quaternion Normalize(Quaternion v) => Quaternion.Normalize(v);
        public static Vector4 Normalize(Vector4 v) => Vector4.Normalize(v);
        public static Vector3 Normalize(Vector3 v) => Vector3.Normalize(v);
        public static Vector2 Normalize(Vector2 v) => Vector2.Normalize(v);
    }
}
