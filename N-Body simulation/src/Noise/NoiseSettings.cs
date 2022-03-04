using GlmSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N_Body_simulation.src.Noise
{
    internal class NoiseSettings
    {
        public int numOfLayers = 5;
        public float roughness = 1.83f;
        public float persistance = 0.54f;
        public float strength = 0.12f;
        public float minvalue = 1.1f;
        public float baseRoughness = 0.71f;
        public vec3 center = new vec3(2.86f, 1.84f, 2.48f);
        
    }
}
