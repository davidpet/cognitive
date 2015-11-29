using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonLib;

namespace Simulation
{
    class ThreeDoorsSimulator: ISimulator
    {
        private void runOneSimulation(Random random, out bool shouldHaveSwitched)
        {
            //create the doors
            bool[] doors = new bool[3];
            for (int i = 0; i < 3; i++)
                doors[i] = false;       //todo: verify that this is done by .NET already and remove if so

            //pick a door for the prize
            doors[random.Next(0, 3)] = true;

            //pick a random door for the player
            int playerDoor = random.Next(0, 3);
            //pick a random door for the host to remove
            uint openDoor = 0;
            if (doors[playerDoor] == false) //only 1 possibility if player has the wrong door
            {
                for (int i = 0; i < 3; i++)
                    if (i != playerDoor && doors[i] == false)
                    {
                        openDoor = (uint)i;
                        break;
                    }
            }
            else //randomly choose between two if player has the right door
            {
                List<int> possibleDoors = new List<int>();
                for (int i = 0; i < 3; i++)
                    if (i != playerDoor)
                        possibleDoors.Add(i);
                openDoor = (uint)possibleDoors[random.Next(0, 2)];
            }

            //determine the ideal action for the player
            shouldHaveSwitched = !doors[playerDoor];
        }

        public virtual void Simulate()
        {
            Random random = new Random();
            uint usersThatShouldHaveSwitched = 0;

            for (uint i = 0; i < numValues; i++)
            {
                Console.WriteLine("Simulation #" + (i+1).ToString());

                bool shouldHaveSwitched = false;
                runOneSimulation(random, out shouldHaveSwitched);

                if (shouldHaveSwitched)
                    usersThatShouldHaveSwitched++;
            }

            double ratio = (double)usersThatShouldHaveSwitched / (numValues - usersThatShouldHaveSwitched);
            Console.WriteLine("--------------------------");    //todo: move into menu object
            Console.WriteLine("Ratio (switch:stay): " + ratio.ToString());
        }

        public virtual void SolicitValues()
        {
            numValues = Menu.SolicitValue("Number of Iterations");
        }

        private uint numValues = 1000;
    }
}
