using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GlmSharp;
using N_Body_simulation.src.Geometry;
using N_Body_simulation.src.Noise;

namespace N_Body_simulation.src.Entity
{
    internal class Planet : IEntity
    {
        private NoiseSettings NoiseSettings;
        private NoiseFilter NoiseFilter;
        private GeometrySettings GeometrySettings;
        private Shape Terrain;
        public Planet()
        {
            NoiseSettings = new NoiseSettings();
            NoiseFilter = new NoiseFilter(NoiseSettings);
            GeometrySettings = new GeometrySettings(1f);
            GeneratePlanet();
        }

        private void GeneratePlanet() 
        {
            List<vec3> vertices;
            List<uint> triangles;


            CreateSphere(out vertices, out triangles);
            Terrain = new Shape(vertices, triangles);

            GeometryGenerator.TurnSphereIntoTerrain(Terrain, NoiseFilter);
            GeometryGenerator.ScaleShape(Terrain, GeometrySettings.Scale);
            Terrain.CalculateNormals();
        }
        private void CreateSphere(out List<vec3> vertices, out List<uint> triangles)
        {
            CubeMesh.CreateCubeMesh(100, out vertices, out triangles);
            GeometryGenerator.MorphCubeToSphere(ref vertices);
        }
        public Shape GetShapes()
        {
            return Terrain;
        }
        public NoiseSettings GetNoiseSettings()
        {
            return NoiseSettings;
        }
        public void SetNoiseSettings(NoiseSettings settings)
        {
            NoiseSettings = settings;
            NoiseFilter.settings = settings;
            GeneratePlanet();
            Render.RenderCore.BufferPlanet(this);
        }
    }
}
