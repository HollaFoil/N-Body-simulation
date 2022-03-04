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
        public static void CreateCubeMesh(uint faceResolution, out List<vec3> vertices, out List<uint> triangles)
        {
            vertices = new List<vec3>((int)(faceResolution * faceResolution * 6));
            triangles = new List<uint>((int)((faceResolution - 1) * (faceResolution - 1) * 36));

            vec3[] faces = { new vec3(0, 1, 0), new vec3(1, 0, 0), new vec3(0, 0, 1), new vec3(0, 0, -1), new vec3(0, -1, 0), new vec3(-1, 0, 0) };
            for (uint face = 0; face < 6; face++)
            {
                FaceMesh.CreateFaceMesh(faceResolution, faces[face], face, out vec3[] v, out uint[] t);
                for (int j = 0; j < faceResolution*faceResolution; j++)
                {
                    vertices.Add(v[j]);
                }
                for (int j = 0; j < (faceResolution - 1) * (faceResolution - 1) * 6; j++)
                {
                    triangles.Add(t[j]);
                }
            }
        }
    }
}
