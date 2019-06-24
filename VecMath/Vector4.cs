using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VecMath
{
    [Serializable]
    public struct Vector4
    {
        public static readonly Vector4 Zero = new Vector4();
        public static readonly Vector4 UnitX = new Vector4(1, 0, 0, 0);
        public static readonly Vector4 UnitY = new Vector4(0, 1, 0, 0);
        public static readonly Vector4 UnitZ = new Vector4(0, 0, 1, 0);
        public static readonly Vector4 UnitW = new Vector4(0, 0, 1, 1);

        public static readonly Vector4[] Units = { UnitX, UnitY, UnitZ, UnitW };

        public float x;
        public float y;
        public float z;
        public float w;

        public float this[int idx]
        {
            get
            {
                switch (idx)
                {
                    case 0: return x;
                    case 1: return y;
                    case 2: return z;
                    case 3: return w;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            set
            {
                switch (idx)
                {
                    case 0:
                        x = value;
                        break;
                    case 1:
                        y = value;
                        break;
                    case 2:
                        z = value;
                        break;
                    case 3:
                        w = value;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public Vector4(float x, float y, float z, float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        public Vector4(Vector2 v1) : this(v1.x, v1.y, 0, 1) { }

        public Vector4(Vector3 v1) : this(v1.x, v1.y, v1.z, 1) { }

        public Vector4(Vector4 v1) : this(v1.x, v1.y, v1.z, v1.w) { }

        public static Vector4 Add(Vector4 v1, Vector4 v2) => new Vector4(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z, v1.w + v2.w);

        public static Vector4 Sub(Vector4 v1, Vector4 v2) => new Vector4(v1.x - v2.x, v1.y - v2.y, v1.z - v2.z, v1.w - v2.w);

        public static Vector4 Scale(Vector4 v1, float d1) => new Vector4(v1.x * d1, v1.y * d1, v1.z * d1, v1.w * d1);

        public static Vector4 Scale(Vector4 v1, Vector4 v2) => new Vector4(v1.x * v2.x, v1.y * v2.y, v1.z * v2.z, v1.w * v2.w);

        public static Vector4 Pow(Vector4 v1, float f1) => new Vector4((float)Math.Pow(v1.x, f1), (float)Math.Pow(v1.y, f1), (float)Math.Pow(v1.z, f1), (float)Math.Pow(v1.w, f1));

        public static float Dot(Vector4 v1, Vector4 v2) => v2.x * v1.x + v2.y * v1.y + v2.z * v1.z + v1.w * v2.w;

        public static Vector4 Clamp(Vector4 v, Vector4 min, Vector4 max) => new Vector4()
        {
            x = v.x < min.x ? min.x : (max.x < v.x ? max.x : v.x),
            y = v.y < min.y ? min.y : (max.y < v.y ? max.y : v.y),
            z = v.z < min.z ? min.z : (max.z < v.z ? max.z : v.z),
            w = v.w < min.w ? min.w : (max.w < v.w ? max.w : v.w),
        };

        public static Vector4 Interpolate(Vector4 v1, Vector4 v2, float t)
        {
            float dot = Dot(v1, v2);
            float t1, t2;

            if (1.0F - dot > VMath.EPS)
            {
                float angle = (float)Math.Acos(dot);
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

        public static Vector4 Normalize(Vector4 v1)
        {
            float len = v1.Length();

            if (len == 1) { return v1; }
            if (len == 0) { return Zero; }

            return v1 * (1 / len);
        }

        public override bool Equals(object obj)
        {
            if (obj is Quaternion q)
            {
                return Equals(q);
            }
            return false;
        }

        public bool Equals(Vector4 v1) => this == v1;

        public static bool EpsilonEquals(Vector4 v1, Vector4 v2, float epsilon)
        {
            float diff;
            diff = v1.x - v2.x;
            if ((diff < 0 ? -diff : diff) > epsilon) return false;
            diff = v1.y - v2.y;
            if ((diff < 0 ? -diff : diff) > epsilon) return false;
            diff = v1.z - v2.z;
            if ((diff < 0 ? -diff : diff) > epsilon) return false;
            diff = v1.w - v2.w;
            if ((diff < 0 ? -diff : diff) > epsilon) return false;
            return true;
        }

        public float Length() => (float)Math.Sqrt(LengthSquare());

        public float LengthSquare() => x * x + y * y + z * z + w * w;

        public override string ToString() => $"[{x}, {y}, {z}, {w}]";

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = x.GetHashCode();
                hashCode = (hashCode * 397) ^ y.GetHashCode();
                hashCode = (hashCode * 397) ^ z.GetHashCode();
                hashCode = (hashCode * 397) ^ w.GetHashCode();
                return hashCode;
            }
        }

        public static Vector4 operator -(Vector4 v1) => v1 * -1;

        public static bool operator ==(Vector4 v1, Vector4 v2) => v1.x == v2.x && v1.y == v2.y && v1.z == v2.z && v1.w == v2.w;

        public static bool operator !=(Vector4 v1, Vector4 v2) => !(v1.x == v2.x && v1.y == v2.y && v1.z == v2.z && v1.w == v2.w);

        public static Vector4 operator +(Vector4 v1, Vector4 v2) => Add(v1, v2);

        public static Vector4 operator -(Vector4 v1, Vector4 v2) => Sub(v1, v2);

        public static Vector4 operator *(Vector4 v1, double d1) => Scale(v1, (float)d1);

        public static Vector4 operator *(double d1, Vector4 v1) => Scale(v1, (float)d1);
    }
}
