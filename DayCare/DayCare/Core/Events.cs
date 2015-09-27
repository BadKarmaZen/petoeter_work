using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayCare.Core
{
    public class Events
    {
        public class Close {}

        public class SwitchTask
        {
            public Screen Task { get; set; }
        }

        public class ShowDialog 
        {
            public Screen Dialog { get; set; }
        }
    }
}
