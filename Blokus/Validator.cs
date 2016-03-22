using System;
using System.Collections.Generic;
using System.Linq;
using log4net;

namespace Blokus
{
    public interface IGameValidator
    {
        bool Validate(int playerId, BlokusMove move, BlokusGameState oldstate);

    }

    public class ValidatorFactory
    {
        public static IGameValidator GetValidator()
        {
            return new GameValidator();
        }
    }

    internal class GameValidator : IGameValidator
    {
        private static ILog sLogger = LogManager.GetLogger(typeof(BlokusGameState));

        public bool Validate(int playerId, BlokusMove move, BlokusGameState oldstate)
        {
            DateTime dt = DateTime.Now;
            TimeSpan t3 = new TimeSpan();
            try
            {
                bool isCorrect = IsCorrectPlayerOnEmptySpace(playerId, move, oldstate);
                bool isCorner = IsCornerToCorner(playerId, move, oldstate);
                if (isCorrect && isCorner)
                {
                    DateTime d = DateTime.Now;
                    bool checkPlacement = CheckPiecePlacement(move, oldstate);
                    t3 = (DateTime.Now - d);
                    return checkPlacement;
                }
                return false;
            }
            finally
            {
                double d = (DateTime.Now - dt).TotalMilliseconds;
                if (d > 4)
                {
                    sLogger.DebugFormat("       Finish validating: {0}ms", (DateTime.Now - dt).TotalMilliseconds);
                    sLogger.DebugFormat("         checkPlacement: {0}ms", t3.TotalMilliseconds);
                }

            }
        }

        public bool IsCorrectPlayerOnEmptySpace(int playerId, BlokusMove move, BlokusGameState oldState)
        {
            List<byte> diff = move.BlokusBoard.Select((a, i) => (byte)(a - oldState.BlokusBoard[i])).ToList();
            return diff.All(a => a == 0 || a == playerId);
        }

        public bool IsCornerToCorner(int playerId, BlokusMove move, BlokusGameState oldState)
        {
            int howManyPer = (int)Math.Sqrt(move.BlokusBoard.Length);
            var diff = move.BlokusBoard.Select((a, i) => new
            {
                X = i % howManyPer,
                Y = i / howManyPer,
                Value = (byte)(a - oldState.BlokusBoard[i])
            }).Where(a => a.Value == playerId);

            bool hasAtLeastOneCorner = false;

            // First piece
            if (oldState.AvailablePieces.Count == 21)
            {
                return GetStartCorner(playerId, move);
            }

            foreach (var coord in diff)
            {
                for (int x = -1; x <= 1; x++)
                {
                    for (int y = -1; y <= 1; y++)
                    {
                        int tempx = coord.X + x;
                        int tempy = coord.Y + y;

                        if (tempx >= 0 && tempx < howManyPer && tempy >= 0 && tempy < howManyPer)
                        {
                            int newIndex = tempx + tempy * howManyPer;
                            byte oldValue = oldState.BlokusBoard[newIndex];
                            if (oldValue == playerId)
                            {
                                if (x == 0 || y == 0)
                                {
                                    // samside
                                    return false;
                                }

                                // corner
                                hasAtLeastOneCorner = true;
                            }
                        }
                    }
                }
            }
            return hasAtLeastOneCorner;
        }

        public static bool GetStartCorner(int playerId, BlokusMove move)
        {
            switch (playerId)
            {
                case 1:
                    return (move.BlokusBoard[0] == playerId);
                case 2:
                    return (move.BlokusBoard[19] == playerId);
                case 3:
                    return (move.BlokusBoard[move.BlokusBoard.Length - 1] == playerId);
                case 4:
                    return (move.BlokusBoard[move.BlokusBoard.Length - 20] == playerId);
                default:
                    return (move.BlokusBoard[0] == playerId);
            }
        }

        public bool CheckPiecePlacement(BlokusMove move, BlokusGameState oldstate)
        {
            byte[] changes = new byte[400];
            for (int i = 0; i < 400; i++)
            {
                if (oldstate.BlokusBoard[i] != move.BlokusBoard[i])
                {
                    changes[i] = 1;
                }
            }

            int minx = 20;
            int maxx = 0;
            int miny = 20;
            int maxy = 0;
            int u = 0;
            int v = 0;
            while (u < 20 && v < 20)
            {
                if (changes[20 * v + u] == 1)
                {
                    if (u < minx) minx = u;
                    if (u > maxx) maxx = u;
                    if (v < miny) miny = v;
                    if (v > maxy) maxy = v;
                }
                u++;
                if (u >= 20)
                {
                    u = 0;
                    v++;
                }
            }

            int height = (maxy - miny) + 1;
            int width = (maxx - minx) + 1;
            int maxdim = height > width ? height : width;
            byte[,] piece = new byte[maxdim, maxdim];

            for (u = minx; u <= maxx; u++)
            {
                for (v = miny; v <= maxy; v++)
                {

                    piece[v - miny, u - minx] = changes[v * 20 + u];
                }
            }
            DateTime dt = DateTime.Now;
            bool ispiece = IsPiece(move, oldstate.AvailablePieces.ToList(), piece);
            //sLogger.DebugFormat("               loop 4: {0}ms", (DateTime.Now - dt).TotalMilliseconds);
            dt = DateTime.Now;

            return ispiece;
        }

        public bool IsPiece(BlokusMove move,List<IPiece> availablepieces, byte[,] arrayBytes)
        {
            //Piece declared is same as piece placed
            if (move.Piece.Equals(arrayBytes))
            {
                foreach (var availablepiece in availablepieces)
                {
                    if (availablepiece.Equals(arrayBytes))
                    {
                        return true;
                    }
                }
            }
            //if (availablepieces.Contains(arrayBytes))
            //    {
            //        return true;
            //    }
            //}
            return false;
        }


    }

}
