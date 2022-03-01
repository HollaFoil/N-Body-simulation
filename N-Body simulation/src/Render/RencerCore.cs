using GLFW;
using GlmSharp;
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
        static uint vao, vbo;

        static ShaderProgram program;
        static RenderCore()
        {
            program = new ShaderProgram();
            viewLoc = glGetUniformLocation(GetProgram(), "view");
            RefreshProjectionMatrix();
            glEnable(GL_DEPTH_TEST);
            glDepthFunc(GL_LESS);
            glClearColor(0.5f, 0.95f, 1.0f, 1.0f);

            //glEnable(GL_CULL_FACE);
            //glCullFace(GL_BACK);
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

            float[] vertices = {
                // first triangle
                    0.5f,  0.5f, 0.0f,  // top right
                    0.5f, -0.5f, 0.0f,  // bottom right
                -0.5f,  0.5f, 0.0f,  // top left 
                // second triangle
                    0.5f, -0.5f, 0.0f,  // bottom right
                -0.5f, -0.5f, 0.0f,  // bottom left
                -0.5f,  0.5f, 0.0f   // top left
            };

            glBindVertexArray(vao);
            glBindBuffer(GL_ARRAY_BUFFER, vbo);

            fixed (void* ptr = &vertices[0]) {
                glBufferData(GL_ARRAY_BUFFER, sizeof(float) * 18, ptr, GL_STATIC_DRAW);
            }
            // 3. then set our vertex attributes pointers
            glVertexAttribPointer(0, 3, GL_FLOAT, false, 3 * sizeof(float), (void*)0);
            glEnableVertexAttribArray(0);
        }
        static public uint GetProgram()
        {
            return program.program;
        }
        static private unsafe void RenderBuffers()
        {
            glUniformMatrix4fv(viewLoc, 1, false, Program._camera.GetLookAtMatrix().ToArray());
            glBindBuffer(GL_ARRAY_BUFFER, vbo);
            glDrawArrays(GL_TRIANGLES, 0, 6);
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
