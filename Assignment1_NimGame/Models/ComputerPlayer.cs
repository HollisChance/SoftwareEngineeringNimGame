using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment1_NimGame.Models
{
    public class ComputerPlayer
    {
        Random rnd = new Random();

        private List<BoardState> badStates = new List<BoardState>();

        public int MakeRowChoice(int max, Row[] rows)
        {
            int choice = 1;
            while (rows[choice - 1].RowSize == 0)
            {
                choice = MakeRandomChoice(max);
            }
            return choice;
        }

        public Move MakeLearnedMove(Row[] rows)
        {
            int row = 1;
            int numToRem = 1;
            int moveAttempts = 1;
            bool isGoodMove = false;
            Move move = new Move { Row = row, NumToRemove = numToRem };
            while (!isGoodMove && moveAttempts < 25)
            {
                Row[] testRows = new Row[] { new Row(rows[0].RowSize), new Row(rows[1].RowSize), new Row(rows[2].RowSize) };

                row = MakeRowChoice(rows.Count(), rows);
                Console.WriteLine("Row sizes: " + rows[0].RowSize + " " + rows[1].RowSize + " " + rows[2].RowSize + " ");
                numToRem = MakeRandomChoice(rows[row - 1].RowSize);

                testRows[row - 1].RowSize -= numToRem;

                BoardState projectedState = new BoardState(testRows[0].RowSize, testRows[1].RowSize, testRows[2].RowSize);

                isGoodMove = TestMove(projectedState);
            }
            move = new Move { Row = row, NumToRemove = numToRem };

            return move;
        }

        public bool TestMove(BoardState projectedState)
        {
            bool isGood = true;
            foreach(BoardState badState in badStates)
            {
                if (projectedState == badState)
                {
                    isGood = false;
                    break;
                }
            }
            return isGood;
        }

        public int MakeRandomChoice(int max)
        {
            int choice = rnd.Next(1, max + 1);
            return choice;
        }

        public void addLosingMove(BoardState losingState)
        {
            badStates.Add(losingState);
            Console.WriteLine(badStates.Last());
        }

        public void PrintBadStates()
        {
            foreach (BoardState state in badStates)
            {
                Console.WriteLine(state);
            }
        }

    }
}
