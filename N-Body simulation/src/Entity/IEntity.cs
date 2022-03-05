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
        protected int ID = -1;
        protected vec3 position;
        protected vec3 axis;
        protected float rotation = 3.14f;
        public vec3 GetPosition()
        {
            return position;
        }
        public mat4 GetTransformMatrix()
        {
            return mat4.Translate(position); ;
        }
        public mat4 GetRotationMatrix()
        {
            return mat4.Rotate(rotation, axis);
        }
        public int GetID ()
        {
            return ID;
        }
        public void SetID (int i)
        {
            ID = i;
        }
        public override int GetHashCode()
        {
            return GetID();
        }
    }
}
