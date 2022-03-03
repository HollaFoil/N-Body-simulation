using GLFW;
using GlmSharp;
using N_Body_simulation.src.Geometry;
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
        static uint vao, vbo, ebo;
        static int elements = 0;

        static ShaderProgram program;
        static RenderCore()
        {
            program = new ShaderProgram();
            viewLoc = glGetUniformLocation(GetProgram(), "view");
            RefreshProjectionMatrix();
            glEnable(GL_DEPTH_TEST);
            glDepthFunc(GL_LESS);
            glClearColor(0.5f, 0.95f, 1.0f, 1.0f);
            //glPolygonMode(GL_FRONT_AND_BACK, GL_LINE);
            glEnable(GL_CULL_FACE);
            glCullFace(GL_FRONT);
            Test();
        }
        static public void Flush()
        {
            glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
            RenderBuffers();
            Glfw.SwapBuffers(Window.GetWindow());

        }
        static unsafe public void Test()
        {
            vao = glGenVertexArray();
            vbo = glGenBuffer();
            ebo = glGenBuffer();

            CubeMesh.CreateSphereMesh(15, out float[] vertices, out uint[] triangles);

            glBindVertexArray(vao);
            glBindBuffer(GL_ARRAY_BUFFER, vbo);

            fixed (void* ptr = &vertices[0]) {
                glBufferData(GL_ARRAY_BUFFER, sizeof(float) * vertices.Length, ptr, GL_STATIC_DRAW);
            }

            glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, ebo);
            fixed (void* ptr = &triangles[0])
            {
                glBufferData(GL_ELEMENT_ARRAY_BUFFER, sizeof(int) * triangles.Length, ptr, GL_STATIC_DRAW);
            }
            
            glVertexAttribPointer(0, 3, GL_FLOAT, false, 3 * sizeof(float), (void*)0);
            glEnableVertexAttribArray(0);
            elements = triangles.Length;
        }
        static public uint GetProgram()
        {
            return program.program;
        }
        static private unsafe void RenderBuffers()
        {
            glUniformMatrix4fv(viewLoc, 1, false, Program._camera.GetLookAtMatrix().ToArray());
            glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, ebo);
            glDrawElements(GL_TRIANGLES, elements, GL_UNSIGNED_INT, null);
        }
        public static void RefreshProjectionMatrix()
        {
            mat4 projection;
            projection = mat4.Perspective(glm.Radians(90.0f), 1280 / (float)720, 0.1f, 600.0f);
            float[] p = projection.ToArray();
            int projectionLoc = glGetUniformLocation(GetProgram(), "projection");
            glUniformMatrix4fv(projectionLoc, 1, false, p);
        }
    }
}
