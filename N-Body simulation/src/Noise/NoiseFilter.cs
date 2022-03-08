using GlmSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplex = N_Body_simulation.src.Util.Noise;

namespace N_Body_simulation.src.Noise
{
    
    internal class NoiseFilter
    {
        public NoiseSettings settings;
        Simplex noise = new Simplex();
        public NoiseFilter(NoiseSettings settings)
        {
            this.settings = settings;
        }
        public float Evaluate(vec3 point)
        {
            float noiseValue = 0;
            float frequency = settings.baseRoughness;
            float amplitude = 1f;

            for (int i = 0; i < settings.numOfLayers; i++)
            {
                vec3 p = point * frequency + settings.center;
                float v = noise.Evaluate(p);
                noiseValue += (v + 1.0f) * 0.5f * amplitude;

                frequency *= settings.roughness;
                amplitude *= settings.persistance;
            }

            return (noiseValue - settings.minvalue) * settings.strength;
        }
    }
}
