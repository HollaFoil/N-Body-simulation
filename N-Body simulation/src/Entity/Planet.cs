using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GlmSharp;
using N_Body_simulation.src.Geometry;
using N_Body_simulation.src.Geometry.Settings;
using N_Body_simulation.src.Noise;

namespace N_Body_simulation.src.Entity
{
    internal class Planet : IEntity
    {
        private NoiseFilter[] NoiseFilters;
        private GeometrySettings GeometrySettings;
        private ColorSettings ColorSettings;
        private Shape Terrain;
        public Planet()
        {
            CreateSettings();
            GeometrySettings = new GeometrySettings(1f);
            ColorSettings = new ColorSettings();
            GeneratePlanet();
        }

        private void GeneratePlanet() 
        {
            List<vec3> vertices;
            List<uint> triangles;


            CreateSphere(out vertices, out triangles);
            Terrain = new Shape(vertices, triangles);

            GeometryGenerator.TurnSphereIntoTerrain(Terrain, NoiseFilters);
            GeometryGenerator.ScaleShape(Terrain, GeometrySettings.Scale);
            Terrain.CalculateNormals();
            Terrain.CalculateColors(ColorSettings);
        }
        private void CreateSettings()
        {
            NoiseSettings settingsContinents = new NoiseSettings();
            settingsContinents.baseRoughness = 0.71f;
            settingsContinents.strength = 0.25f;
            settingsContinents.minvalue = 1f;
            settingsContinents.numOfLayers = 4;
            settingsContinents.persistance = 0.34f;
            settingsContinents.roughness = 2.74f;
            settingsContinents.center = new vec3(2.86f, 1.84f, 2.48f);

            NoiseSettings settingsHills = new NoiseSettings();
            settingsHills.baseRoughness = 1.08f;
            settingsHills.strength = 4.3f;
            settingsHills.minvalue = 1f;
            settingsHills.numOfLayers = 5;
            settingsHills.persistance = 0.5f;
            settingsHills.roughness = 2.34f;
            settingsHills.center = new vec3(0f, 0f, 4.6f);

            NoiseFilter noiseContinents = new NoiseFilter(settingsContinents);
            NoiseFilter noiseHills = new NoiseFilter(settingsHills);
            NoiseFilters = new NoiseFilter[]{ noiseContinents, noiseHills};
        }
        private void CreateSphere(out List<vec3> vertices, out List<uint> triangles)
        {
            CubeMesh.CreateCubeMesh(250, out vertices, out triangles);
            GeometryGenerator.MorphCubeToSphere(ref vertices);
        }
        public Shape GetShapes()
        {
            return Terrain;
        }
        /*public NoiseSettings GetNoiseSettings()
        {
            return NoiseSettings;
        }
        public void SetNoiseSettings(NoiseSettings settings)
        {
            NoiseSettings = settings;
            NoiseFilter.settings = settings;
            GeneratePlanet();
            Render.RenderCore.BufferPlanet(this);
        }*/
    }
}
