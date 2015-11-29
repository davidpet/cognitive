using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MemoryPractice
{
    enum Order
    {
        Forward,
        Backwards,
        Random
    }

    enum Polarity
    {
        OrdinalFirst,
        ValueFirst
    }

    class MemoryList
    {
        public MemoryList()
        {
            Ordinals = new List<string>();
            Values = new List<string>();
            AlreadyMissed = new List<bool>();
        }

        //should only throw I/O related exceptions
        public void Load(string filename)
        {
            Ordinals.Clear();
            Values.Clear();
            AlreadyMissed.Clear();
            Cursor = -1;

            StreamReader reader = new StreamReader(filename);
            string line = reader.ReadLine();
            while (line != null)
            {
                if (!line.Trim().StartsWith("#"))
                {
                    int tabIndex = line.IndexOf('\t');
                    if (tabIndex != -1)
                    {
                        string ordinal = line.Substring(0, tabIndex).Trim();
                        string value = line.Substring(tabIndex).Trim();
                        if (ordinal != "" && value != "")
                        {
                            Ordinals.Add(ordinal);
                            Values.Add(value);
                            AlreadyMissed.Add(false);
                        }
                    }
                }
                line = reader.ReadLine();
            }

            reader.Close();
        }

        public void Dump()
        {
            for (int i = 0; i < Ordinals.Count; i++)
                Console.WriteLine(Ordinals[i] + ":\t" + Values[i]);
        }

        public void Begin()
        {
            switch (Order)
            {
                case MemoryPractice.Order.Forward:
                    Cursor = 0;
                    break;
                case MemoryPractice.Order.Backwards:
                    Cursor = Ordinals.Count - 1;
                    break;
                default:
                    Cursor = random.Next(Ordinals.Count);
                    break;
            }

            LastRightAnswer = "";
            initialCount = Ordinals.Count;
            missCount = 0;

            for (int i = 0; i < initialCount; i++)
                AlreadyMissed[i] = false;
        }

        public string GetNextChallenge()
        {
            return Polarity == MemoryPractice.Polarity.OrdinalFirst ? Ordinals[Cursor] : Values[Cursor];
        }

        public bool TryResponse(string response)
        {
            bool ret = false;

            string expected = (Polarity == MemoryPractice.Polarity.OrdinalFirst ? Values[Cursor] : Ordinals[Cursor]);
            if (expected.ToLower() == response.ToLower())
            {
                ret = true;
                Ordinals.RemoveAt(Cursor);
                Values.RemoveAt(Cursor);
                AlreadyMissed.RemoveAt(Cursor);
            }
            else
            {
                if (!AlreadyMissed[Cursor])
                    missCount++;
                AlreadyMissed[Cursor] = true;
            }
            LastRightAnswer = expected;

            switch (Order)
            {
                case MemoryPractice.Order.Forward:
                    //if (!ret)
                      //  Cursor++;
                    break;
                case MemoryPractice.Order.Backwards:
                    if (ret)
                        Cursor--;
                    break;
                default:
                    Cursor = random.Next(Ordinals.Count);
                    break;
            }

            return ret;
        }

        public bool HasMoreValues { get { return Ordinals.Count > 0; } }
        public string LastRightAnswer { get; set; }

        public Order Order { get; set; }
        public Polarity Polarity { get; set; }

        public int Correct { get { return initialCount - missCount;}}
        public int Total { get { return initialCount; } }

        protected List<string> Ordinals { get; set; }
        protected List<string> Values { get; set; }
        protected List<bool> AlreadyMissed { get; set; }

        private Random random = new Random();

        protected int Cursor = -1;

        private int initialCount;
        private int missCount;
    }
}
