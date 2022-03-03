using GlmSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N_Body_simulation.src.Shapes
{
    internal static class FaceMesh
    {
        
        public static void CreateFaceMesh(uint faceResolution, vec3 localUp, uint indexOffset, out vec3[] vertices, out uint[] triangles)
        {
            vec3 normal = localUp;
            vec3 axisA = new vec3(normal.y, normal.z, normal.x);
            vec3 axisB = glm.Cross(normal, axisA);

            vertices = new vec3[faceResolution*faceResolution];
            triangles = new uint[(faceResolution-1) * (faceResolution-1)*6];
            uint triIndex = 0;
            uint vertexOffset = faceResolution * faceResolution * indexOffset;

            for (uint i = 0; i < faceResolution; i++)
            {
                for (uint j = 0; j < faceResolution; j++)
                {
                    uint index = i*faceResolution+j;
                    vec2 percent = new vec2(i, j) / (faceResolution-1);
                    vec3 pointOnUnitCube = normal + (percent.x - 0.5f) * 2 * axisA + (percent.y - 0.5f) * 2 * axisB;
                    vertices[index] = pointOnUnitCube;

                    if (i != faceResolution-1 && j != faceResolution-1)
                    {
                        triangles[triIndex] = index + vertexOffset;
                        triangles[triIndex + 1] = index + faceResolution + 1 + vertexOffset;
                        triangles[triIndex + 2] = index + faceResolution + vertexOffset;

                        triangles[triIndex + 3] = index + vertexOffset;
                        triangles[triIndex + 4] = index + 1 + vertexOffset;
                        triangles[triIndex + 5] = index + faceResolution + 1 + vertexOffset;

                        triIndex += 6;
                    }
                }
            }
        }
    }
}
