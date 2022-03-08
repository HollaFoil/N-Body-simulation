using GLFW;
using GlmSharp;
using N_Body_simulation.src.Event.Evenets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N_Body_simulation.src.Input
{
    internal static class Input
    {
        private static float sensitivity = 0.05f;
        public static KeyCallback keyCallback = KeyCallback;
        public static MouseCallback mouseCallback = MouseCallback;
        private static int debugInt = 0;
        public static void Init()
        {
            Glfw.SetKeyCallback(Render.Window.GetWindow(), keyCallback);
            Glfw.SetCursorPositionCallback(Render.Window.GetWindow(), mouseCallback);
        }
        private static void KeyCallback(Window window, Keys key, int scancode, InputState state, ModifierKeys modifiers)
        {
            if (window == null) return;

            if (state == InputState.Repeat || !Keybinds.ContainsKey(key))
            {
                Debug(key, state, modifiers);
                return;
            }
            IsKeyPressed[(int)Keybinds[key]] = !IsKeyPressed[(int)Keybinds[key]];
            ThrowEvents(key);

        }
        private static void Debug(Keys key, InputState state, ModifierKeys modifiers)
        {
            if (state != InputState.Press) return;

            float val = 1.1f;
            if (modifiers == ModifierKeys.Control) val = 0.9f;
            //if (modifiers == ModifierKeys.Alt) val *= 0.92f;
            if (key == Keys.Alpha0) debugInt = 0;
            if (key == Keys.Alpha1) debugInt = 1;
            if (key == Keys.Numpad2) debugInt = 2;

            Noise.NoiseFilter[] filters = Program.p1.GetNoiseSettings();

            if (key == Keys.Equal) Program.p1.resolution += (Program.p1.resolution / 10);
            if (key == Keys.Minus) Program.p1.resolution -= (Program.p1.resolution / 10);

            if (key == Keys.Y) filters[debugInt].settings.persistance *= val;
            if (key == Keys.U) filters[debugInt].settings.baseRoughness *= val;
            if (key == Keys.I) filters[debugInt].settings.minvalue *= val;
            if (key == Keys.O) filters[debugInt].settings.roughness *= val;
            if (key == Keys.P) filters[debugInt].settings.strength *= val;

            if (key == Keys.Right) filters[debugInt].settings.center.x *= val;
            if (key == Keys.Down) filters[debugInt].settings.center.z *= val;
            if (key == Keys.Up) filters[debugInt].settings.center.y *= val;

            Program.p1.SetNoiseSettings(filters);
        }
        private static void MouseCallback(Window window, double x, double y)
        {
            if (window == null) return;

            var screen = Glfw.PrimaryMonitor.WorkArea;
            int windowx = screen.X;
            int windowy = screen.Y;
            int width = screen.Width;
            int height = screen.Height;

            float centerx = ((windowx + width / 2) / (float)screen.Width);
            float centery = ((windowy + height / 2) / (float)screen.Height);

            vec2 change = new vec2(centerx * width - (float)x, centery * height - (float)y) * sensitivity;

            Glfw.SetCursorPosition(window, centerx * width, centery * height);
            new MouseMovedEvent(change).Fire();
        }

        //TEMPORARY LOGIC REALLY UGLY NOT EXPANDABLE NEED BETTER IDEA;
        private static void ThrowEvents(Keys key)
        {
            if (Keybinds.Take(6).Any(pair => pair.Key == key))
            {
                new DirectionalKeyPressEvent(GetKeysDirection()).Fire();
            }
        }
        private static Dictionary<Keys, Action> Keybinds = new Dictionary<Keys, Action>()
        {
            {Keys.W, Action.Forward},
            {Keys.A, Action.Left},
            {Keys.S, Action.Backward},
            {Keys.D, Action.Right},
            {Keys.Space, Action.Up},
            {Keys.LeftShift, Action.Down},
            {Keys.Q, Action.Break},
        };
        private static bool[] IsKeyPressed = new bool[Keybinds.Count];
        public static vec3 GetKeysDirection()
        {
            vec3 direction = new vec3(0, 0, 0);
            if (IsKeyPressed[(int)Action.Forward]) direction.z += 1f;
            if (IsKeyPressed[(int)Action.Backward]) direction.z += -1f;
            if (IsKeyPressed[(int)Action.Right]) direction.x += 1f;
            if (IsKeyPressed[(int)Action.Left]) direction.x += -1f;
            if (IsKeyPressed[(int)Action.Up]) direction.y += 1f;
            if (IsKeyPressed[(int)Action.Down]) direction.y += -1f;
            return direction;
        }
        public enum Action : int
        {
            Forward = 0,
            Backward = 1,
            Left = 2,
            Right = 3,
            Down = 4,
            Up = 5,
            Break = 6,
        }
    }
}