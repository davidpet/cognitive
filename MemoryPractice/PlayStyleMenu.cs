using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonLib;

namespace MemoryPractice
{
    class Configuration
    {
        public Configuration(Order order, Polarity polarity, bool dumpOnly = false)
        {
            Order = order;
            Polarity = polarity;
            DumpOnly = dumpOnly;
        }

        public Order Order;
        public Polarity Polarity;
        public bool DumpOnly;
    }

    class PlayStyleMenu: Menu
    {
        public PlayStyleMenu(string listFile) :
            base("Test Methodology")
        {
            AddChoice("DumpList", new Configuration(Order.Forward, Polarity.OrdinalFirst, true));
            AddChoice("Forward by Number", new Configuration(Order.Forward, Polarity.OrdinalFirst));
            AddChoice("Backward by Number", new Configuration(Order.Backwards, Polarity.OrdinalFirst));
            AddChoice("Random by Number", new Configuration(Order.Random, Polarity.OrdinalFirst));
            AddChoice("Forward by Object", new Configuration(Order.Forward, Polarity.ValueFirst));
            AddChoice("Backward by Object", new Configuration(Order.Backwards, Polarity.ValueFirst));
            AddChoice("Random by Object", new Configuration(Order.Random, Polarity.ValueFirst));

            ListFile = listFile;
        }

        public string ListFile { get; set; }
    }
}
