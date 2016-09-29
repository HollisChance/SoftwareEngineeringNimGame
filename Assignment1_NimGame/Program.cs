using Assignment1_NimGame.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment1_NimGame
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Game game = new Game();
            Game aiTest = new Game();
            game.PlayGame();
            //aiTest.RandomVsLearningAI(10000);
        }
    }
}
