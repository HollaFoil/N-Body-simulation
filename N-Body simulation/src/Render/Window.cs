using GLFW;
using static OpenGL.GL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N_Body_simulation.src.Render
{
    internal static class Window
    {
        private static GLFW.Window window;
        private static string Title = "Voxel Engine";
        private static int width = 1920, height = 1080;
        public static void Init()
        {
            PrepareContext();
            CreateWindow(width, height);
        }
        public static void PrepareContext()
        {
            Glfw.WindowHint(Hint.ClientApi, ClientApi.OpenGL);
            Glfw.WindowHint(Hint.ContextVersionMajor, 3);
            Glfw.WindowHint(Hint.ContextVersionMinor, 3);
            Glfw.WindowHint(Hint.OpenglProfile, Profile.Core);
            Glfw.WindowHint(Hint.Doublebuffer, true);
            Glfw.WindowHint(Hint.Decorated, true);
            Glfw.WindowHint(Hint.OpenglDebugContext, true);
        }
        public static GLFW.Window GetWindow()
        {
            return window;
        }
        public static void CreateWindow(int width, int height)
        {
            window = Glfw.CreateWindow(width, height, Title, GLFW.Monitor.None, GLFW.Window.None);
            var screen = Glfw.PrimaryMonitor.WorkArea;
            var x = (screen.Width - width) / 2;
            var y = (screen.Height - height) / 2;
            Glfw.SetWindowPosition(window, x, y);

            Glfw.MakeContextCurrent(window);
            Import(Glfw.GetProcAddress);
            glViewport(0, 0, width, height);

            Glfw.SetInputMode(window, InputMode.Cursor, (int)CursorMode.Hidden);
        }
    }
}
