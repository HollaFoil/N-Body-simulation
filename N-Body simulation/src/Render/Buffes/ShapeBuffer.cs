﻿using N_Body_simulation.src.Geometry;
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
        private uint vao, vbo, ebo;
        private int Elements = 0;

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
            uint[] triangles = shape.GetTriIndices().ToArray();

            glBindVertexArray(vao);
            glBindBuffer(GL_ARRAY_BUFFER, vbo);

            fixed (void* ptr = &vertices[0]) fixed (void* ptr2 = &normals[0])
            {
                glBufferData(GL_ARRAY_BUFFER, 2 * sizeof(float) * vertices.Length, null, GL_DYNAMIC_DRAW);
                glBufferSubData(GL_ARRAY_BUFFER, 0, sizeof(float) * vertices.Length, ptr);
                glBufferSubData(GL_ARRAY_BUFFER, sizeof(float) * vertices.Length, sizeof(float) * vertices.Length, ptr2);
            }

            glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, ebo);
            fixed (void* ptr = &triangles[0])
            {
                glBufferData(GL_ELEMENT_ARRAY_BUFFER, sizeof(int) * triangles.Length, ptr, GL_STATIC_DRAW);
            }



            glVertexAttribPointer(0, 3, GL_FLOAT, false, 3 * sizeof(float), (void*)0);
            glVertexAttribPointer(1, 3, GL_FLOAT, false, 3 * sizeof(float), (void*) (sizeof(float) * vertices.Length));
            glEnableVertexAttribArray(0);
            glEnableVertexAttribArray(1);
            Elements = triangles.Length;
        }

        public unsafe void Render()
        {
            glBindVertexArray(vao);
            glDrawElements(GL_TRIANGLES, Elements, GL_UNSIGNED_INT, null);
        }
    }
}
