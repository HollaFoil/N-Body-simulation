using GlmSharp;
using N_Body_simulation.src.Tick;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N_Body_simulation.src.Entity
{
    public class IEntity : ITickable
    {
        protected vec3 position;
        protected vec2 facing;
        public vec3 GetPosition()
        {
            return position;
        }
        public vec2 GetYawPitch()
        {
            return facing;
        }
    }
}
