using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VecMath
{
    public struct Matrix2
    {
        public static Matrix2 Identity { get; } = new Matrix2(1, 0, 0, 1);

        public float m00;
        public float m01;
        public float m10;
        public float m11;

        public float this[int idx1, int idx2]
        {
            get
            {
                switch (idx1)
                {
                    case 0:
                        switch (idx2)
                        {
                            case 0: return m00;
                            case 1: return m01;
                        }
                        break;
                    case 1:
                        switch (idx2)
                        {
                            case 0: return m10;
                            case 1: return m11;
                        }
                        break;
                }
                throw new ArgumentOutOfRangeException(idx1 + ", " + idx2);
            }

            set
            {
                switch (idx1)
                {
                    case 0:
                        switch (idx2)
                        {
                            case 0: m00 = value; return;
                            case 1: m01 = value; return;
                        }
                        break;
                    case 1:
                        switch (idx2)
                        {
                            case 0: m10 = value; return;
                            case 1: m11 = value; return;
                        }
                        break;
                }
                throw new ArgumentOutOfRangeException(idx1 + ", " + idx2);
            }
        }

        public Matrix2(float m00, float m01, float m10, float m11)
        {
            this.m00 = m00;
            this.m01 = m01;
            this.m10 = m10;
            this.m11 = m11;
        }

        public Matrix2(Vector2 x, Vector2 y) : this(x.x, x.y, y.x, y.y) { }

        public static Matrix2 Rotation(float angle)
        {
            float cos = (float)Math.Cos(angle);
            float sin = (float)Math.Sin(angle);
            return new Matrix2(cos, sin, -sin, cos);
        }

        public static Matrix2 Inverse(Matrix2 m)
        {
            float det = m.Det();

            return new Matrix2(m.m11, m.m10, m.m01, m.m00) * (1.0F / det);
        }

        public static Matrix2 Mul(Matrix2 m1, float f) => new Matrix2
        {
            m00 = m1.m00 * f,
            m01 = m1.m00 * f,
            m10 = m1.m10 * f,
            m11 = m1.m10 * f,
        };

        public static Matrix2 Mul(Matrix2 m1, Matrix2 m2) => new Matrix2
        {
            m00 = m1.m00 * m2.m00 + m1.m01 * m2.m10,
            m01 = m1.m00 * m2.m01 + m1.m01 * m2.m11,
            m10 = m1.m10 * m2.m00 + m1.m11 * m2.m10,
            m11 = m1.m10 * m2.m01 + m1.m11 * m2.m11,
        };

        public static Vector2 Transform(Vector2 v, Matrix2 m) => new Vector2
        {
            x = v.x * m.m00 + v.y * m.m01,
            y = v.x * m.m10 + v.y * m.m11,
        };

        public static bool EpsilonEquals(Matrix2 m1, Matrix2 m2, float epsilon)
        {
            float diff;

            for (int i = 0; i < 4; i++)
            {
                diff = m1[i % 4, i / 4] - m2[i % 4, i / 4];
                if ((diff < 0 ? -diff : diff) > epsilon) return false;
            }
            return true;
        }

        public float Det() => m00 * m11 - m01 * m10;

        public override string ToString() => $"[{m00}, {m01}]\n[{m10}, {m11}]";

        public static Matrix2 operator ~(Matrix2 m1) => Inverse(m1);

        public static Matrix2 operator *(Matrix2 m1, double d1) => Mul(m1, (float)d1);
    }
}
