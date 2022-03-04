using GlmSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N_Body_simulation.src.Geometry
{
    internal class Shape
    {
        private List<vec3> vertices;
        private List<uint> triangles;
        private List<vec3> normals;

        public Shape()
        {
            vertices = new List<vec3>();
            triangles = new List<uint>();
        }
        public Shape(List<vec3> vertices, List<uint> triangles)
        {
            this.vertices = vertices;
            this.triangles = triangles;
        }

        public void AddVertex(vec3 v)
        {
            vertices.Add(v);
        }
        public void AddTriIndex(uint i)
        {
            triangles.Add(i);
        }

        public List<uint> GetTriIndices()
        {
            return triangles;
        }
        public List<vec3> GetVertices()
        {
            return vertices;
        }
        public float[] GetVerticesFloat()
        {
            return Vec3ToFloatArray(vertices);
        }
        public float[] GetNormalsFloat()
        {
            return Vec3ToFloatArray(normals);
        }
        private float[] Vec3ToFloatArray(List<vec3> list)
        {
            float[] v = new float[list.Count * 3];
            for (int i = 0; i < list.Count; i++)
            {
                v[3 * i] = list[i].x;
                v[3 * i + 1] = list[i].y;
                v[3 * i + 2] = list[i].z;
            }
            return v;
        }

        public void CalculateNormals()
        {
            normals = new List<vec3>(vertices.Count);
            for (int i = 0; i < vertices.Count; i++)
            {
                normals.Add(new vec3(0,0,0));
            }

            for (int i = 0; i < triangles.Count; i += 3)
            {
                int a = (int) triangles[i];
                int b = (int) triangles[i + 1];
                int c = (int) triangles[i + 2];

                vec3 v1 = vertices[a];
                vec3 v2 = vertices[b];
                vec3 v3 = vertices[c];

                vec3 sum = glm.Cross(v1, v2) + glm.Cross(v2, v3) + glm.Cross(v3, v1);
                normals[a] += sum;
                normals[b] += sum;
                normals[c] += sum;
            }
            for (int i = 0; i < vertices.Count; i++)
            {
                normals[i] = normals[i].NormalizedSafe;
            }
        }
    }
}
