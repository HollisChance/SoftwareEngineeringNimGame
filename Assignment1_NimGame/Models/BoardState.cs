using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment1_NimGame.Models
{
    public class BoardState
    {
        private const int maxMoveValue = 100;
        private const int minMoveValue = -100;
        private int topRow;
        private int middleRow;
        private int bottomRow;
        private int _moveValue;

        public int MoveValue {
            get{ return _moveValue; }
            set
            {
                if (_moveValue >= 100 || _moveValue <= -100 )
                {
                    // do nothing
                }
                else
                {
                    _moveValue = value;
                }
            }
        } 


        //public  bool isBadState { get; set; }  // ??? better to be a numerical value?

        public BoardState(int top, int mid, int bottom)
        {
            topRow = top;
            middleRow = mid;
            bottomRow = bottom;
        }

        public override string ToString()
        {
            string state = "Top: " + topRow + " Mid: " + middleRow + " Bottom: " + bottomRow + " Value: " + MoveValue;
            return state;
        }

        public override bool Equals(object obj)
        {
            bool isEqual = false;
            BoardState state = (BoardState)obj;
            if (this.topRow == state.topRow && this.middleRow == state.middleRow && this.bottomRow == state.bottomRow)
            {
                isEqual = true;
            }
            return isEqual;
        }
    }
}
