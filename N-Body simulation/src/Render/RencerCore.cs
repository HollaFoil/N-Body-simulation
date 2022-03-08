using GLFW;
using GlmSharp;
using N_Body_simulation.src.Entity;
using N_Body_simulation.src.Geometry;
using N_Body_simulation.src.Render.Buffes;
using N_Body_simulation.src.Render.Shaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static OpenGL.GL;

namespace N_Body_simulation.src.Render
{
    internal static class RenderCore
    {
        static int viewLoc;
        static Dictionary<int, PlanetBuffer> Buffers;
        static Shape Ocean;
        static Shape Terrain;

        public static int transformLocation;
        public static int rotationLocation;

        static ShaderProgram program;
        static RenderCore()
        {
            program = new ShaderProgram();
            Buffers = new Dictionary<int, PlanetBuffer>();

            viewLoc = glGetUniformLocation(GetProgram(), "view");
            RefreshProjectionMatrix();
            glEnable(GL_DEPTH_TEST);
            glDepthFunc(GL_LESS);
            glClearColor(0f, 0f, 0f, 1.0f);

            transformLocation = glGetUniformLocation(GetProgram(), "transform");
            rotationLocation = glGetUniformLocation(GetProgram(), "rotation");
            //glPolygonMode(GL_FRONT_AND_BACK, GL_LINE);
            //glEnable(GL_CULL_FACE);
            //glCullFace(GL_FRONT);
        }
        static public void Flush()
        {
            glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
            RenderBuffers();
            Glfw.SwapBuffers(Window.GetWindow());

        }
        
        static public void BufferEntity(IEntity p)
        {
            if (p.GetID() == -1)
            {
                int ID = Buffers.Count + 1;
                p.SetID(ID);
                Buffers.Add(ID, new PlanetBuffer(p));
            }

            Buffers[p.GetID()].Buffer(p);
        }

        static public uint GetProgram()
        {
            return program.program;
        }
        static private unsafe void RenderBuffers()
        {
            glUniformMatrix4fv(viewLoc, 1, false, Program._camera.GetLookAtMatrix().ToArray());
            foreach (var buffer in Buffers.Values) buffer.Render();
        }
        public static void RefreshProjectionMatrix()
        {
            mat4 projection;
            projection = mat4.Perspective(glm.Radians(90.0f), 1920 / (float)1080, 1f, 60000.0f);
            float[] p = projection.ToArray();
            int projectionLoc = glGetUniformLocation(GetProgram(), "projection");
            glUniformMatrix4fv(projectionLoc, 1, false, p);
        }
    }
}
