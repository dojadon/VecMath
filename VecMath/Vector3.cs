﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VecMath
{
    [Serializable]
    public struct Vector3
    {
        public static readonly Vector3 Zero = new Vector3();
        public static readonly Vector3 UnitX = new Vector3(1, 0, 0);
        public static readonly Vector3 UnitY = new Vector3(0, 1, 0);
        public static readonly Vector3 UnitZ = new Vector3(0, 0, 1);

        public float x;
        public float y;
        public float z;

        public Vector3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Vector3(Vector3 v1) : this(v1.x, v1.y, v1.z)
        {

        }

        public static Vector3 Add(Vector3 v1, Vector3 v2) => new Vector3(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z);

        public static Vector3 Sub(Vector3 v1, Vector3 v2) => new Vector3(v1.x - v2.x, v1.y - v2.y, v1.z - v2.z);

        public static Vector3 Scale(Vector3 v1, float d1) => new Vector3(v1.x * d1, v1.y * d1, v1.z * d1);

        public static Vector3 Scale(Vector3 v1, Vector3 v2) => new Vector3(v1.x * v2.x, v1.y * v2.y, v1.z * v2.z);

        public static float Dot(Vector3 v1, Vector3 v2) => v2.x * v1.x + v2.y * v1.y + v2.z * v1.z;

        public static Vector3 Cross(Vector3 v1, Vector3 v2) => new Vector3()
        {
            x = v1.y * v2.z - v1.z * v2.y,
            y = v1.z * v2.x - v1.x * v2.z,
            z = v1.x * v2.y - v1.y * v2.x
        };

        public static Vector3 Normalize(Vector3 v1)
        {
            float len = v1.Length();

            if (len == 1) { return v1; }
            if (len == 0) { return Zero; }

            float mult = 1 / len;

            return new Vector3()
            {
                x = v1.x * mult,
                y = v1.y * mult,
                z = v1.z * mult
            };
        }

        public override bool Equals(object obj)
        {
            if (obj is Vector3 v)
            {
                return Equals(v);
            }
            return false;
        }

        public bool Equals(Vector3 v1) => this == v1;

        public static bool EpsilonEquals(Vector3 v1, Vector3 v2, float epsilon)
        {
            float diff;
            diff = v1.x - v2.x;
            if ((diff < 0 ? -diff : diff) > epsilon) return false;
            diff = v1.y - v2.y;
            if ((diff < 0 ? -diff : diff) > epsilon) return false;
            diff = v1.z - v2.z;
            if ((diff < 0 ? -diff : diff) > epsilon) return false;
            return true;
        }

        public float Length() => (float)Math.Sqrt(x * x + y * y + z * z);

        public override string ToString() => $"[{x}, {y}, {z}]";

        public override int GetHashCode() => x.GetHashCode() ^ y.GetHashCode() << 2 ^ z.GetHashCode() >> 2;

        public static Vector3 operator -(Vector3 v1) => v1 * -1;

        public static Vector3 operator +(Vector3 v1) => Normalize(v1);

        public static bool operator ==(Vector3 v1, Vector3 v2) => v1.x == v2.x && v1.y == v2.y && v1.z == v2.z;

        public static bool operator !=(Vector3 v1, Vector3 v2) => !(v1.x == v2.x && v1.y == v2.y && v1.z == v2.z);

        public static Vector3 operator +(Vector3 v1, Vector3 v2) => Add(v1, v2);

        public static Vector3 operator -(Vector3 v1, Vector3 v2) => Sub(v1, v2);

        public static Vector3 operator *(Vector3 v1, double d1) => Scale(v1, (float)d1);

        public static Vector3 operator *(double d1, Vector3 v1) => Scale(v1, (float)d1);

        public static float operator *(Vector3 v1, Vector3 v2) => Dot(v1, v2);

        public static Vector3 operator ^(Vector3 v1, Vector3 v2) => Cross(v1, v2);

        public static explicit operator Vector4(Vector3 v1) => new Vector4(v1.x, v1.y, v1.z, 1);

        public static implicit operator Vector3(Vector4 v1) => new Vector3(v1.x, v1.y, v1.z);

        public static explicit operator DxMath.Vector3(Vector3 v1) => new DxMath.Vector3(v1.x, v1.y, v1.z);

        public static explicit operator OpenTK.Vector3(Vector3 v1) => new OpenTK.Vector3(v1.x, v1.y, v1.z);

        public static implicit operator Vector3(DxMath.Vector3 v1) => new Vector3(v1.X, v1.Y, v1.Z);

        public static implicit operator Vector3(OpenTK.Vector3 v1) => new Vector3(v1.X, v1.Y, v1.Z);

        public static explicit operator float[] (Vector3 v1) => new float[] { v1.x, v1.y, v1.z };

        public static implicit operator Vector3(float[] f) => new Vector3(f[0], f[1], f[2]);
    }
}
