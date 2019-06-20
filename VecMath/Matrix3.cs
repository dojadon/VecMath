using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VecMath
{
    [Serializable]
    public struct Matrix3
    {
        public static readonly Matrix3 Identity = new Matrix3(Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ);

        public float m00;
        public float m01;
        public float m02;
        public float m10;
        public float m11;
        public float m12;
        public float m20;
        public float m21;
        public float m22;

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
                            case 2: return m02;
                        }
                        break;
                    case 1:
                        switch (idx2)
                        {
                            case 0: return m10;
                            case 1: return m11;
                            case 2: return m12;
                        }
                        break;
                    case 2:
                        switch (idx2)
                        {
                            case 0: return m20;
                            case 1: return m21;
                            case 2: return m22;
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
                            case 2: m02 = value; return;
                        }
                        break;
                    case 1:
                        switch (idx2)
                        {
                            case 0: m10 = value; return;
                            case 1: m11 = value; return;
                            case 2: m12 = value; return;
                        }
                        break;
                    case 2:
                        switch (idx2)
                        {
                            case 0: m20 = value; return;
                            case 1: m21 = value; return;
                            case 2: m22 = value; return;
                        }
                        break;
                }
                throw new ArgumentOutOfRangeException(idx1 + ", " + idx2);
            }
        }

        public Matrix3(float m00, float m01, float m02, float m10, float m11, float m12, float m20, float m21, float m22)
        {
            this.m00 = m00;
            this.m01 = m01;
            this.m02 = m02;

            this.m10 = m10;
            this.m11 = m11;
            this.m12 = m12;

            this.m20 = m20;
            this.m21 = m21;
            this.m22 = m22;
        }

        public Matrix3(Vector3 x, Vector3 y, Vector3 z) : this(x.x, x.y, x.z, y.x, y.y, y.z, z.x, z.y, z.z) { }

        public Matrix3(float m00, float m11, float m22) : this(m00, 0, 0, 0, m11, 0, 0, 0, m22) { }

        public Matrix3(Vector3 v) : this(v.x, 0, 0, 0, v.y, 0, 0, 0, v.z) { }

        public Matrix3(float f) : this(f, 0, 0, 0, f, 0, 0, 0, f) { }

        public Matrix3(Quaternion q)
        {
            float xx = q.x * q.x;
            float yy = q.y * q.y;
            float zz = q.z * q.z;

            m00 = 1 - 2 * (yy + zz);
            m01 = 2 * (q.x * q.y + q.w * q.z);
            m02 = 2 * (q.x * q.z - q.w * q.y);

            m10 = 2 * (q.x * q.y - q.w * q.z);
            m11 = 1 - 2 * (zz + xx);
            m12 = 2 * (q.y * q.z + q.w * q.x);

            m20 = 2 * (q.x * q.z + q.w * q.y);
            m21 = 2 * (q.y * q.z - q.w * q.x);
            m22 = 1 - 2 * (xx + yy);
        }

        public Matrix3(Matrix4 m) : this(m.m00, m.m01, m.m02, m.m10, m.m11, m.m12, m.m20, m.m21, m.m22) { }

        public static Matrix3 RotationAxis(Vector3 axis, float angle, bool normalize = true)
        {
            if (normalize)
            {
                axis = MathUtil.Normalize(axis);
            }

            float cos = (float)Math.Cos(angle);
            float sin = (float)Math.Sin(angle);

            var x = Vector3.UnitX;
            var n = axis.x * axis;
            x = cos * (x - n) + sin * MathUtil.Cross(axis, x) + n;

            var y = Vector3.UnitY;
            n = axis.y * axis;
            y = cos * (y - n) + sin * MathUtil.Cross(axis, y) + n;

            var z = Vector3.UnitZ;
            n = axis.z * axis;
            z = cos * (z - n) + sin * MathUtil.Cross(axis, z) + n;

            return new Matrix3(x, y, z);
        }

        public static Matrix3 RotationX(float angle)
        {
            float sin = (float)Math.Sin(angle);
            float cos = (float)Math.Cos(angle);

            return new Matrix3(1, 0, 0, 0, cos, -sin, 0, sin, cos);
        }

        public static Matrix3 RotationY(float angle)
        {
            float sin = (float)Math.Sin(angle);
            float cos = (float)Math.Cos(angle);

            return new Matrix3(cos, 0, sin, 0, 1, 0, -sin, 0, cos);
        }

        public static Matrix3 RotationZ(float angle)
        {
            float sin = (float)Math.Sin(angle);
            float cos = (float)Math.Cos(angle);

            return new Matrix3(cos, -sin, 0, sin, cos, 0, 0, 0, 1);
        }

        public static Matrix3 LookAt(Vector3 forward, Vector3 upward)
        {
            if (forward == upward || forward == -upward) return Identity;

            var z = -MathUtil.Normalize(forward);
            var x = MathUtil.Normalize(MathUtil.Cross(upward, z));
            var y = MathUtil.Normalize(MathUtil.Cross(z, x));

            return new Matrix3(x, y, z);
        }

        public static Matrix3 Mul(Matrix3 m1, Matrix3 m2) => new Matrix3()
        {
            m00 = m1.m00 * m2.m00 + m1.m01 * m2.m10 + m1.m02 * m2.m20,
            m01 = m1.m00 * m2.m01 + m1.m01 * m2.m11 + m1.m02 * m2.m21,
            m02 = m1.m00 * m2.m02 + m1.m01 * m2.m12 + m1.m02 * m2.m22,

            m10 = m1.m10 * m2.m00 + m1.m11 * m2.m10 + m1.m12 * m2.m20,
            m11 = m1.m10 * m2.m01 + m1.m11 * m2.m11 + m1.m12 * m2.m21,
            m12 = m1.m10 * m2.m02 + m1.m11 * m2.m12 + m1.m12 * m2.m22,

            m20 = m1.m20 * m2.m00 + m1.m21 * m2.m10 + m1.m22 * m2.m20,
            m21 = m1.m20 * m2.m01 + m1.m21 * m2.m11 + m1.m22 * m2.m21,
            m22 = m1.m20 * m2.m02 + m1.m21 * m2.m12 + m1.m22 * m2.m22,
        };

        public static Matrix3 Mul(Matrix3 m, float f) => new Matrix3()
        {
            m00 = m.m00 * f,
            m01 = m.m01 * f,
            m02 = m.m02 * f,

            m10 = m.m10 * f,
            m11 = m.m11 * f,
            m12 = m.m12 * f,

            m20 = m.m20 * f,
            m21 = m.m21 * f,
            m22 = m.m22 * f,
        };

        public static Matrix3 Inverse(Matrix3 m)
        {
            float det = m.Det();

            if (det == 0)
            {
                throw new ArithmeticException("Determinant is 0");
            }

            return new Matrix3()
            {
                m00 = m.m11 * m.m22 - m.m12 * m.m21,
                m01 = -m.m01 * m.m22 + m.m02 * m.m21,
                m02 = m.m01 * m.m12 - m.m02 * m.m11,
                m10 = -m.m10 * m.m22 + m.m12 * m.m20,
                m11 = m.m00 * m.m22 - m.m02 * m.m20,
                m12 = -m.m00 * m.m12 + -m.m02 * m.m10,
                m20 = m.m10 * m.m21 - m.m11 * m.m20,
                m21 = -m.m00 * m.m21 + m.m01 * m.m20,
                m22 = m.m00 * m.m11 - m.m01 * m.m10,
            } * (1.0F / det);
        }

        public static Matrix3 Transpose(Matrix3 m1) => new Matrix4()
        {
            m00 = m1.m00,
            m01 = m1.m10,
            m02 = m1.m20,
            m10 = m1.m01,
            m11 = m1.m11,
            m12 = m1.m21,
            m20 = m1.m02,
            m21 = m1.m12,
            m22 = m1.m22,
        };

        public static Vector3 Transform(Vector3 v1, Matrix3 m1) => new Vector3()
        {
            x = v1.x * m1.m00 + v1.y * m1.m10 + v1.z * m1.m20,
            y = v1.x * m1.m01 + v1.y * m1.m11 + v1.z * m1.m21,
            z = v1.x * m1.m02 + v1.y * m1.m12 + v1.z * m1.m22
        };

        public Matrix2 SubMatrix(int row, int coulm)
        {
            var result = Matrix2.Identity;

            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    result[i, j] = this[i < row ? i : i + 1, j < coulm ? j : j + 1];
                }
            }
            return result;
        }

        public float[] CalcEigenvalues()
        {
            float a2 = -(m00 + m11 + m22);
            float a1 = SubMatrix(0, 0).Det() + SubMatrix(1, 1).Det() + SubMatrix(2, 2).Det();
            float a0 = -Det();

            double[] solution = EquationUtil.SolveCubic(1, a2, a1, a0);
            return solution.Select(d=>(float)d).ToArray();
        }

        public static bool IsNaN(Matrix3 m) =>
            float.IsNaN(m.m00) || float.IsNaN(m.m01) || float.IsNaN(m.m02) ||
            float.IsNaN(m.m10) || float.IsNaN(m.m11) || float.IsNaN(m.m12) ||
            float.IsNaN(m.m20) || float.IsNaN(m.m21) || float.IsNaN(m.m22);

        public static bool EpsilonEquals(Matrix3 m1, Matrix3 m2, float epsilon)
        {
            for (int i = 0; i < 9; i++)
            {
                float diff = m1[i / 3, i % 3] - m2[i / 3, i % 3];
                if ((diff < 0 ? -diff : diff) > epsilon) return false;
            }
            return true;
        }

        public float Det() => m00 * m11 * m22 + m01 * m12 * m21 + m02 * m10 * m21 - m02 * m11 * m20 - m01 * m10 * m22 - m00 * m12 * m21;

        public override string ToString() => $"[{m00}, {m01}, {m02}]\n[{m10}, {m11}, {m12}]\n[{m20}, {m21}, {m22}]";

        public static Matrix3 operator ~(Matrix3 m1) => Inverse(m1);

        public static Matrix3 operator *(Matrix3 m1, double d1) => Mul(m1, (float)d1);

        public static Vector3 operator *(Vector3 v1, Matrix3 m1) => Transform(v1, m1);

        public static Matrix3 operator *(Matrix3 m1, Matrix3 m2) => Mul(m1, m2);

        public static implicit operator Matrix3(Quaternion q1) => new Matrix3(q1);

        public static implicit operator Matrix3(Matrix4 m) => new Matrix3(m);
    }
}
