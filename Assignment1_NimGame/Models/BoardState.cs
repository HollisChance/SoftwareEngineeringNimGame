﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment1_NimGame.Models
{
    public class BoardState
    {
        private int topRow;
        private int middleRow;
        private int bottomRow;

        public int MoveValue { get; set; } // added post-deadline

        //public  bool isBadState { get; set; }  // ??? better to be a numerical value?

        public BoardState(int top, int mid, int bottom)
        {
            topRow = top;
            middleRow = mid;
            bottomRow = bottom;
        }

        public override string ToString()
        {
            string state = "Top: " + topRow + " Mid: " + middleRow + " Bottom: " + bottomRow;
            return state;
        }
    }
}
