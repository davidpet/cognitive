using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonLib;

namespace MemoryPractice
{
    class Program
    {
        private static void playGame(MemoryList list, string filename, bool dumpOnly)
        {
            try
            {
                list.Load(filename);
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR: " + e.Message);
                return;
            }

            if (dumpOnly)
            {
                list.Dump();
                return;
            }

            list.Begin();
            while (list.HasMoreValues)
            {
                Console.Write(list.GetNextChallenge() + ": ");
                string guess = Console.ReadLine().Trim();

                if (!list.TryResponse(guess))
                    Console.WriteLine("!!!WRONG!!! - " + list.LastRightAnswer);
            }

            Console.WriteLine("-------------------------------------");
            Console.WriteLine(list.Correct + "/" + list.Total + " (" + (100*list.Correct/list.Total) + "%)");
            Console.WriteLine("-------------------------------------");
        }

        //TODO: encapsulate this better
        private static void processPlayStyle(object menu, object choice)
        {
            Configuration config = (Configuration)choice;

            MemoryList list = new MemoryList();
            list.Order = config.Order;
            list.Polarity = config.Polarity;
            playGame(list, ((PlayStyleMenu)menu).ListFile, config.DumpOnly);
        }

        //todo: better display of multi-level menus
        private static void processList(object ignored, object filename)
        {
            PlayStyleMenu menu = new PlayStyleMenu((string)filename);

            menu.ChoiceMade += processPlayStyle;
            menu.Execute();
        }

        static void Main(string[] args)
        {
            FolderMenu listMenu = new FolderMenu(@"D:\Programming\Solutions\General\MemoryPractice\Lists");
            listMenu.ChoiceMade += processList;

            listMenu.Execute();
        }
    }
}
