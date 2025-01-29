using System;
using System.Collections.Generic;
using System.Linq;

namespace AntsProject
{
    class Program
    {
        public static void Main(string[] args)
        {
            var game = Game.Instance;
            game.Initialize();
            game.RunSimulation();
        }
    }
}
