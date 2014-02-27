using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;
using Blokus.Model;
using log4net;

namespace Blokus
{
    public class BlokusGameState : IBlokusGameState
    {
        private static ILog sLogger = LogManager.GetLogger(typeof(BlokusGameState));

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
            DateTime dt1 = DateTime.Now;
            List<int> corners = GetCorners(playerId);
            //sLogger.DebugFormat("Get corners: {0}ms", (DateTime.Now - dt1).TotalMilliseconds);
            IList<BlokusMove> results = new List<BlokusMove>();

            //dt1 = DateTime.Now;
            foreach (IPiece piece in AvailablePieces)
            //Parallel.ForEach(AvailablePieces, piece =>
            {
                DateTime dt2 = DateTime.Now;
                var rotations = piece.Rotations;
                //sLogger.DebugFormat("       Getting rotations: {0}ms", (DateTime.Now - dt2).TotalMilliseconds);
                dt2 = DateTime.Now;
                Parallel.ForEach(rotations, rotation =>
                //foreach (byte[,] rotation in rotations)
                {
                    foreach (int corner in corners)
                    {
                        IList<BlokusMove> moves = PlacePiece(rotation, corner, playerId);
                        foreach (BlokusMove move in moves)
                        {
                            lock (results)
                            {
                                results.Add(move);
                            }
                        }
                    }
                });
                //sLogger.DebugFormat("       Finish all rotations: {0}ms", (DateTime.Now - dt2).TotalMilliseconds);

            };
            //sLogger.DebugFormat("Get Available pieces: {0}ms", (DateTime.Now - dt1).TotalMilliseconds);

            return results;
        }

        public IList<BlokusMove> PlacePiece(byte[,] rotation, int corner, int playerId)
        {
            int cornerX = corner%mRows;
            int cornerY = (corner - cornerX)/mRows;
            Point cornerPoint = new Point(cornerX,cornerY);
            List<BlokusMove> availableMoves = new List<BlokusMove>();

            for (int x = 0; x <= rotation.GetUpperBound(0); x++)
            {
                for (int y = 0; y <= rotation.GetUpperBound(1); y++)
                {
                    if (rotation[x, y] == 9)
                    {
                        BlokusMove move = TryPlacePieceOnCorner(rotation, cornerPoint, new Point(x,y), playerId);
                        if (move != null)
                        {
                            availableMoves.Add(move);
                        }
                    }
                }
            }
            return availableMoves;
        }

        public BlokusMove TryPlacePieceOnCorner(byte[,] rotation, Point targetPoint, Point piecePoint, int playerId)
        {
            IGameValidator validator = ValidatorFactory.GetValidator();
            int offsetX = targetPoint.X - piecePoint.X;
            int offsetY = targetPoint.Y - piecePoint.Y;

            BlokusMove move = new BlokusMove(BlokusBoard);

            for (int x = 0; x <= rotation.GetUpperBound(0); x++)
            {
                for (int y = 0; y <= rotation.GetUpperBound(1); y++)
                {
                    if (rotation[x, y] != 0)
                    {
                        int tempX = offsetX + x;
                        int tempY = offsetY + y;

                        if(tempX >= mColumns || tempX < 0 || tempY >= mRows || tempY < 0)
                        {
                            return null;
                        }

                        move.BlokusBoard[tempX + tempY * mRows] = (byte)playerId;
                    }
                }
            }
            move.Piece = Piece.GetPiece(rotation);// new Piece(rotation);
            return validator.Validate(playerId, move, this) ? move : null;
        }

        private byte[,] Get2DBoard()
        {
            byte[,] board = new byte[mColumns,mRows];

            for (int x = 0; x < mColumns; x++)
            {
                for (int y = 0; y < mRows; y++)
                {
                    board[x, y] = BlokusBoard[x + y*mRows];
                }
            }
            return board;
        }

        private int mColumns, mRows;
        public byte[] BlokusBoard { get; private set; }
        public IList<IPiece> AvailablePieces { get; set; }

        public List<int> GetCorners(int pPlayerId)
        {
            List<int> availableCorners = new List<int>();

            if (!BlokusBoard.Any(a => a == pPlayerId))
            {
                switch (pPlayerId)
                {
                    case 1:
                        availableCorners.Add(0);
                        break;
                    case 2:
                        availableCorners.Add(19);
                        break;
                    case 3:
                        availableCorners.Add(399);
                        break;
                    case 4:
                        availableCorners.Add(380);
                        break;
                }
            }
            else
            {
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
                    List<int> directions = new List<int> { NW, SW, NE, SE };

                    availableCorners.AddRange(directions.Where(a => IsLegitCorner(a, pPlayerId)));
                }
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
