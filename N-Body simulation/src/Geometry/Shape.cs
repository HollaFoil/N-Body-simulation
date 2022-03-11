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
        private List<float> heightsNorm;
        private List<float> rawDepth;
        private List<Color> gradientTexture;

        private float MinElevation = 100000, MaxElevation = -100000;
        private float MinElevationOcean = 100000, MaxElevationOcean = -100000;
        public Shape()
        {
            vertices = new List<vec3>();
            rawDepth = new List<float>();
            triangles = new List<uint>();
            gradientTexture = new List<Color>();
        }
        public Shape(List<vec3> vertices, List<uint> triangles)
        {
            gradientTexture = new List<Color>();
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
        public List<float> GetHeightsNormalized()
        {
            return heightsNorm;
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
        public float[] GetGradientTextureFloat()
        {
            float[] v = new float[gradientTexture.Count * 3];
            for (int i = 0; i < gradientTexture.Count; i++)
            {
                v[3 * i] = gradientTexture[i].RNorm;
                v[3 * i + 1] = gradientTexture[i].GNorm;
                v[3 * i + 2] = gradientTexture[i].BNorm;
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
                normals[i] = -normals[i].NormalizedSafe;
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
        public void CreateGradientTexture(ColorSettings[] settings, float step = 0.01f)
        {
            for (float i = -1f; i <= 1f; i += step)
            {
                if (i <= 0)
                {
                    gradientTexture.Add(settings[1].GetColorAt(i + 1));
                }
                else
                {
                    gradientTexture.Add(settings[0].GetColorAt(i));
                }
            }
        }
        public void CalculateColors(ColorSettings[] settings)
        {
            var landColors = settings[0];
            var oceanColors = settings[1];
            heightsNorm = new List<float>(vertices.Count);
            FindMinMaxElevations();
            CreateGradientTexture(settings);
            for (int i = 0; i < vertices.Count; i++)
            {
                float length = rawDepth[i];
                if (length > MinElevation)
                {
                    float pos = (length - MinElevation) / (MaxElevation - MinElevation);
                    heightsNorm.Add(pos);
                }
                else
                {
                    float pos = (length - MinElevationOcean) / (MaxElevationOcean - MinElevationOcean);
                    heightsNorm.Add(pos - 1.0f);
                }
                
            }

            
        }
    }
}
