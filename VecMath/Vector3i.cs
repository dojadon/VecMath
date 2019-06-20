using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VecMath
{
    [Serializable]
    public struct Vector3i
    {
        public static readonly Vector3i Zero = new Vector3i();
        public static readonly Vector3i UnitX = new Vector3i(1, 0, 0);
        public static readonly Vector3i UnitY = new Vector3i(0, 1, 0);
        public static readonly Vector3i UnitZ = new Vector3i(0, 0, 1);

        public static readonly Vector3i[] Units = { UnitX, UnitY, UnitZ };

        public int x;
        public int y;
        public int z;

        public int this[int idx]
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

        public Vector3i(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Vector3i(IEnumerable<int> pos) : this(pos.First(), pos.First(), pos.First())
        {
        }

        public static Vector3i Add(Vector3i v1, Vector3i v2) => new Vector3i(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z);

        public static Vector3i Sub(Vector3i v1, Vector3i v2) => new Vector3i(v1.x - v2.x, v1.y - v2.y, v1.z - v2.z);

        public static Vector3i Scale(Vector3i v1, int d1) => new Vector3i(v1.x * d1, v1.y * d1, v1.z * d1);

        public static Vector3i Scale(Vector3i v1, Vector3i v2) => new Vector3i(v1.x * v2.x, v1.y * v2.y, v1.z * v2.z);

        public float Length() => (float)Math.Sqrt(LengthSquare());

        public float LengthSquare() => x * x + y * y + z * z;

        public override string ToString() => $"[{x}, {y}, {z}]";

        public override int GetHashCode() => x ^ y << 2 ^ z >> 2;

        public override bool Equals(object obj)
        {
            if (obj is Vector3 v)
            {
                return Equals(v);
            }
            return false;
        }

        public bool Equals(Vector3i v1) => this == v1;

        public static bool operator ==(Vector3i v1, Vector3i v2) => v1.x == v2.x && v1.y == v2.y && v1.z == v2.z;
        public static bool operator !=(Vector3i v1, Vector3i v2) => !(v1.x == v2.x && v1.y == v2.y && v1.z == v2.z);

        public static bool operator <(Vector3i v1, Vector3i v2) => v1.x < v2.x && v1.y < v2.y && v1.z < v2.z;
        public static bool operator >(Vector3i v1, Vector3i v2) => v1.x > v2.x && v1.y > v2.y && v1.z > v2.z;

        public static bool operator <=(Vector3i v1, Vector3i v2) => v1.x <= v2.x && v1.y <= v2.y && v1.z <= v2.z;
        public static bool operator >=(Vector3i v1, Vector3i v2) => v1.x >= v2.x && v1.y >= v2.y && v1.z >= v2.z;

        public static Vector3i operator +(Vector3i v1, Vector3i v2) => Add(v1, v2);

        public static Vector3i operator -(Vector3i v1, Vector3i v2) => Sub(v1, v2);

        public static Vector3i operator *(Vector3i v1, int d1) => Scale(v1, d1);

        public static Vector3i operator *(int d1, Vector3i v1) => Scale(v1, d1);
    }
}
