using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blokus
{
    public class BlokusGameState : IBlokusGameState
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

        public BlokusGameState(byte[] pGameState)
        {
            BlokusBoard = pGameState;
            AvailablePieces = new List<IPiece>();
            mColumns = (int) Math.Sqrt(pGameState.Length);
            mRows = (int) Math.Sqrt(pGameState.Length);

        }

        public BlokusGameState(byte[] pGameState, IList<IPiece> pPieces)
        {
            BlokusBoard = pGameState;
            AvailablePieces = pPieces;
            mColumns = (int)Math.Sqrt(pGameState.Length);
            mRows = (int)Math.Sqrt(pGameState.Length);
        }

        public IList<BlokusMove> GetAvailableMoves(int playerId)
        {
            IGameValidator validator = ValidatorFactory.GetValidator();
            List<int> corners = GetCorners(playerId);
            IList<BlokusMove> Results = new List<BlokusMove>();

            foreach (IPiece piece in AvailablePieces)
            {
                var rotations = piece.ListRoations();
                foreach (byte[,] rotation in rotations)
                {
                    foreach (int corner in corners)
                    {
                        IList<BlokusMove> moves = PlacePice(rotation, corner);
                        foreach (BlokusMove move in moves)
                        {
                            //!TODO change validator.Validate to use move instead of GameState and dont solve the remove piece in validate
                            if (validator.Validate(playerId, move, this))
                            {
                                Results.Add(move);
                            }
                        }
                    }
                }
            }
            return new List<BlokusMove>();
        }

        public IList<BlokusMove> PlacePice(byte[,] rotation, int corner)
        {
            //!TODO make list of all possible moves 
            return new List<BlokusMove>();
        }


        private int mColumns, mRows;
        public byte[] BlokusBoard { get; private set; }
        public IList<IPiece> AvailablePieces { get; set; }

        public List<int> GetCorners(int pPlayerId)
        {
            List<int> availableCorners = new List<int>();

            for (int i = 0; i < BlokusBoard.Length; i++)
            {
                if (BlokusBoard[i] != pPlayerId)
                {
                    continue;
                }

                int NW = i % mColumns == 0 || i < mColumns ? -1 : i - 1 - mColumns;
                int SW = i % mColumns == 0 || i >= mColumns * mRows - mColumns ? -1 : i - 1 + mColumns;
                int NE = i % mColumns == mColumns - 1 || i < mColumns ? -1 : i + 1 - mColumns;
                int SE = i % mColumns == mColumns - 1 || i >= mColumns * mRows - mColumns ? -1 : i + 1 + mColumns;
                List<int> directions = new List<int>{NW,SW,NE,SE};

                availableCorners.AddRange(directions.Where(a=> IsLegitCorner(a, pPlayerId)));
            }
            return availableCorners.Distinct().ToList();
        }

        private bool IsLegitCorner(int position, int pPlayerId)
        {
            if (position == -1 || BlokusBoard[position] != 0)
            {
                return false;
            }

            int N = position < mColumns ? -1 : position - mColumns;
            int S = position >= mColumns * mRows - mColumns ? -1 : position + mColumns;
            int E = position % mColumns == mColumns - 1 ? -1 : position + 1;
            int W = position % mColumns == 0 ? -1 : position - 1;
            List<int> directions = new List<int>{N,S,E,W};
            return directions.All(direction => direction == -1 || BlokusBoard[direction] != pPlayerId);
        }
    }
}
