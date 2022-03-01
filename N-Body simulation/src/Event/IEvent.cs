using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N_Body_simulation.src.Event
{
    public abstract class IEvent<T> where T : class
    {
        public delegate void CallBack(T e);
        public static event CallBack? Listeners;
        protected abstract T GetEvent();
        public void Fire()
        {
            Listeners?.Invoke(GetEvent());
        }
    }
}
