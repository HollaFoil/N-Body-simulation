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

        public PlanetBuffer(Planet planet)
        {
            Shape TerrainShape = planet.GetShapes();
            TerrainBuffer = new ShapeBuffer(TerrainShape);
        }

        public void Render()
        {
            TerrainBuffer.Render();
        }
        public void Buffer(Planet planet)
        {
            Shape TerrainShape = planet.GetShapes();
            TerrainBuffer.Buffer(TerrainShape);
        }
    }
}
