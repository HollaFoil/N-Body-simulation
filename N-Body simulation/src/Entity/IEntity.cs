using GlmSharp;
using N_Body_simulation.src.Geometry;
using N_Body_simulation.src.Tick;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N_Body_simulation.src.Entity
{
    public abstract class IEntity : ITickable
    {
        protected int ID = -1;
        protected vec3 position;
        protected vec3 axis;
        protected float rotation = 3.14f;

        protected Shape[] Terrain = new Shape[4];
        private float[,] BitMapPositions = new float[,] { { 0f, 170f, 300f, 600f }, { 150f, 250f, 550f, 999999f } };
        protected Shape ActiveShape;
        public uint resolution = 400;
        protected void GenerateMeshes()
        {
            int res = (int)resolution;
            for (int i = 0; i < Terrain.Length; i++, res = (int)((float)res * 0.7))
            {
                GeneratePlanet(out Terrain[i], (uint)res);
            }
            ActiveShape = Terrain[0];
        }
        protected abstract void GeneratePlanet(out Shape Terrain, uint resolution);
        protected abstract void OnUpdate();
        protected override void Update()
        {
            float dist = GetDistanceFromCamera();
            for (int i = 0; i < 4; i++)
            {
                if (!(dist > BitMapPositions[0, i] && dist < BitMapPositions[1, i])) continue;
                if (ActiveShape == Terrain[i]) break;

                ActiveShape = Terrain[i];
                Buffer();
                break;
            }
            OnUpdate();
        }
        public vec3 GetPosition()
        {
            return position;
        }
        public void SetPosition(vec3 pos)
        {
            position = pos;
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
        public float GetDistanceFromCameraSquared()
        {
            return (position - Program.Camera.GetPosition()).LengthSqr;
        }
        public float GetDistanceFromCamera()
        {
            return (position - Program.Camera.GetPosition()).Length;
        }
        public void Buffer()
        {
            Render.RenderCore.BufferEntity(this);
        }
        public Shape GetShape()
        {
            return ActiveShape;
        }
    }
}
