using GLFW;
using N_Body_simulation.src;
using N_Body_simulation.src.Entity;
using N_Body_simulation.src.Input;
using N_Body_simulation.src.Render;
using System;
using static OpenGL.GL;
using Timer = N_Body_simulation.src.Util.Timer;
using Window = N_Body_simulation.src.Render.Window;

namespace N_Body_simulation // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        public static Camera _camera;
        static Timer frameTimer;
        const int TargetFramerate = 60;
        public static Planet p = new Planet();
        static void Main()
        {
            Init();
            
            RenderCore.BufferPlanet(p);
            while (!Glfw.WindowShouldClose(Window.GetWindow()))
            {
                //Console.WriteLine("working");
                GLFW.Glfw.PollEvents();
                if (frameTimer.Next(out int elapsedMilliseconds))
                {
                    _camera.Update(elapsedMilliseconds);
                    RenderCore.Flush();
                    Console.WriteLine(_camera.GetPosition());
                    Console.WriteLine(_camera.GetYawPitch());
                }
            }
            //Thread.Sleep(10000);
        }

        static private void Init()
        {
            Window.Init();
            Input.Init();
            _camera = new Camera();
            frameTimer = new Timer(TargetFramerate);
        }
    }
}