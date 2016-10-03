using Assignment1_NimGame.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment1_NimGame.Models
{
 public class Game
    {
        const int _numRows = 3;
        const int row1Size = 3;
        const int row2Size = 5;
        const int row3Size = 7;
        private Row[] _rows;
        private List<BoardState> boardStates = new List<BoardState>(); // maybe this shouldn't be global? just stored on a per game basis(ie in PlayGame)
        Player turn;
        SmartAI compPlayer1;
        SmartAI compPlayer2;
        int player1Wins = 0;
        int player2Wins = 0;
        int numComputerPlayers;
        
        public void PlayGame()
        {
            turn = Player.Player1;
            bool keepPlaying = true;
            compPlayer1 = new SmartAI();
            compPlayer2 = new SmartAI();
            numComputerPlayers = ChooseGameMode();
            while(keepPlaying)
            {
                _rows = new Row[] { new Row(row1Size), new Row(row2Size), new Row(row3Size) };
                bool isGameOver = false;
                while (!isGameOver)
                {
                    printBoard();
                    isGameOver = TakeTurn(numComputerPlayers); // set to one human vs comp
                }
                Console.WriteLine("enter 0 to Quit, or anything else to play again");
                string input = Console.ReadLine();
                if (input.Equals("0"))
                {
                    keepPlaying = false; // exits game
                    Console.WriteLine("Comp1 bad states");
                    compPlayer1.PrintBadStates();
                    Console.WriteLine();
                    Console.WriteLine();
                    Console.WriteLine("Comp2 bad states");
                    compPlayer2.PrintBadStates();

                }
            }
        }

        public void RandomVsLearningAI(int playTimes)
        {
            turn = Player.Player1;
            compPlayer1 = new SmartAI();
            compPlayer2 = new SmartAI();
            numComputerPlayers = 2;
            for (int j = 0; j < playTimes; ++j)
            {
                _rows = new Row[] { new Row(row1Size), new Row(row2Size), new Row(row3Size) };
                bool isGameOver = false;
                while (!isGameOver)
                {
                    printBoard();
                    isGameOver = TakeTurn(numComputerPlayers); 
                }
            }
            Console.WriteLine("Comp1 bad states");
            compPlayer1.PrintBadStates();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Comp2 bad states");
            compPlayer2.PrintBadStates();
            Console.WriteLine("Player1 wins: " + player1Wins + " Player2 wins: " + player2Wins);

        }

        public int ChooseGameMode()
        {
            int numComputerPlayers = PromptForInt("Enter 0 for player vs player, 1 for player vs cpu, or 2 for cpu vs cpu", 0, 2);
            return numComputerPlayers;
        }

        public bool TakeTurn(int compPlayers)
        {
            bool isGameOver = false;
            if (compPlayers == 1)
            {
                if (turn == Player.Player2)
                {
                    isGameOver = TakeComputerTurn(compPlayer2);
                }
                else
                {
                    isGameOver = TakePlayerTurn();
                }
            }
            else if (compPlayers == 2)
            {
                if (turn == Player.Player1)
                {
                    isGameOver = TakeComputerTurn(compPlayer1);                        
                }
                else
                {
                    isGameOver = TakeComputerTurn(compPlayer2);
                }
            }
            else
            {
                isGameOver = TakePlayerTurn();
            }
            return isGameOver;
        }

        public bool TakePlayerTurn()
        {
            bool isGameOver = false;

            int row = PromptForRow(turn + " enter the row you wish to take piece/pieces from");
            int numToRemove = PromptForInt("Enter the number of pieces you wish to remove from row " + row, 1, _rows[row - 1].RowSize);

            isGameOver = MakeMove(row, numToRemove);

            return isGameOver;
        }

        public bool TakeComputerTurn(SmartAI comp)
        {
            int row = 0;
            int numToRemove = 0;
            if (turn == Player.Player2) // computer 2 uses the learning system
            {
                Move move = comp.MakeMove(_rows);
                row = move.Row;
                numToRemove = move.NumToRemove;
            }
            else  // computer 1 doesn't use the learning ai
            {
                row = comp.MakeRowChoice(_rows.Count(), _rows);
                numToRemove = comp.MakeRandomChoice(_rows[row - 1].RowSize);
            }
            Console.WriteLine("Computer " + turn + " takes " + numToRemove + " from row " + row);
            bool isGameOver = MakeMove(row, numToRemove);
            return isGameOver;
        }

        public bool MakeMove(int row, int numToRemove)
        {
            bool isGameOver = false;
            if (_rows[row - 1].RemovePieces(numToRemove))
            {
                ChangeTurn();

                BoardState state = new BoardState(_rows[0].RowSize, _rows[1].RowSize, _rows[2].RowSize);
                boardStates.Add(state); // boardstate is stored here!

                if (CheckForGameOver())
                {
                    Console.WriteLine("Player " + turn + " wins");
                    Console.WriteLine("Player1 wins: " + player1Wins + " Player2 wins: " + player2Wins);

                    IncrementWins();
                    StoreCriticalMoves();
                    isGameOver = true;
                    boardStates.Clear();
                }
            }
            return isGameOver;
        }

        public void StoreCriticalMoves()
        {
            if (numComputerPlayers == 1)
            {
                if (turn == Player.Player1)
                {
                    BoardState losingState = boardStates[(boardStates.Count - 3)]; // third from last move, the move that led to the win/loss
                    losingState.MoveValue += -1;
                    compPlayer2.AddCriticalMove(losingState);
                }
                else if(turn == Player.Player2)
                {
                    BoardState losingState = boardStates[(boardStates.Count - 3)]; // third from last move, the move that led to the win/loss
                    losingState.MoveValue += 1; 
                    compPlayer2.AddCriticalMove(losingState);
                }
            }
            else if (numComputerPlayers == 2)
            {
                if (turn == Player.Player1) // Comp stores last critical move
                {
                    BoardState losingState = boardStates[(boardStates.Count - 3)];
                    losingState.MoveValue += -1;
                    compPlayer2.AddCriticalMove(losingState);
                }
                else if (turn == Player.Player2) // if the winner is player 2, comp 1 stores the losing move
                {
                    BoardState losingState = boardStates[(boardStates.Count - 3)];
                    losingState.MoveValue += 1;
                    compPlayer2.AddCriticalMove(losingState);
                }
            }
        }

        public void IncrementWins()
        {
            if (turn == Player.Player1)
            {
                ++player1Wins;
            }
            else
            {
                ++player2Wins;
            }
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
                row = PromptForInt(turn + " enter the row you wish to take piece/pieces from", 1, _rows.Count());
                if (_rows[row - 1].RowSize >= 1)
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
