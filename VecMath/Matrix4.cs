﻿using System;
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

        public static Matrix4 SetTranslation(Matrix4 m1, Vector3 trans)
        {
            m1.Translation = trans;
            return m1;
        }

        public static Matrix4 SetRotation(Matrix4 m1, Matrix3 m2)
        {
            m1.Rotation = m2;
            return m1;
        }

        public static Matrix4 RotationAxis(Vector3 axis, float angle, Vector3? trans = null)
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

            return new Matrix4(x, y, z, trans ?? Vector3.Zero);
        }

        public static Matrix4 Perspective(float fovy, float aspect, float zNear, float zFar)
        {
            float yMax = zNear * (float)System.Math.Tan(0.5f * fovy);
            float yMin = -yMax;
            float xMin = yMin * aspect;
            float xMax = yMax * aspect;

            return PerspectiveOffCenter(xMin, xMax, yMin, yMax, zNear, zFar);
        }

        public static Matrix4 PerspectiveOffCenter(float left, float right, float bottom, float top, float zNear, float zFar)
        {
            if (zNear <= 0)
                throw new ArgumentOutOfRangeException("zNear");
            if (zFar <= 0)
                throw new ArgumentOutOfRangeException("zFar");
            if (zNear >= zFar)
                throw new ArgumentOutOfRangeException("zNear");

            float x = (2.0f * zNear) / (right - left);
            float y = (2.0f * zNear) / (top - bottom);
            float a = (right + left) / (right - left);
            float b = (top + bottom) / (top - bottom);
            float c = -(zFar + zNear) / (zFar - zNear);
            float d = -(2.0f * zFar * zNear) / (zFar - zNear);

            return new Matrix4()
            {
                m00 = x,
                m01 = 0,
                m02 = 0,
                m03 = 0,

                m10 = 0,
                m11 = y,
                m12 = 0,
                m13 = 0,

                m20 = a,
                m21 = b,
                m22 = c,
                m23 = -1,

                m30 = x,
                m31 = 0,
                m32 = d,
                m33 = 0,
            };
        }

        public static Matrix4 LookAt(Vector3 forward, Vector3 upward, Vector3? trans = null)
        {
            var z = -forward;
            var x = +(upward ^ z);
            var y = +(z ^ x);

            return new Matrix4(x, y, z, trans ?? Vector3.Zero);
        }

        public static Matrix4 LookAtCenter(Vector3 center, Vector3 eye, Vector3 upward)
        {
            var z = +(eye - center);
            var x = +(upward ^ z);
            var y = +(z ^ x);

            var mat = new Matrix3(x, y, z);

            return SetTranslation(mat, eye * mat);
        }

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

        public static Matrix4 Inverse(Matrix4 m1)
        {
            var rot = ~m1.Rotation;
            return SetTranslation(rot, -m1.Translation * rot);
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

        public static Matrix4 Leap(Matrix4 m1, Matrix4 m2, float weight) => new Matrix4()
        {
            m00 = m1.m00 * weight + m2.m00 * (1 - weight),
            m01 = m1.m01 * weight + m2.m01 * (1 - weight),
            m02 = m1.m02 * weight + m2.m02 * (1 - weight),
            m03 = m1.m03 * weight + m2.m03 * (1 - weight),

            m10 = m1.m10 * weight + m2.m10 * (1 - weight),
            m11 = m1.m11 * weight + m2.m11 * (1 - weight),
            m12 = m1.m12 * weight + m2.m12 * (1 - weight),
            m13 = m1.m13 * weight + m2.m13 * (1 - weight),

            m20 = m1.m20 * weight + m2.m20 * (1 - weight),
            m21 = m1.m21 * weight + m2.m21 * (1 - weight),
            m22 = m1.m22 * weight + m2.m22 * (1 - weight),
            m23 = m1.m23 * weight + m2.m23 * (1 - weight),

            m30 = m1.m30 * weight + m2.m30 * (1 - weight),
            m31 = m1.m31 * weight + m2.m31 * (1 - weight),
            m32 = m1.m32 * weight + m2.m32 * (1 - weight),
            m33 = m1.m33 * weight + m2.m33 * (1 - weight),
        };

        public static Matrix4 Leap(Matrix4 m1, Matrix4 m2, Matrix4 m3, Matrix4 m4, float[] weight) => new Matrix4()
        {
            m00 = m1.m00 * weight[0] + m2.m00 * weight[1] + m3.m00 * weight[2] + m4.m00 * weight[3],
            m01 = m1.m01 * weight[0] + m2.m01 * weight[1] + m3.m01 * weight[2] + m4.m01 * weight[3],
            m02 = m1.m02 * weight[0] + m2.m02 * weight[1] + m3.m02 * weight[2] + m4.m02 * weight[3],
            m03 = m1.m03 * weight[0] + m2.m03 * weight[1] + m3.m03 * weight[2] + m4.m03 * weight[3],

            m10 = m1.m10 * weight[0] + m2.m10 * weight[1] + m3.m10 * weight[2] + m4.m10 * weight[3],
            m11 = m1.m11 * weight[0] + m2.m11 * weight[1] + m3.m11 * weight[2] + m4.m11 * weight[3],
            m12 = m1.m12 * weight[0] + m2.m12 * weight[1] + m3.m12 * weight[2] + m4.m12 * weight[3],
            m13 = m1.m13 * weight[0] + m2.m13 * weight[1] + m3.m13 * weight[2] + m4.m13 * weight[3],

            m20 = m1.m20 * weight[0] + m2.m20 * weight[1] + m3.m20 * weight[2] + m4.m20 * weight[3],
            m21 = m1.m21 * weight[0] + m2.m21 * weight[1] + m3.m21 * weight[2] + m4.m21 * weight[3],
            m22 = m1.m22 * weight[0] + m2.m22 * weight[1] + m3.m22 * weight[2] + m4.m22 * weight[3],
            m23 = m1.m23 * weight[0] + m2.m23 * weight[1] + m3.m23 * weight[2] + m4.m23 * weight[3],

            m30 = m1.m30 * weight[0] + m2.m30 * weight[1] + m3.m30 * weight[2] + m4.m30 * weight[3],
            m31 = m1.m31 * weight[0] + m2.m31 * weight[1] + m3.m31 * weight[2] + m4.m31 * weight[3],
            m32 = m1.m32 * weight[0] + m2.m32 * weight[1] + m3.m32 * weight[2] + m4.m32 * weight[3],
            m33 = m1.m33 * weight[0] + m2.m33 * weight[1] + m3.m33 * weight[2] + m4.m33 * weight[3],
        };

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.Append("[");
            sb.Append(this.m00).Append(", ").Append(this.m01).Append(", ").Append(this.m02).Append(", ").Append(this.m03);
            sb.Append("]\n[");
            sb.Append(this.m10).Append(", ").Append(this.m11).Append(", ").Append(this.m12).Append(", ").Append(this.m13);
            sb.Append("]\n[");
            sb.Append(this.m20).Append(", ").Append(this.m21).Append(", ").Append(this.m22).Append(", ").Append(this.m23);
            sb.Append("]\n[");
            sb.Append(this.m30).Append(", ").Append(this.m31).Append(", ").Append(this.m32).Append(", ").Append(this.m33);
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

        public static Matrix4 operator ~(Matrix4 m1) => Inverse(m1);

        public static Matrix4 operator *(Matrix4 m1, Matrix4 m2) => Mul(m1, m2);

        public static Vector4 operator *(Vector4 v1, Matrix4 m1) => Transform(v1, m1);

        public static Vector3 operator *(Vector3 v1, Matrix4 m1) => Transform(v1, m1);

        public static implicit operator Matrix4(Matrix3 m) => new Matrix4(m);

        public static implicit operator Matrix4(Quaternion q1) => new Matrix4(q1);

        public static explicit operator OpenTK.Matrix4(Matrix4 m) => new OpenTK.Matrix4()
        {
            M11 = m.m00,
            M12 = m.m01,
            M13 = m.m02,
            M14 = m.m03,
            M21 = m.m10,
            M22 = m.m11,
            M23 = m.m12,
            M24 = m.m13,
            M31 = m.m20,
            M32 = m.m21,
            M33 = m.m22,
            M34 = m.m23,
            M41 = m.m30,
            M42 = m.m31,
            M43 = m.m32,
            M44 = m.m33,
        };

        public static implicit operator Matrix4(OpenTK.Matrix4 m) => new Matrix4()
        {
            m00 = m.M11,
            m01 = m.M12,
            m02 = m.M13,
            m03 = m.M14,
            m10 = m.M21,
            m11 = m.M22,
            m12 = m.M23,
            m13 = m.M24,
            m20 = m.M31,
            m21 = m.M32,
            m22 = m.M33,
            m23 = m.M34,
            m30 = m.M41,
            m31 = m.M42,
            m32 = m.M43,
            m33 = m.M44,
        };

        public static explicit operator float[] (Matrix4 m)
        {
            var result = new float[16];

            int index = 0;
            result[index++] = m.m00;
            result[index++] = m.m01;
            result[index++] = m.m02;
            result[index++] = m.m03;

            result[index++] = m.m10;
            result[index++] = m.m11;
            result[index++] = m.m12;
            result[index++] = m.m13;

            result[index++] = m.m20;
            result[index++] = m.m21;
            result[index++] = m.m22;
            result[index++] = m.m23;

            result[index++] = m.m30;
            result[index++] = m.m31;
            result[index++] = m.m32;
            result[index++] = m.m33;

            return result;
        }

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
    }
}
