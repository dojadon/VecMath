using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VecMath
{
    public struct Ray
    {
        public Vector3 pos;
        public Vector3 vec;

        public Ray(Vector3 pos, Vector3 vec)
        {
            this.pos = pos;
            this.vec = vec;
        }
    }

    [Serializable]
    public struct Vector3
    {
        public static readonly Vector3 Zero = new Vector3();
        public static readonly Vector3 UnitX = new Vector3(1, 0, 0);
        public static readonly Vector3 UnitY = new Vector3(0, 1, 0);
        public static readonly Vector3 UnitZ = new Vector3(0, 0, 1);

        public static readonly Vector3[] Units = { UnitX, UnitY, UnitZ };

        public float x;
        public float y;
        public float z;

        public Vector2 xy
        {
            get => new Vector2(x, y);
            set
            {
                x = value.x;
                y = value.y;
            }
        }

        public Vector2 xz
        {
            get => new Vector2(x, z);
            set
            {
                x = value.x;
                z = value.y;
            }
        }

        public Vector2 yz
        {
            get => new Vector2(y, z);
            set
            {
                y = value.x;
                z = value.y;
            }
        }

        public float this[int idx]
        {
            get
            {
                switch (idx)
                {
                    case 0: return x;
                    case 1: return y;
                    case 2: return z;
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
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public Vector3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Vector3(Vector2 v1) : this(v1.x, v1.y, 0) { }

        public Vector3(Vector3 v1) : this(v1.x, v1.y, v1.z) { }

        public Vector3(Vector4 v1) : this(v1.x, v1.y, v1.z) { }

        public Vector3(IEnumerable<float> e) : this(e.First(), e.First(), e.First()) { }

        public static Vector3 Add(Vector3 v1, Vector3 v2) => new Vector3(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z);

        public static Vector3 Sub(Vector3 v1, Vector3 v2) => new Vector3(v1.x - v2.x, v1.y - v2.y, v1.z - v2.z);

        public static Vector3 Scale(Vector3 v1, float d1) => new Vector3(v1.x * d1, v1.y * d1, v1.z * d1);

        public static Vector3 Scale(Vector3 v1, float x, float y, float z) => new Vector3(v1.x * x, v1.y * y, v1.z * z);

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

        public static Vector3 Normalize(Vector3 v1)
        {
            float len = v1.Length();

            if (len == 1) { return v1; }
            if (len == 0) { return Zero; }

            return v1 * (1 / len);
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

        public float Length() => (float)Math.Sqrt(LengthSquare());

        public float LengthSquare() => x * x + y * y + z * z;

        public override string ToString() => $"[{x}, {y}, {z}]";

        public override int GetHashCode() => x.GetHashCode() ^ y.GetHashCode() << 2 ^ z.GetHashCode() >> 2;

        public static Vector3 operator -(Vector3 v1) => v1 * -1;

        public static bool operator ==(Vector3 v1, Vector3 v2) => v1.x == v2.x && v1.y == v2.y && v1.z == v2.z;
        public static bool operator !=(Vector3 v1, Vector3 v2) => !(v1.x == v2.x && v1.y == v2.y && v1.z == v2.z);

        public static bool operator <(Vector3 v1, Vector3 v2) => v1.x < v2.x && v1.y < v2.y && v1.z < v2.z;
        public static bool operator >(Vector3 v1, Vector3 v2) => v1.x > v2.x && v1.y > v2.y && v1.z > v2.z;

        public static bool operator <=(Vector3 v1, Vector3 v2) => v1.x <= v2.x && v1.y <= v2.y && v1.z <= v2.z;
        public static bool operator >=(Vector3 v1, Vector3 v2) => v1.x >= v2.x && v1.y >= v2.y && v1.z >= v2.z;

        public static Vector3 operator +(Vector3 v1, Vector3 v2) => Add(v1, v2);

        public static Vector3 operator -(Vector3 v1, Vector3 v2) => Sub(v1, v2);

        public static Vector3 operator *(Vector3 v1, double d1) => Scale(v1, (float)d1);

        public static Vector3 operator *(double d1, Vector3 v1) => Scale(v1, (float)d1);

        public static Vector3 operator /(Vector3 v1, double d1) => Scale(v1, 1 / (float)d1);

        public static explicit operator Vector4(Vector3 v1) => new Vector4(v1.x, v1.y, v1.z, 1);

        public static implicit operator Vector3(Vector4 v1) => new Vector3(v1.x, v1.y, v1.z);
    }
}
