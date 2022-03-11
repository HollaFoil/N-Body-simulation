using GLFW;
using GlmSharp;
using N_Body_simulation.src;
using N_Body_simulation.src.Entity;
using N_Body_simulation.src.Input;
using N_Body_simulation.src.Render;
using N_Body_simulation.src.Tick;
using System;
using static OpenGL.GL;
using Timer = N_Body_simulation.src.Util.Timer;
using Window = N_Body_simulation.src.Render.Window;

namespace N_Body_simulation 
{
    internal class Program
    {
        public static Camera Camera;
        static Timer frameTimer;
        const int TargetFramerate = 60;
        public static Earth p1;
        static void Main()
        {
            Init();
            
            RenderCore.BufferEntity(p1);
            while (!Glfw.WindowShouldClose(Window.GetWindow()))
            {
                GLFW.Glfw.PollEvents();
                if (frameTimer.Next(out int elapsedMilliseconds))
                {
                    Tick.DoTick();
                    Camera.Update(elapsedMilliseconds);
                    Console.WriteLine(Camera.GetPosition());
                    RenderCore.Flush();

                }
            }
            //Thread.Sleep(10000);
        }

        static private void Init()
        {
            p1 = new Earth();
            p1.SetPosition(new vec3(-50, 70f, -50));

            Window.Init();
            Input.Init();
            Camera = new Camera();
            frameTimer = new Timer(TargetFramerate);
        }
    }
}