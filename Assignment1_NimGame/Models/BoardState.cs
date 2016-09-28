using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment1_NimGame.Models
{
    class BoardState
    {
        private int topRow;
        private int middleRow;
        private int bottomRow;

        public  bool isBadState { get; set; }  // ??? better to be a numerical value?

        public BoardState(int top, int mid, int bottom)
        {
            topRow = top;
            middleRow = mid;
            bottomRow = bottom;
        }
    }
}
