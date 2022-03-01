using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N_Body_simulation.src.Tick
{
    public class ITickable
    {
        public ITickable()
        {
            Init();
        }
        private void Init()
        {
            Tick.PreUpdate += PreUpdate;
            Tick.Update += Update;
            Tick.PostUpdate += PostUpdate;
            OnInit();
        }
        public void Disable()
        {
            Tick.PreUpdate -= PreUpdate;
            Tick.Update -= Update;
            Tick.PostUpdate -= PostUpdate;
            OnDisable();
        }
        virtual protected void OnDisable()
        {

        }
        virtual protected void OnInit()
        {

        }
        virtual protected void PreUpdate()
        {

        }
        virtual protected void Update()
        {

        }
        virtual protected void PostUpdate()
        {

        }
    }
}
