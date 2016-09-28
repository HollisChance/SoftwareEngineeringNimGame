using Assignment1_NimGame.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment1_NimGame.Models
{
    class Game
    {
        const int _numRows = 3;
        const int row1Size = 3;
        const int row2Size = 5;
        const int row3Size = 7;
        private Row[] _rows;
        private List<BoardState> boardStates = new List<BoardState>(); // maybe this shouldn't be global? just stored on a per game basis(ie in PlayGame)
        Player turn;
        
        public void PlayGame()
        {
            turn = Player.Player1;
            bool keepPlaying = true;
            while(keepPlaying)
            {
                _rows = new Row[] { new Row(row1Size), new Row(row2Size), new Row(row3Size) };
                bool isGameOver = false;
                while (!isGameOver)
                {
                    printBoard();
                    isGameOver = TakeTurn();
                }
                Console.WriteLine("enter 0 to Quit, or anything else to play again");
                string input = Console.ReadLine();
                if (input.Equals("0"))
                {
                    keepPlaying = false; // exits game
                }
            }
        }

        public bool TakeTurn()
        {
            bool isGameOver = false;
            int row = PromptForRow(turn + " enter the row you wish to take piece/pieces from");
            int numToRemove = PromptForInt("Enter the number of pieces you wish to remove from row " + row, 1, _rows[row - 1].RowSize);
            
            if (_rows[row - 1].RemovePieces(numToRemove))
            {
                ChangeTurn();
                // store board state here???
                if (CheckForGameOver())
                {
                    Console.WriteLine("Player " + turn + " wins");
                    isGameOver = true;
                }
            }
            return isGameOver;
        }

        public bool CheckForGameOver()
        {
            bool isGameOver = true;
            for (int j = 0; j < _rows.Count(); ++j)
            {
                if (_rows[j].RowSize != 0)
                {
                    isGameOver = false;
                    break;
                }
            }
            return isGameOver;
        }

        public void ChangeTurn()
        {
            if (turn.Equals(Player.Player1))
            {
                turn = Player.Player2;
            }
            else
            {
                turn = Player.Player1;
            }
        }

        public void printBoard()
        {
            for (int j = 0; j < _rows.Count(); ++j)
            {
                _rows[j].printRow();
            }
        }

        public int PromptForRow(string message)
        {
            bool isValid = false;
            int row = 1;
            while (!isValid)
            {
                row = PromptForInt(turn + " enter the row you wish to take piece/pieces from", 0, _rows.Count());
                if (_rows[row - 1].RowSize > 0)
                {
                    isValid = true;
                }
                else
                {
                    Console.WriteLine("That row has no pieces, pick a different row");
                }
            }
            return row;
        }

        public int PromptForInt(string message, int min, int max)
        {
            bool isValid = false;
            int result = 0;

            while (!isValid)
            {
                Console.WriteLine(message);
                string input = Console.ReadLine();
                isValid = int.TryParse(input, out result) && result >= min && result <= max;

                if (!isValid)
                {
                    Console.WriteLine("You must enter a valid integer value between " + min + " and " + max);
                }
            }
            return result;
        }
        
    }
}
