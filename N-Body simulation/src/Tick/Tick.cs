using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N_Body_simulation.src.Tick
{
    internal static class Tick
    {
        static Tick()
        {

        }
        private static void OnPreUpdate()
        {
            PreUpdate?.Invoke();
        }
        private static void OnUpdate()
        {
            Update?.Invoke();
        }
        private static void OnPostUpdate()
        {
            PostUpdate?.Invoke();
        }
        public static void DoTick()
        {
            OnPreUpdate();
            OnUpdate();
            OnPostUpdate();
        }

        public delegate void PreUpdateDelegate();
        public delegate void UpdateDelegate();
        public delegate void PostUpdateDelegate();
        public static PreUpdateDelegate PreUpdate;
        public static UpdateDelegate Update;
        public static PostUpdateDelegate PostUpdate;
    }
}
