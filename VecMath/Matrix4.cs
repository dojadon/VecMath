using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VecMath
{
    [Serializable]
    public struct Matrix4
    {
        public static readonly Matrix4 Identity = new Matrix4(Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ, Vector3.Zero);

        public float m00;
        public float m01;
        public float m02;
        public float m03;
        public float m10;
        public float m11;
        public float m12;
        public float m13;
        public float m20;
        public float m21;
        public float m22;
        public float m23;
        public float m30;
        public float m31;
        public float m32;
        public float m33;

        public Matrix3 Rotation
        {
            get => this;
            set
            {
                m00 = value.m00;
                m01 = value.m01;
                m02 = value.m02;

                m10 = value.m10;
                m11 = value.m11;
                m12 = value.m12;

                m20 = value.m20;
                m21 = value.m21;
                m22 = value.m22;
            }
        }

        public Vector3 Translation
        {
            get => new Vector3(m30, m31, m32);
            set
            {
                m30 = value.x;
                m31 = value.y;
                m32 = value.z;
            }
        }

        public Matrix4(float m00, float m01, float m02, float m03, float m10, float m11, float m12, float m13, float m20, float m21,
            float m22, float m23, float m30, float m31, float m32, float m33)
        {
            this.m00 = m00;
            this.m01 = m01;
            this.m02 = m02;
            this.m03 = m03;

            this.m10 = m10;
            this.m11 = m11;
            this.m12 = m12;
            this.m13 = m13;

            this.m20 = m20;
            this.m21 = m21;
            this.m22 = m22;
            this.m23 = m23;

            this.m30 = m30;
            this.m31 = m31;
            this.m32 = m32;
            this.m33 = m33;
        }

        public Matrix4(Vector3 x, Vector3 y, Vector3 z, Vector3 trans)
        : this(x.x, y.x, z.x, 0, x.y, y.y, z.y, 0, x.z, y.z, z.z, 0, trans.x, trans.y, trans.z, 1) { }

        public Matrix4(Vector3 x, Vector3 y, Vector3 z) : this(x, y, z, Vector3.Zero) { }

        public Matrix4(Matrix3 m, Vector3 trans)
        : this(m.m00, m.m01, m.m02, 0, m.m10, m.m11, m.m12, 0, m.m20, m.m21, m.m22, 0, trans.x, trans.y, trans.z, 1) { }

        public Matrix4(Matrix3 m) : this(m, Vector3.Zero) { }

        public Matrix4(Quaternion q, Vector3 trans) : this(new Matrix3(q), trans) { }

        public Matrix4(Quaternion q) : this(q, Vector3.Zero) { }

        public static Matrix4 RotationAxis(Vector3 axis, float angle) => RotationAxis(axis, angle, Vector3.Zero);

        public static Matrix4 RotationAxis(Vector3 axis, float angle, Vector3 trans)
        {
            float cos = (float)Math.Cos(angle);
            float sin = (float)Math.Sin(angle);

            var x = Vector3.UnitX;
            var n = axis.x * axis;
            x = cos * (x - n) + sin * (axis ^ x) + n;

            var y = Vector3.UnitY;
            n = axis.y * axis;
            y = cos * (y - n) + sin * (axis ^ y) + n;

            var z = Vector3.UnitZ;
            n = axis.z * axis;
            z = cos * (z - n) + sin * (axis ^ z) + n;

            return new Matrix4(x, y, z, trans);
        }

        public static Matrix4 LookAt(Vector3 center, Vector3 eye, Vector3 upward)
        {
            var z = +(eye - center);
            var x = +(upward ^ z);
            var y = +(z ^ x);

            return new Matrix4(x, y, z, new Vector3(-x * eye, -y * eye, -z * eye));
        }

        public static Matrix4 Perspective(float width, float height, float zNear, float zFar, float fovy)
        {
            float aspect = width / height;

            float top = (float)Math.Tan(fovy) * zNear;
            float bottom = -top;
            float left = bottom * aspect;
            float right = top * aspect;

            return Frustum(left, right, bottom, top, zNear, zFar);
        }

        public static Matrix4 Frustum(float left, float right, float bottom, float top, float near, float far) => new Matrix4
        {
            m00 = 2 * near / (right - left),
            m02 = (right + left) / (right - left),
            m11 = 2 * near / (top - bottom),
            m12 = (top + bottom) / (top - bottom),
            m22 = -(far + near) / (far - near),
            m23 = -2 * far * near / (far - near),
            m32 = -1,
        };

        public static Matrix4 Mul(Matrix4 m1, Matrix4 m2) => new Matrix4()
        {
            m00 = m1.m00 * m2.m00 + m1.m01 * m2.m10 + m1.m02 * m2.m20 + m1.m03 * m2.m30,
            m01 = m1.m00 * m2.m01 + m1.m01 * m2.m11 + m1.m02 * m2.m21 + m1.m03 * m2.m31,
            m02 = m1.m00 * m2.m02 + m1.m01 * m2.m12 + m1.m02 * m2.m22 + m1.m03 * m2.m32,
            m03 = m1.m00 * m2.m03 + m1.m01 * m2.m13 + m1.m02 * m2.m23 + m1.m03 * m2.m33,

            m10 = m1.m10 * m2.m00 + m1.m11 * m2.m10 + m1.m12 * m2.m20 + m1.m13 * m2.m30,
            m11 = m1.m10 * m2.m01 + m1.m11 * m2.m11 + m1.m12 * m2.m21 + m1.m13 * m2.m31,
            m12 = m1.m10 * m2.m02 + m1.m11 * m2.m12 + m1.m12 * m2.m22 + m1.m13 * m2.m32,
            m13 = m1.m10 * m2.m03 + m1.m11 * m2.m13 + m1.m12 * m2.m23 + m1.m13 * m2.m33,

            m20 = m1.m20 * m2.m00 + m1.m21 * m2.m10 + m1.m22 * m2.m20 + m1.m23 * m2.m30,
            m21 = m1.m20 * m2.m01 + m1.m21 * m2.m11 + m1.m22 * m2.m21 + m1.m23 * m2.m31,
            m22 = m1.m20 * m2.m02 + m1.m21 * m2.m12 + m1.m22 * m2.m22 + m1.m23 * m2.m32,
            m23 = m1.m20 * m2.m03 + m1.m21 * m2.m13 + m1.m22 * m2.m23 + m1.m23 * m2.m33,

            m30 = m1.m30 * m2.m00 + m1.m31 * m2.m10 + m1.m32 * m2.m20 + m1.m33 * m2.m30,
            m31 = m1.m30 * m2.m01 + m1.m31 * m2.m11 + m1.m32 * m2.m21 + m1.m33 * m2.m31,
            m32 = m1.m30 * m2.m02 + m1.m31 * m2.m12 + m1.m32 * m2.m22 + m1.m33 * m2.m32,
            m33 = m1.m30 * m2.m03 + m1.m31 * m2.m13 + m1.m32 * m2.m23 + m1.m33 * m2.m33
        };

        public static Matrix4 InverseOrthonormal(Matrix4 m1)
        {
            var inv = ~m1.Rotation;
            return new Matrix4(inv, -m1.Translation * inv);
        }

        public static Matrix4 Inverse(Matrix4 m1)
        {
            float[][] array = (float[][])m1;
            float[][] invArray = (float[][])Identity;

            float buf;

            for (int i = 0; i < 4; i++)
            {
                buf = 1 / array[i][i];
                for (int j = 0; j < 4; j++)
                {
                    array[i][j] *= buf;
                    invArray[i][j] *= buf;
                }
                for (int j = 0; j < 4; j++)
                {
                    if (i != j)
                    {
                        buf = array[j][i];
                        for (int k = 0; k < 4; k++)
                        {
                            array[j][k] -= array[i][k] * buf;
                            invArray[j][k] -= invArray[i][k] * buf;
                        }
                    }
                }
            }
            return invArray;
        }

        public static Matrix4 Transpose(Matrix4 m1) => new Matrix4()
        {
            m00 = m1.m00,
            m01 = m1.m10,
            m02 = m1.m20,
            m03 = m1.m30,

            m10 = m1.m01,
            m11 = m1.m11,
            m12 = m1.m21,
            m13 = m1.m31,

            m20 = m1.m02,
            m21 = m1.m12,
            m22 = m1.m22,
            m23 = m1.m32,

            m30 = m1.m03,
            m31 = m1.m13,
            m32 = m1.m23,
            m33 = m1.m33
        };

        public static Vector4 Transform(Vector4 v1, Matrix4 m1) => new Vector4()
        {
            x = v1.x * m1.m00 + v1.y * m1.m10 + v1.z * m1.m20 + v1.w * m1.m30,
            y = v1.x * m1.m01 + v1.y * m1.m11 + v1.z * m1.m21 + v1.w * m1.m31,
            z = v1.x * m1.m02 + v1.y * m1.m12 + v1.z * m1.m22 + v1.w * m1.m32,
            w = v1.x * m1.m03 + v1.y * m1.m13 + v1.z * m1.m23 + v1.w * m1.m33
        };

        public static Vector3 Transform(Vector3 v1, Matrix4 m1) => new Vector3()
        {
            x = v1.x * m1.m00 + v1.y * m1.m10 + v1.z * m1.m20,
            y = v1.x * m1.m01 + v1.y * m1.m11 + v1.z * m1.m21,
            z = v1.x * m1.m02 + v1.y * m1.m12 + v1.z * m1.m22
        };

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.Append("[");
            sb.Append(m00).Append(", ").Append(m01).Append(", ").Append(m02).Append(", ").Append(m03);
            sb.Append("]\n[");
            sb.Append(m10).Append(", ").Append(m11).Append(", ").Append(m12).Append(", ").Append(m13);
            sb.Append("]\n[");
            sb.Append(m20).Append(", ").Append(m21).Append(", ").Append(m22).Append(", ").Append(m23);
            sb.Append("]\n[");
            sb.Append(m30).Append(", ").Append(m31).Append(", ").Append(m32).Append(", ").Append(m33);
            sb.Append("]");

            return sb.ToString();
        }

        public static bool EpsilonEquals(Matrix4 m1, Matrix4 m2, float epsilon)
        {
            float diff;
            float[] f1 = (float[])m1;
            float[] f2 = (float[])m2;

            for (int i = 0; i < 16; i++)
            {
                diff = f1[i] - f2[i];
                if ((diff < 0 ? -diff : diff) > epsilon) return false;
            }
            return true;
        }

        public override bool Equals(object obj) => obj is Matrix4 m ? Equals(m) : false;

        public bool Equals(Matrix4 m)
        {
            return
            m00 == m.m00 && m10 == m.m10 && m20 == m.m20 && m30 == m.m30 &&
            m01 == m.m01 && m11 == m.m11 && m21 == m.m21 && m31 == m.m31 &&
            m02 == m.m02 && m12 == m.m12 && m22 == m.m22 && m32 == m.m32 &&
            m03 == m.m03 && m13 == m.m13 && m23 == m.m23 && m33 == m.m33;
        }

        public override int GetHashCode()
        {
            long hashCode = 0;
            hashCode = (hashCode * 31) + m00.GetHashCode();
            hashCode = (hashCode * 31) + m01.GetHashCode();
            hashCode = (hashCode * 31) + m02.GetHashCode();
            hashCode = (hashCode * 31) + m03.GetHashCode();

            hashCode = (hashCode * 31) + m10.GetHashCode();
            hashCode = (hashCode * 31) + m11.GetHashCode();
            hashCode = (hashCode * 31) + m12.GetHashCode();
            hashCode = (hashCode * 31) + m13.GetHashCode();

            hashCode = (hashCode * 31) + m20.GetHashCode();
            hashCode = (hashCode * 31) + m21.GetHashCode();
            hashCode = (hashCode * 31) + m22.GetHashCode();
            hashCode = (hashCode * 31) + m23.GetHashCode();

            hashCode = (hashCode * 31) + m30.GetHashCode();
            hashCode = (hashCode * 31) + m31.GetHashCode();
            hashCode = (hashCode * 31) + m32.GetHashCode();
            hashCode = (hashCode * 31) + m33.GetHashCode();
            return hashCode.GetHashCode();
        }

        public static bool operator ==(Matrix4 m1, Matrix4 m2)=>
            m1.m00 == m2.m00 && m1.m10 == m2.m10 && m1.m20 == m2.m20 && m1.m30 == m2.m30 &&
            m1.m01 == m2.m01 && m1.m11 == m2.m11 && m1.m21 == m2.m21 && m1.m31 == m2.m31 &&
            m1.m02 == m2.m02 && m1.m12 == m2.m12 && m1.m22 == m2.m22 && m1.m32 == m2.m32 &&
            m1.m03 == m2.m03 && m1.m13 == m2.m13 && m1.m23 == m2.m23 && m1.m33 == m2.m33;

        public static bool operator !=(Matrix4 m1, Matrix4 m2) => !(
            m1.m00 == m2.m00 && m1.m10 == m2.m10 && m1.m20 == m2.m20 && m1.m30 == m2.m30 &&
            m1.m01 == m2.m01 && m1.m11 == m2.m11 && m1.m21 == m2.m21 && m1.m31 == m2.m31 &&
            m1.m02 == m2.m02 && m1.m12 == m2.m12 && m1.m22 == m2.m22 && m1.m32 == m2.m32 &&
            m1.m03 == m2.m03 && m1.m13 == m2.m13 && m1.m23 == m2.m23 && m1.m33 == m2.m33);

        public static Matrix4 operator ~(Matrix4 m1) => Inverse(m1);

        public static Matrix4 operator *(Matrix4 m1, Matrix4 m2) => Mul(m1, m2);

        public static Vector4 operator *(Vector4 v1, Matrix4 m1) => Transform(v1, m1);

        public static Vector3 operator *(Vector3 v1, Matrix4 m1) => Transform(v1, m1);

        public static implicit operator Matrix4(Matrix3 m) => new Matrix4(m);

        public static implicit operator Matrix4(Quaternion q1) => new Matrix4(q1);

        public static explicit operator float[] (Matrix4 m) => new[]
        {
        m.m00, m.m01, m.m02, m.m03,
        m.m10, m.m11, m.m12, m.m13,
        m.m20, m.m21, m.m22, m.m23,
        m.m30, m.m31, m.m32, m.m33
        };

        public static implicit operator Matrix4(float[] src)
        {
            int index = 0;
            return new Matrix4()
            {
                m00 = src[index++],
                m01 = src[index++],
                m02 = src[index++],
                m03 = src[index++],

                m10 = src[index++],
                m11 = src[index++],
                m12 = src[index++],
                m13 = src[index++],

                m20 = src[index++],
                m21 = src[index++],
                m22 = src[index++],
                m23 = src[index++],

                m30 = src[index++],
                m31 = src[index++],
                m32 = src[index++],
                m33 = src[index++]
            };
        }

        public static explicit operator float[][] (Matrix4 m) => new[]
        {
        new []{ m.m00, m.m01, m.m02, m.m03 },
        new []{ m.m10, m.m11, m.m12, m.m13 },
        new []{ m.m20, m.m21, m.m22, m.m23 },
        new []{ m.m30, m.m31, m.m32, m.m33 }
        };

        public static implicit operator Matrix4(float[][] src)
        {
            int index1 = 0, index2 = 0;
            return new Matrix4()
            {
                m00 = src[index1][index2++],
                m01 = src[index1][index2++],
                m02 = src[index1][index2++],
                m03 = src[index1++][index2++],

                m10 = src[index1][(index2 = 1) - 1],
                m11 = src[index1][index2++],
                m12 = src[index1][index2++],
                m13 = src[index1++][index2++],

                m20 = src[index1][(index2 = 1) - 1],
                m21 = src[index1][index2++],
                m22 = src[index1][index2++],
                m23 = src[index1++][index2++],

                m30 = src[index1][(index2 = 1) - 1],
                m31 = src[index1][index2++],
                m32 = src[index1][index2++],
                m33 = src[index1++][index2++],
            };
        }
    }
}
