using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blokus
{
    public class BlokusGameState
    {
        public BlokusGameState(byte[] pGameState, IList<IPiece> pPieces, int pColumns, int pRows)
        {
            BlokusBoard = pGameState;
            AvailablePieces = pPieces;
            if (pGameState.Length != pColumns*pRows)
            {
                Console.WriteLine("WARNING: game state length is not same as columns * rows");
                return;
            }
            mColumns = pColumns;
            mRows = pRows;
        }

        private int mColumns, mRows;
        public byte[] BlokusBoard { get; private set; }
        public IList<IPiece> AvailablePieces { get; set; }

        public List<int> GetCorners(int player)
        {
            List<int> availableCorners = new List<int>();

            for (int i = 0; i < BlokusBoard.Length; i++)
            {
                if (BlokusBoard[i] != player)
                {
                    continue;
                }

                List<int> directions = new List<int>{i - 1 - mRows,
                                                     i + 1 - mRows,
                                                     i - 1 + mRows,
                                                     i + 1 + mRows};

                foreach (int direction in directions)
                {
                    availableCorners.AddRange(GetCornersFromCornerBlock(direction));
                }
            }
            return availableCorners;
        }

        private List<int> GetCornersFromCornerBlock(int position)
        {
            if (position != 0)
            {
                return null;
            }

            List<int> directions = new List<int>{position - mRows,
                                                 position + mRows,
                                                 position - 1,
                                                 position + 1};
            
            //foreach (int direction in directions)
            //{
            //    if(direction )
            //}

            return null;
        }

        //private bool IsCornerBlocked(int position)
        //{
        //    if(N % mRows == 0 || )
        //}
    }
}
