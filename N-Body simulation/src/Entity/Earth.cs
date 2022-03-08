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
    internal class Earth : IEntity
    {
        private NoiseFilter[] NoiseFilters;
        private GeometrySettings GeometrySettings;
        private ColorSettings[] ColorSettings;
        
        public Earth()
        {
            axis = new vec3(5, -1, 0).Normalized;
            position = new vec3(0, 0, 0);
            CreateSettings();
            GeometrySettings = new GeometrySettings(50f);
            
            GenerateMeshes();
        }

        protected override void OnUpdate()
        {
            rotation += 0.01f;
            
        }
        
        protected override void GeneratePlanet(out Shape Terrain, uint resolution) 
        {
            List<vec3> vertices;
            List<uint> triangles;

            CreateSphere(resolution, out vertices, out triangles);
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

            ColorSettings = new ColorSettings[2];
            ColorSettings[0] = new ColorSettings();
            ColorSettings[1] = new ColorSettings();

            ColorSettings[0].Add(new Mark(0.04f, new Color(164, 174, 91)));
            ColorSettings[0].Add(new Mark(0.10f, new Color(112, 183, 19)));
            ColorSettings[0].Add(new Mark(0.47f, new Color(145, 90, 42)));
            ColorSettings[0].Add(new Mark(0.70f, new Color(125, 81, 45)));
            ColorSettings[0].Add(new Mark(0.78f, new Color(255, 255, 255)));

            ColorSettings[1].Add(new Mark(0.6f, new Color(30, 64, 128)));
            ColorSettings[1].Add(new Mark(0.9f, new Color(30, 102, 204)));
            ColorSettings[1].Add(new Mark(1f, new Color(68, 167, 196))); 
        }
        private void CreateSphere(uint resolution, out List<vec3> vertices, out List<uint> triangles)
        {
            CubeMesh.CreateCubeMesh(resolution, out vertices, out triangles);
            GeometryGenerator.MorphCubeToSphere(ref vertices);
        }
        public NoiseFilter[] GetNoiseSettings()
        {
            return NoiseFilters;
        }
        public void SetNoiseSettings(NoiseFilter[] noiseFilters)
        {
            NoiseFilters = noiseFilters;
            GenerateMeshes();
            Render.RenderCore.BufferEntity(this);
        }
    }
}
