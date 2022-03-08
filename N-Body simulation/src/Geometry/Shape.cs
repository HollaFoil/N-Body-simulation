using GlmSharp;
using N_Body_simulation.src.Geometry.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N_Body_simulation.src.Geometry
{
    public class Shape
    {
        private List<vec3> vertices;
        private List<uint> triangles;
        private List<vec3> normals;
        private List<Color> colors;
        private List<float> rawDepth;
        private float MinElevation = 100000, MaxElevation = -100000;
        private float MinElevationOcean = 100000, MaxElevationOcean = -100000;
        public Shape()
        {
            vertices = new List<vec3>();
            rawDepth = new List<float>();
            triangles = new List<uint>();
        }
        public Shape(List<vec3> vertices, List<uint> triangles)
        {
            
            this.vertices = vertices;
            this.triangles = triangles;
            rawDepth = new List<float>(vertices.Count);
            for (int i = 0; i < vertices.Count; i++)
            {
                rawDepth.Add(1f);
            }
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
        public List<float> GetRawDepth()
        {
            return rawDepth;
        }
        public float[] GetColorsFloat()
        {
            float[] v = new float[colors.Count * 3];
            for (int i = 0; i < colors.Count; i++)
            {
                v[3 * i] = colors[i].RNorm;
                v[3 * i + 1] = colors[i].GNorm;
                v[3 * i + 2] = colors[i].BNorm;
            }
            return v;
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
            int resolution = (int)Math.Sqrt(vertices.Count / 6);
            Dictionary<vec3, List<int>> repeatingVerts = new Dictionary<vec3, List<int>>();
            normals = new List<vec3>(vertices.Count);
            for (int i = 0; i < vertices.Count; i++)
            {
                normals.Add(new vec3(0,0,0));
                var v = vertices[i];
                if (repeatingVerts.TryGetValue(v, out List<int> verts))
                {
                    verts.Add(i);
                }
                else
                {
                    verts = new List<int>();
                    verts.Add(i);
                    repeatingVerts.Add(v, verts);
                }
            }

            for (int i = 0; i < triangles.Count; i += 3)
            {
                
                int a = (int) triangles[i];
                int b = (int) triangles[i + 1];
                int c = (int) triangles[i + 2];

                vec3 v1 = vertices[a];
                vec3 v2 = vertices[b];
                vec3 v3 = vertices[c];

                vec3 edge1 = v2 - v1;
                vec3 edge2 = v3 - v1;

                vec3 sum = (glm.Cross(edge1, edge2)).NormalizedSafe;
                normals[a] += sum;
                normals[b] += sum;
                normals[c] += sum;

                if (repeatingVerts.TryGetValue(v1, out List<int> verts1))
                {
                    foreach (var v in verts1) if (v != a) normals[v] += sum;
                }
                if (repeatingVerts.TryGetValue(v2, out List<int> verts2))
                {
                    foreach (var v in verts2) if (v != b) normals[v] += sum;
                }
                if (repeatingVerts.TryGetValue(v3, out List<int> verts3))
                {
                    foreach (var v in verts3) if (v != c) normals[v] += sum;
                }

            }
            for (int i = 0; i < vertices.Count; i++)
            {
                normals[i] = normals[i].NormalizedSafe;
            }
        }
        public void FindMinMaxElevations()
        {
            for (int i = 0; i < vertices.Count; i++)
            {
                if (rawDepth[i] > 0)
                {
                    MinElevation = MathF.Min(MinElevation, rawDepth[i]);
                    MaxElevation = MathF.Max(MaxElevation, rawDepth[i]);
                }
                else
                {
                    MinElevationOcean = MathF.Min(MinElevationOcean, rawDepth[i]);
                    MaxElevationOcean = MathF.Max(MaxElevationOcean, rawDepth[i]);
                }
            }
        }
        public void CalculateColors(ColorSettings[] settings)
        {
            var landColors = settings[0];
            var oceanColors = settings[1];
            colors = new List<Color>(vertices.Count);
            FindMinMaxElevations();
            for (int i = 0; i < vertices.Count; i++)
            {
                float length = rawDepth[i];
                if (length > MinElevation)
                {
                    float pos = (length - MinElevation) / (MaxElevation - MinElevation);
                    colors.Add(landColors.GetColorAt(pos));
                }
                else
                {
                    float pos = (length - MinElevationOcean) / (MaxElevationOcean - MinElevationOcean);
                    colors.Add(oceanColors.GetColorAt(pos));
                }
                
            }

            
        }
    }
}
