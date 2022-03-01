﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static OpenGL.GL;

namespace N_Body_simulation.src.Render.Shaders
{
    internal class Shader
    {
        int type;
        uint shader;
        string fileName;
        string source;
        public Shader(int type, string path)
        {
            this.type = type;
            this.fileName = path;
            shader = glCreateShader(type);
            Stream imgStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(
    "N_Body_simulation.src.Assets.Shaders." + path);
            source = new StreamReader(imgStream, Encoding.UTF8).ReadToEnd();

            glShaderSource(shader, source);
            glCompileShader(shader);

            int success = glGetShaderiv(shader, GL_COMPILE_STATUS, 1)[0];
            if (success == GL_FALSE)
            {
                Console.Write("ERROR::SHADER::" + fileName + "::COMPILATION_FAILED\n" + glGetShaderInfoLog(shader));
            }
        }
        public uint GetShader()
        {
            return shader;
        }
        public void Free()
        {
            glDeleteShader(shader);
        }
    }
}
