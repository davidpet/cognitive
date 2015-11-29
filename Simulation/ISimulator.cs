using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Simulation
{
    interface ISimulator
    {
        void SolicitValues();
        void Simulate();
    }
}
