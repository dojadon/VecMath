using System;

namespace VecMath.Geometry
{
    public interface IGeometry
    {
        bool IsIntersectWithRay(Ray ray);
        float CalculateTimeToIntersectWithRay(Ray ray);
    }

    public class EmptyGeometry : IGeometry
    {
        public float CalculateTimeToIntersectWithRay(Ray ray) => float.MaxValue;
        public bool IsIntersectWithRay(Ray ray) => false;
    }

    public struct AABoundingBox : IGeometry
    {
        public static AABoundingBox Root { get; } = new AABoundingBox(new Vector3(1, 1, 1) * -1E10F, new Vector3(1, 1, 1) * 1E10F);

        public Vector3 PosMin { get; set; }
        public Vector3 PosMax { get; set; }

        public AABoundingBox(Vector3 pos1, Vector3 pos2)
        {
            PosMin = VMath.Min(pos1, pos2);
            PosMax = VMath.Max(pos1, pos2);
        }

        public bool IsIntersectWithPoint(Vector3 pos)
        {
            return PosMin.x <= pos.x && pos.x <= PosMin.x && PosMin.y <= pos.y && pos.y <= PosMin.y && PosMin.z <= pos.z && pos.z <= PosMin.z;
        }

        public bool IsIntersectWithRay(Ray ray)
        {
            return IsIntersectWithPoint(ray.pos) || CalculateTimeToIntersect(ray.pos, ray.vec, out float min, out float max);
        }

        public float CalculateTimeToIntersectWithRay(Ray ray)
        {
            CalculateTimeToIntersect(ray.pos, ray.vec, out float min, out float max);
            return min;
        }

        public bool CalculateTimeToIntersect(Vector3 pos, Vector3 vec, out float min, out float max)
        {
            max = float.MaxValue;
            min = float.MinValue;

            if (IsIntersectWithPoint(pos))
            {
                min = 0;

                for (int i = 0; i < 3; i++)
                {
                    float v = vec[i] != 0 ? vec[i] : 1E-7F;
                    float t1 = (PosMin[i] - pos[i]) / v;
                    float t2 = (PosMax[i] - pos[i]) / v;

                    max = Math.Min(max, Math.Max(t1, t2));
                }
                return true;
            }

            for (int i = 0; i < 3; i++)
            {
                float v = vec[i] != 0 ? vec[i] : 1E-7F;
                float t1 = (PosMin[i] - pos[i]) / v;
                float t2 = (PosMax[i] - pos[i]) / v;

                float near = Math.Min(t1, t2);
                float far = Math.Max(t1, t2);

                min = Math.Max(min, near);
                max = Math.Min(max, far);

                if (min > max + 1E+4F) return false;
            }
            return true;
        }
    }

    public struct Vertex
    {
        public Vector3 pos;
        public Vector3 normal;
        public Vector2 uv;

        public Vertex(Vector3 pos, Vector3 normal, Vector2 uv)
        {
            this.pos = pos;
            this.normal = normal;
            this.uv = uv;
        }
    }

    public class Polygon : IGeometry
    {
        public Vertex V1 { get => vertices[0]; set => vertices[0] = value; }
        public Vertex V2 { get => vertices[1]; set => vertices[1] = value; }
        public Vertex V3 { get => vertices[2]; set => vertices[2] = value; }
        public Vertex[] vertices = new Vertex[3];
        public Vector3 normal;

        public Polygon()
        {
            vertices = new Vertex[3];
        }

        public Polygon(Vertex[] vertices)
        {
            this.vertices = vertices;
            normal = VMath.Normalize(V1.normal + V2.normal + V3.normal);
        }

        public Polygon(Vertex v1, Vertex v2, Vertex v3) : this()
        {
            V1 = v1;
            V2 = v2;
            V3 = v3;
            normal = VMath.Normalize(v1.normal + v2.normal + v3.normal);
        }

        public Polygon Copy() => new Polygon(V1, V2, V3);

        public bool IsIntersectWithRay(Ray ray)
        {
            var cross1 = VMath.Cross(V2.pos - V1.pos, ray.pos - V1.pos);
            var dot1 = VMath.Dot(cross1, ray.vec);

            var cross2 = VMath.Cross(V3.pos - V2.pos, ray.pos - V2.pos);
            var dot2 = VMath.Dot(cross2, ray.vec);

            if (dot1 > 0 ^ dot2 > 0)
            {
                return false;
            }

            var cross3 = VMath.Cross(V1.pos - V3.pos, ray.pos - V3.pos);
            var dot3 = VMath.Dot(cross3, ray.vec);

            return !(dot2 > 0 ^ dot3 > 0);
        }

        public float CalculateTimeToIntersectWithRay(Ray ray)
        {
            float dot = VMath.Dot(normal, ray.vec);

            if (Math.Abs(dot) <= 1E-6) return 1E+6F;

            return VMath.Dot(normal, V1.pos - ray.pos) / dot;
        }
    }

    public struct Triangle : IGeometry
    {
        public Vector3 Pos1 { get; }
        public Vector3 Pos2 { get; }
        public Vector3 Pos3 { get; }
        public Vector3 Normal { get; }

        public Triangle(Vector3 pos1, Vector3 pos2, Vector3 pos3)
        {
            Pos1 = pos1;
            Pos2 = pos2;
            Pos3 = pos3;
            Normal = VMath.Cross(Pos3 - Pos1, Pos2 - Pos1);
        }

        public bool IsIntersectWithRay(Ray ray)
        {
            var cross1 = VMath.Cross(Pos2 - Pos1, ray.pos - Pos1);
            var dot1 = VMath.Dot(cross1, ray.vec);

            var cross2 = VMath.Cross(Pos3 - Pos2, ray.pos - Pos2);
            var dot2 = VMath.Dot(cross2, ray.vec);

            if (dot1 > 0 ^ dot2 > 0)
            {
                return false;
            }

            var cross3 = VMath.Cross(Pos1 - Pos3, ray.pos - Pos3);
            var dot3 = VMath.Dot(cross3, ray.vec);

            return !(dot2 > 0 ^ dot3 > 0);
        }

        public float CalculateTimeToIntersectWithRay(Ray ray)
        {
            float dot = VMath.Dot(Normal, ray.vec);

            if (Math.Abs(dot) <= 1E-6) return 1E+6F;

            return VMath.Dot(Normal, Pos1 - ray.pos) / dot;
        }
    }

    public struct Plane : IGeometry
    {
        public Vector3 Pos { get; set; }
        public Vector3 Normal { get; set; }

        public Plane(Vector3 pos, Vector3 normal)
        {
            Pos = pos;
            Normal = normal;
        }

        public bool IsIntersectWithRay(Ray ray)
        {
            return Math.Abs(VMath.Dot(Normal, ray.vec)) > 1E-6;
        }

        public float CalculateTimeToIntersectWithRay(Ray ray)
        {
            float dot = VMath.Dot(Normal, ray.vec);

            if (Math.Abs(dot) <= 1E-6) return 1E+6F;

            return VMath.Dot(Normal, Pos - ray.pos) / dot;
        }
    }

    public struct Sphere : IGeometry
    {
        public Vector3 Pos { get; }
        public float Range { get; }

        public Sphere(Vector3 pos, float range)
        {
            Pos = pos;
            Range = range;
        }

        public float CalculateTimeToIntersectWithRay(Ray ray)
        {
            float a0 = ray.vec.LengthSquare();
            float a1 = VMath.Dot(ray.vec, Pos - ray.pos);
            float a2 = (Pos - ray.pos).LengthSquare() - Range * Range;

            float d = a1 * a1 - a0 * a2;

            if (d < 0) return 1E+6F;

            float d2 = (float)Math.Sqrt(d);

            if (-a1 - d2 >= 0)
            {
                return -(a1 + d2) / (2 * a0);
            }
            else
            {
                return -(a1 - d2) / (2 * a0);
            }
        }

        public bool IsIntersectWithRay(Ray ray)
        {
            var target = Pos - ray.pos;
            float time = VMath.Dot(VMath.Normalize(ray.vec), target);

            if (time <= 0) return false;

            float distance = target.LengthSquare() - time * time;

            return distance <= Range * Range;
        }
    }
}
