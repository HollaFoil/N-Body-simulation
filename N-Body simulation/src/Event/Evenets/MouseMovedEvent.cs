using GlmSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N_Body_simulation.src.Event.Evenets
{
    internal class MouseMovedEvent : IEvent<MouseMovedEvent>
    {
        private vec2 change;
        public MouseMovedEvent(vec2 change)
        {
            this.change = change;
        }
        public vec2 GetChange()
        {
            return change;
        }
        protected override MouseMovedEvent GetEvent()
        {
            return this;
        }
    }
}
