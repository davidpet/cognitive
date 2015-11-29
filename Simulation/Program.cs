using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonLib;

namespace Simulation
{
    class Program
    {
        static void Main(string[] args)
        {
            Menu menu = new Menu("Simulation");
            menu.AddChoice("3 Doors", new ThreeDoorsSimulator());
            menu.ChoiceMade += (ignored, choice) =>
            {
                ISimulator simulator = (ISimulator)choice;
                simulator.SolicitValues();      //todo: improve the weird cross-dependency here
                simulator.Simulate();           //todo: possibly make simulator not know about console
            };
            menu.Execute();
        }
    }
}
