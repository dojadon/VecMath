using System;
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

        private const float EPS = 1.0e-7F;

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

        public static Vector3 Pow(Vector3 v1, float f1) => new Vector3((float)Math.Pow(v1.x, f1), (float)Math.Pow(v1.y, f1), (float)Math.Pow(v1.z, f1));

        public static float Dot(Vector3 v1, Vector3 v2) => v2.x * v1.x + v2.y * v1.y + v2.z * v1.z;

        public static Vector3 Clamp(Vector3 v, Vector3 min, Vector3 max) => new Vector3()
        {
            x = v.x < min.x ? min.x : (max.x < v.x ? max.x : v.x),
            y = v.y < min.y ? min.y : (max.y < v.y ? max.y : v.y),
            z = v.z < min.z ? min.z : (max.z < v.z ? max.z : v.z),
        };

        public static Vector3 Cross(Vector3 v1, Vector3 v2) => new Vector3()
        {
            x = v1.y * v2.z - v1.z * v2.y,
            y = v1.z * v2.x - v1.x * v2.z,
            z = v1.x * v2.y - v1.y * v2.x
        };

        public static Vector3 Interpolate(Vector3 v1, Vector3 v2, float t)
        {
            float dot = v1 * v2;
            float t1, t2;

            if (1.0F - dot > EPS)
            {
                float angle = (float)Math.Acos(v1 * v2);
                float sin = (float)Math.Sin(angle);
                t1 = (float)Math.Sin(angle * (1 - t)) / sin;
                t2 = (float)Math.Sin(angle * t) / sin;
            }
            else
            {
                t1 = t;
                t2 = 1 - t;
            }

            return t1 * v1 + t2 * v2;
        }

        public static Vector3 RotationEularMatrixZXY(Matrix3 m)
        {
            Vector3 eular = Zero;
            double sinX = m.m21;

            eular.x = (float)Math.Asin(sinX);

            if (-1 < sinX && sinX < 1)
            {
                double cosX = Math.Cos(eular.x);

                double cosY = m.m22 / cosX;
                double sinY = m.m21 / -cosX;
                eular.y = (float)Math.Atan2(sinY, cosY);

                double cosZ = m.m11 / cosX;
                double sinZ = m.m01 / -cosX;
                eular.z = (float)Math.Atan2(sinZ, cosZ);
            }
            else
            {
                eular.z = (float)Math.Atan2(m.m10, m.m00);
            }
            return eular;
        }

        public static Vector3 RotationEularMatrixXZY(Matrix3 m)
        {
            Vector3 eular = Zero;

            double sinZ = -m.m01;
            eular.z = (float)Math.Asin(sinZ);

            if (-1 < sinZ && sinZ < 1)
            {
                double cosZ = Math.Cos(eular.z);

                double sinY = m.m02 / cosZ;
                double cosY = m.m00 / cosZ;
                eular.y = (float)Math.Atan2(sinY, cosY);

                double sinX = m.m21 / cosZ;
                double cosX = m.m11 / cosZ;
                eular.x = (float)Math.Atan2(sinX, cosX);
            }
            else
            {
                double sinX = -m.m20;
                double cosX = -m.m10;
                eular.x = (float)Math.Atan2(sinX, cosX);
            }

            return eular;
        }

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

        public bool IsNaN() => float.IsNaN(x) || float.IsNaN(y) || float.IsNaN(z);

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

        public static Vector3 operator ^(Vector3 v1, double d1) => Pow(v1, (float)d1);

        public static float operator *(Vector3 v1, Vector3 v2) => Dot(v1, v2);

        public static Vector3 operator ^(Vector3 v1, Vector3 v2) => Cross(v1, v2);

        public static explicit operator Vector4(Vector3 v1) => new Vector4(v1.x, v1.y, v1.z, 1);

        public static implicit operator Vector3(Vector4 v1) => new Vector3(v1.x, v1.y, v1.z);

        public static explicit operator DxMath.Vector3(Vector3 v1) => new DxMath.Vector3(v1.x, v1.y, v1.z);

        public static implicit operator Vector3(DxMath.Vector3 v1) => new Vector3(v1.X, v1.Y, v1.Z);

        public static explicit operator float[] (Vector3 v1) => new float[] { v1.x, v1.y, v1.z };

        public static implicit operator Vector3(float[] f) => new Vector3(f[0], f[1], f[2]);
    }
}
