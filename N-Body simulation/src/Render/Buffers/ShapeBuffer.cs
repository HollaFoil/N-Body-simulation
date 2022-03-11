using N_Body_simulation.src.Entity;
using N_Body_simulation.src.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static OpenGL.GL;

namespace N_Body_simulation.src.Render
{
    internal class ShapeBuffer
    {
        private uint vao, vbo, ebo, texID;
        private int elements = 0;

        public ShapeBuffer(Shape shape)
        {
            CreateBuffer();
            Buffer(shape);
        }

        private void CreateBuffer()
        {
            vao = glGenVertexArray();
            vbo = glGenBuffer();
            ebo = glGenBuffer();
        }

        public unsafe void Buffer(Shape shape)
        {
            float[] vertices = shape.GetVerticesFloat();
            float[] normals = shape.GetNormalsFloat();
            float[] heights = shape.GetHeightsNormalized().ToArray();
            uint[] triangles = shape.GetTriIndices().ToArray();
            float[] gradientTexture = shape.GetGradientTextureFloat();

            glBindVertexArray(vao);
            glBindBuffer(GL_ARRAY_BUFFER, vbo);

            fixed (void* ptr = &vertices[0]) 
            fixed (void* ptr2 = &normals[0]) 
            fixed (void* ptr3 = &heights[0])
            {
                glBufferData(GL_ARRAY_BUFFER, 3 * sizeof(float) * vertices.Length, null, GL_DYNAMIC_DRAW);
                glBufferSubData(GL_ARRAY_BUFFER, 0, sizeof(float) * vertices.Length, ptr);
                glBufferSubData(GL_ARRAY_BUFFER, sizeof(float) * vertices.Length, sizeof(float) * vertices.Length, ptr2);
                glBufferSubData(GL_ARRAY_BUFFER, 2 * sizeof(float) * vertices.Length, sizeof(float) * vertices.Length/3, ptr3);
            }

            glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, ebo);
            fixed (void* ptr = &triangles[0])
            {
                glBufferData(GL_ELEMENT_ARRAY_BUFFER, sizeof(int) * triangles.Length, ptr, GL_STATIC_DRAW);
            }

            texID = glGenTexture();
            glBindTexture(GL_TEXTURE_1D, texID);
            glTexParameteri(GL_TEXTURE_1D, GL_TEXTURE_MIN_FILTER, GL_LINEAR);
            glTexParameteri(GL_TEXTURE_1D, GL_TEXTURE_MAG_FILTER, GL_LINEAR);
            glTexParameteri(GL_TEXTURE_1D, GL_TEXTURE_WRAP_S, GL_CLAMP_TO_EDGE);
            glTexParameteri(GL_TEXTURE_1D, GL_TEXTURE_WRAP_T, GL_CLAMP_TO_EDGE);
            fixed (void* ptr = &gradientTexture[0])
            {
                glTexImage1D(GL_TEXTURE_1D, 0, GL_RGB16F, gradientTexture.Length / 3, 0, GL_RGB, GL_FLOAT, ptr);
            }

            glVertexAttribPointer(0, 3, GL_FLOAT, false, 3 * sizeof(float), (void*)0);
            glVertexAttribPointer(1, 3, GL_FLOAT, false, 3 * sizeof(float), (void*) (sizeof(float) * vertices.Length));
            glVertexAttribPointer(2, 3, GL_FLOAT, false, sizeof(float), (void*)(2 * sizeof(float) * vertices.Length));
            glEnableVertexAttribArray(0);
            glEnableVertexAttribArray(1);
            glEnableVertexAttribArray(2);
            elements = triangles.Length;
        }

        public unsafe void Render(IEntity planet)
        {
            glUniformMatrix4fv(RenderCore.transformLocation, 1, false, planet.GetTransformMatrix().ToArray());
            glUniformMatrix4fv(RenderCore.rotationLocation, 1, false, planet.GetRotationMatrix().ToArray());
            glBindVertexArray(vao);
            glDrawElements(GL_TRIANGLES, elements, GL_UNSIGNED_INT, null);
        }
    }
}
