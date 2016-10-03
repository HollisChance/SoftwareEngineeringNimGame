using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment1_NimGame.Models
{
    public class RandomAI : NimPlayer
    {
        Random rnd = new Random();
        private List<BoardState> MoveStates = new List<BoardState>();

        public override Move MakeMove(Row[] rows)
        {
            int row = MakeRowChoice(rows.Count(), rows);
            int numToRemove = MakeRandomChoice(rows[row].RowSize);
            Move move = new Move { Row = row, NumToRemove = numToRemove };
            return move;
        }

        public int MakeRowChoice(int max, Row[] rows)
        {
            int choice = 1;
            while (rows[choice - 1].RowSize == 0)
            {
                choice = MakeRandomChoice(max);
            }
            return choice;
        }

        public int MakeRandomChoice(int max)
        {
            int choice = rnd.Next(1, max + 1);
            return choice;
        }

        public void PrintBadStates()
        {
            foreach (BoardState state in MoveStates)
            {
                Console.WriteLine(state);
            }
        }
    }
}
