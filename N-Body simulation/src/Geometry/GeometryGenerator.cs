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
            var depths = shape.GetRawDepth();
            for (int i = 0; i < vertices.Count; i++)
            {
                vertices[i] = vertices[i] * scaleFactor;
                depths[i] = depths[i] * scaleFactor;
            }
        }
        public static void TurnSphereIntoTerrain(Shape sphere, Noise.NoiseFilter[] NoiseFilters)
        {
            var vertices = sphere.GetVertices();
            var depths = sphere.GetRawDepth();
            for (int i = 0; i < vertices.Count; i++)
            {
                float elevation = GetUnscaledElevation(vertices[i], NoiseFilters);
                float elevationCapped = GetScaledElevation(elevation);

                depths[i] = elevation;

                
                vertices[i] *= (1 + elevationCapped);
            }
            Console.WriteLine();
            //MathF.Max(0, noiseValue - settings.minvalue);
        }
        public static float GetUnscaledElevation(vec3 point, NoiseFilter[] filters)
        {
            float val1 = EvaluateAtPoint(point, filters[0]);
            float val2 = EvaluateAtPoint(point, filters[1]);
            float elevation1 = val1;
            float elevation2 = (val2) * elevation1;
            return elevation1 + elevation2;
        }
        public static float GetScaledElevation(float elevation)
        {
            elevation = MathF.Max(elevation, 0);
            return 1 + elevation;
        }
        public static float EvaluateAtPoint(vec3 point, NoiseFilter noiseFilter)
        {   float val = noiseFilter.Evaluate(point);
            return val;
        }
    }
}
