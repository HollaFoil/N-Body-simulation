using GlmSharp;
using N_Body_simulation.src.Noise;
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
        public static void TurnSphereIntoTerrain(Shape sphere, Noise.NoiseFilter[] NoiseFilters)
        {
            var vertices = sphere.GetVertices();
            for (int i = 0; i < vertices.Count; i++)
            {
                float elevation = 0;
                elevation += EvaluateAtPoint(vertices[i], NoiseFilters[0]);
                float mask = elevation;

                elevation += EvaluateAtPoint(vertices[i], NoiseFilters[1]) * mask;
                vertices[i] *= (1 + elevation);
            }
        }
        public static float EvaluateAtPoint(vec3 point, NoiseFilter noiseFilter)
        {
            return (noiseFilter.Evaluate(point));
        }
    }
}
