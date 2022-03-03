using GlmSharp;
using N_Body_simulation.src.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N_Body_simulation.src.Geometry
{
    internal static class CubeMesh
    {
        public static void CreateSphereMesh(uint faceResolution, out float[] vertices, out uint[] triangles)
        {
            vertices = new float[faceResolution * faceResolution * 18];
            triangles = new uint[(faceResolution - 1) * (faceResolution - 1) * 36];
            int offset1 = 0;
            int offset2 = 0;

            vec3[] faces = { new vec3(0, 1, 0), new vec3(1, 0, 0), new vec3(0, 0, 1), new vec3(0, 0, -1), new vec3(0, -1, 0), new vec3(-1, 0, 0) };
            for (uint face = 0; face < 6; face++)
            {
                FaceMesh.CreateFaceMesh(faceResolution, faces[face], face, out vec3[] v, out uint[] t);
                for (int j = 0; j < faceResolution*faceResolution; j++)
                {
                    vertices[offset1] = v[j].Normalized.x;
                    vertices[offset1 + 1] = v[j].Normalized.y;
                    vertices[offset1 + 2] = v[j].Normalized.z;
                    offset1 += 3;
                }
                for (int j = 0; j < (faceResolution - 1) * (faceResolution - 1) * 6; j++)
                {
                    triangles[offset2++] = t[j];
                }
            }
            Console.WriteLine();
        }
    }
}
