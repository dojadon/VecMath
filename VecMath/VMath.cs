using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VecMath
{
    public static class VMath
    {
        public static Vector2 Min(Vector2 v1, Vector2 v2) => new Vector2
        {
            x = v1.x < v2.x ? v1.x : v2.x,
            y = v1.y < v2.y ? v1.y : v2.y,
        };

        public static Vector3 Min(Vector3 v1, Vector3 v2) => new Vector3
        {
            x = v1.x < v2.x ? v1.x : v2.x,
            y = v1.y < v2.y ? v1.y : v2.y,
            z = v1.z < v2.z ? v1.z : v2.z,
        };

        public static Vector4 Min(Vector4 v1, Vector4 v2) => new Vector4
        {
            x = v1.x < v2.x ? v1.x : v2.x,
            y = v1.y < v2.y ? v1.y : v2.y,
            z = v1.z < v2.z ? v1.z : v2.z,
            w = v1.w < v2.w ? v1.w : v2.w,
        };

        public static Vector2 Max(Vector2 v1, Vector2 v2) => new Vector2
        {
            x = v1.x > v2.x ? v1.x : v2.x,
            y = v1.y > v2.y ? v1.y : v2.y,
        };

        public static Vector3 Max(Vector3 v1, Vector3 v2) => new Vector3
        {
            x = v1.x > v2.x ? v1.x : v2.x,
            y = v1.y > v2.y ? v1.y : v2.y,
            z = v1.z > v2.z ? v1.z : v2.z,
        };

        public static Vector4 Max(Vector4 v1, Vector4 v2) => new Vector4
        {
            x = v1.x > v2.x ? v1.x : v2.x,
            y = v1.y > v2.y ? v1.y : v2.y,
            z = v1.z > v2.z ? v1.z : v2.z,
            w = v1.w > v2.w ? v1.w : v2.w,
        };

        public static Vector2 Clamp(Vector2 v, float min, float max) => new Vector2
        {
            x = v.x < min ? min : (max < v.x ? max : v.x),
            y = v.y < min ? min : (max < v.y ? max : v.y),
        };

        public static Vector3 Clamp(Vector3 v, float min, float max) => new Vector3
        {
            x = v.x < min ? min : (max < v.x ? max : v.x),
            y = v.y < min ? min : (max < v.y ? max : v.y),
            z = v.z < min ? min : (max < v.z ? max : v.z),
        };

        public static Vector4 Clamp(Vector4 v, float min, float max) => new Vector4
        {
            x = v.x < min ? min : (max < v.x ? max : v.x),
            y = v.y < min ? min : (max < v.y ? max : v.y),
            z = v.z < min ? min : (max < v.z ? max : v.z),
            w = v.w < min ? min : (max < v.w ? max : v.w),
        };

        public static Vector2 Clamp(Vector2 v, Vector2 min, Vector2 max) => new Vector2
        {
            x = v.x < min.x ? min.x : (max.x < v.x ? max.x : v.x),
            y = v.y < min.y ? min.y : (max.y < v.y ? max.y : v.y),
        };

        public static Vector3 Clamp(Vector3 v, Vector3 min, Vector4 max) => new Vector3
        {
            x = v.x < min.x ? min.x : (max.x < v.x ? max.x : v.x),
            y = v.y < min.y ? min.y : (max.y < v.y ? max.y : v.y),
            z = v.z < min.z ? min.z : (max.z < v.z ? max.z : v.z),
        };

        public static Vector4 Clamp(Vector4 v, Vector4 min, Vector4 max) => new Vector4
        {
            x = v.x < min.x ? min.x : (max.x < v.x ? max.x : v.x),
            y = v.y < min.y ? min.y : (max.y < v.y ? max.y : v.y),
            z = v.z < min.z ? min.z : (max.z < v.z ? max.z : v.z),
            w = v.w < min.w ? min.w : (max.w < v.w ? max.w : v.w),
        };
    }
}
