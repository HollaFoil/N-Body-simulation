using N_Body_simulation.src.Entity;
using N_Body_simulation.src.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N_Body_simulation.src.Render.Buffes
{
    internal class PlanetBuffer
    {
        ShapeBuffer TerrainBuffer;
        IEntity planet;

        public PlanetBuffer(IEntity planet)
        {
            Shape TerrainShape = planet.GetShape();
            TerrainBuffer = new ShapeBuffer(TerrainShape);
            this.planet = planet;
        }

        public void Render()
        {
            TerrainBuffer.Render(planet);
        }
        public void Buffer(IEntity planet)
        {
            Shape TerrainShape = planet.GetShape();
            TerrainBuffer.Buffer(TerrainShape);
        }
    }
}
