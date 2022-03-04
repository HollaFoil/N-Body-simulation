using GlmSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N_Body_simulation.src.Geometry
{
    internal class GeometryGenerator
    {
        public static void MorphCubeToSphere(ref List<vec3> cubeVertices)
        {
            for (int i = 0; i < cubeVertices.Count; i++)
            {
                cubeVertices[i] = cubeVertices[i].Normalized;
            }
        }
        public static void ScaleVertices(ref List<vec3> vertices, float scaleFactor)
        {
            for (int i = 0; i < vertices.Count; i++)
            {
                vertices[i] = vertices[i] * scaleFactor;
            }
        }
        public static void ScaleShape(Shape shape, float scaleFactor)
        {
            var vertices = shape.GetVertices();
            for (int i = 0; i < vertices.Count; i++)
            {
                vertices[i] = vertices[i] * scaleFactor;
            }
        }
        public static void TurnSphereIntoTerrain(Shape sphere, Noise.NoiseFilter NoiseFilter)
        {
            var vertices = sphere.GetVertices();
            for (int i = 0; i < vertices.Count; i++)
            {
                vertices[i] *= (1+(float)NoiseFilter.Evaluate(vertices[i]));
            }
        }
    }
}
