using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VecMath
{
    [Serializable]
    public struct Quaternion
    {
        public static readonly Quaternion Identity = new Quaternion(0, 0, 0, 1);
        private const double EPS = 1.0e-7F;

        public float x;
        public float y;
        public float z;
        public float w;

        public float Length() => (float)Math.Sqrt(x * x + y * y + z * z + w * w);

        public Quaternion(float x, float y, float z, float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        public Quaternion(Quaternion q1) : this(q1.x, q1.y, q1.z, q1.w) { }

        public Quaternion(Vector3 v1, float w) : this(v1.x, v1.y, v1.z, w) { }

        public Quaternion(Matrix3 m)
        {
            float[] values = new float[4];
            values[0] = m.m00 - m.m11 - m.m22;
            values[1] = -m.m00 + m.m11 - m.m22;
            values[2] = -m.m00 - m.m11 + m.m22;
            values[3] = m.m00 + m.m11 + m.m22;

            int biggestIndex = 0;
            for (int i = 1; i < 4; i++)
            {
                if (values[i] > values[biggestIndex]) biggestIndex = i;
            }

            float val = (float)Math.Sqrt(values[biggestIndex] + 1.0F) * 0.5F;
            float mult = 0.25F / val;

            switch (biggestIndex)
            {
                case 0:
                    x = val;
                    y = (m.m10 + m.m01) * mult;
                    z = (m.m02 + m.m20) * mult;
                    w = (m.m12 - m.m21) * mult;
                    break;

                case 1:
                    x = (m.m10 + m.m01) * mult;
                    y = val;
                    z = (m.m21 + m.m12) * mult;
                    w = (m.m20 - m.m02) * mult;
                    break;

                case 2:
                    x = (m.m02 + m.m20) * mult;
                    y = (m.m21 + m.m12) * mult;
                    z = val;
                    w = (m.m01 - m.m10) * mult;
                    break;

                case 3:
                    x = (m.m12 - m.m21) * mult;
                    y = (m.m20 - m.m02) * mult;
                    z = (m.m01 - m.m10) * mult;
                    w = val;
                    break;

                default:
                    throw new ApplicationException();
            }
        }

        public Quaternion(Matrix4 m) : this((Matrix3)m) { }

        public static Quaternion Conjugate(Quaternion q1) => new Quaternion()
        {
            w = q1.w,
            x = -q1.x,
            y = -q1.y,
            z = -q1.z
        };

        public static Quaternion Inverse(Quaternion q1)
        {
            float norm = 1.0F / (q1.w * q1.w + q1.x * q1.x + q1.y * q1.y + q1.z * q1.z);
            return new Quaternion()
            {
                w = norm * q1.w,
                x = -norm * q1.x,
                y = -norm * q1.y,
                z = -norm * q1.z
            };
        }

        public static Quaternion Add(Quaternion q1, Quaternion q2) => new Quaternion()
        {
            w = q1.w + q2.w,
            x = q1.x + q2.x,
            y = q1.y + q2.y,
            z = q1.z + q2.z,
        };

        public static Quaternion Mul(Quaternion q1, float f2) => new Quaternion()
        {
            w = q1.w * f2,
            x = q1.x * f2,
            y = q1.y * f2,
            z = q1.z * f2,
        };

        public static Quaternion Mul(Quaternion q1, Quaternion q2) => new Quaternion()
        {
            w = q1.w * q2.w - q1.x * q2.x - q1.y * q2.y - q1.z * q2.z,
            x = q1.w * q2.x + q2.w * q1.x - q1.y * q2.z + q1.z * q2.y,
            y = q1.w * q2.y + q2.w * q1.y + q1.x * q2.z - q1.z * q2.x,
            z = q1.w * q2.z + q2.w * q1.z - q1.x * q2.y + q1.y * q2.x
        };

        public static Quaternion Normalize(Quaternion q1)
        {
            float norm = 1.0F / (float)Math.Sqrt(q1.x * q1.x + q1.y * q1.y + q1.z * q1.z + q1.w * q1.w);

            return new Quaternion(norm * q1.x, norm * q1.y, norm * q1.z, norm * q1.w);
        }

        public static Quaternion RotationAxis(Vector3 a, float angle)
        {
            float amag = (float)Math.Sqrt(a.x * a.x + a.y * a.y + a.z * a.z);

            if (amag < EPS)
            {
                return Identity;
            }
            else
            {
                amag = 1.0F / amag;
                float mag = (float)Math.Sin(angle * 0.5);

                return new Quaternion()
                {
                    w = (float)Math.Cos(angle * 0.5),
                    x = a.x * amag * mag,
                    y = a.y * amag * mag,
                    z = a.z * amag * mag
                };
            }
        }

        public static Quaternion Pow(Quaternion q1, float exponent)
        {
            if (Math.Abs(q1.w) > 1 - EPS || exponent < EPS) { return Identity; }

            float angle1 = (float)Math.Acos(q1.w);
            float angle2 = angle1 * exponent;

            float mult = (float)(Math.Sin(angle2) / Math.Sin(angle1));

            return new Quaternion()
            {
                w = (float)Math.Cos(angle2),
                x = q1.x * mult,
                y = q1.y * mult,
                z = q1.z * mult
            };
        }

        public static Quaternion Interpolate(Quaternion q1, Quaternion q2, float alpha)
        {
            float dot, s1, s2, om, sinom;

            dot = Dot(q1, q2);

            if (dot < 0)
            {
                q1.x = -q1.x;
                q1.y = -q1.y;
                q1.z = -q1.z;
                q1.w = -q1.w;
                dot = -dot;
            }

            if (1.0 - dot > EPS)
            {
                om = (float)Math.Acos(dot);
                sinom = (float)Math.Sin(om);
                s1 = (float)Math.Sin((1.0 - alpha) * om) / sinom;
                s2 = (float)Math.Sin(alpha * om) / sinom;
            }
            else
            {
                s1 = 1.0F - alpha;
                s2 = alpha;
            }
            return new Quaternion()
            {
                w = s1 * q1.w + s2 * q2.w,
                x = s1 * q1.x + s2 * q2.x,
                y = s1 * q1.y + s2 * q2.y,
                z = s1 * q1.z + s2 * q2.z
            };
        }

        public static float Dot(Quaternion q1, Quaternion q2) => q2.x * q1.x + q2.y * q1.y + q2.z * q1.z + q2.w * q1.w;

        public static Vector3 RotateVector(Vector3 v1, Quaternion q1)
        {
            if (v1 == Vector3.Zero)
            {
                return Vector3.Zero;
            }
            var q2 = ~q1 * new Quaternion(v1.x, v1.y, v1.z, 0) * q1;

            return new Vector3(q2.x, q2.y, q2.z);
        }

        public static bool IsNaN(Quaternion q) => float.IsNaN(q.x) || float.IsNaN(q.y) || float.IsNaN(q.z) || float.IsNaN(q.w);

        public override bool Equals(object obj)
        {
            if (obj is Quaternion q)
            {
                return Equals(q);
            }
            return false;
        }

        public bool Equals(Quaternion q1) => this == q1;

        public static bool EpsilonEquals(Quaternion q1, Quaternion q2, float epsilon)
        {
            float diff;
            diff = q1.x - q2.x;
            if ((diff < 0 ? -diff : diff) > epsilon) return false;
            diff = q1.y - q2.y;
            if ((diff < 0 ? -diff : diff) > epsilon) return false;
            diff = q1.z - q2.z;
            if ((diff < 0 ? -diff : diff) > epsilon) return false;
            diff = q1.w - q2.w;
            if ((diff < 0 ? -diff : diff) > epsilon) return false;
            return true;
        }

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

        public override string ToString() => $"[{x}, {y}, {z}, {w}]";

        public static bool operator ==(Quaternion q1, Quaternion q2) => q1.x == q2.x && q1.y == q2.y && q1.z == q2.z && q1.w == q2.w;

        public static bool operator !=(Quaternion q1, Quaternion q2) => !(q1.x == q2.x && q1.y == q2.y && q1.z == q2.z && q1.w == q2.w);

        public static Quaternion operator -(Quaternion q1) => Conjugate(q1);

        public static Quaternion operator ~(Quaternion q1) => Inverse(q1);

        public static Quaternion operator +(Quaternion q1, Quaternion q2) => Add(q1, q2);

        public static Quaternion operator *(Quaternion q1, double d2) => Mul(q1, (float)d2);

        public static Quaternion operator *(Quaternion q1, Quaternion q2) => Mul(q1, q2);

        public static Vector3 operator *(Vector3 v1, Quaternion q1) => RotateVector(v1, q1);

        public static Quaternion operator ^(Quaternion q1, double d1) => Pow(q1, (float)d1);

        public static implicit operator Quaternion(Matrix4 m1) => new Quaternion(m1);

        public static implicit operator Quaternion(Matrix3 m1) => new Quaternion(m1);
    }
}
