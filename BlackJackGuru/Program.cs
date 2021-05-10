using System;
using BlackJackGuru.Drivers;

namespace BlackJackGuru
{
    class Program
    {
        static void Main(string[] args)
        {
            GameSimulation simulation = new GameSimulation();
            simulation.Start();
        }
    }
}
