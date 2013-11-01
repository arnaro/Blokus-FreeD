using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blokus.Model;

namespace Blokus
{
    public interface IGameValidator
    {
        bool Validate(IBlokusPlayer player, BlokusGameState newState, BlokusGameState oldstate);
        bool Validate(IBlokusPlayer player, BlokusGameState newState, BlokusGameState oldstate, bool performMove);
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
        public bool Validate(IBlokusPlayer player, BlokusGameState newState, BlokusGameState oldstate)
        {
            return Validate(player, newState, oldstate, true);
        }

        public bool Validate(IBlokusPlayer player, BlokusGameState newState, BlokusGameState oldstate, bool performMove)
        {
            if (IsCorrectPlayerOnEmptySpace(player, newState, oldstate) && IsCornerToCorner(player, newState, oldstate))
            {
                return CheckAndPlacePiece(newState, oldstate, performMove);
            }
            return false;
        }

        public bool IsCorrectPlayerOnEmptySpace(IBlokusPlayer player, BlokusGameState newState, BlokusGameState oldState)
        {
            List<byte> diff = newState.BlokusBoard.Select((a, i) => (byte)(a - oldState.BlokusBoard[i])).ToList();
            return diff.All(a => a == 0 || a == player.Id);
        }

        public bool IsCornerToCorner(IBlokusPlayer player, BlokusGameState newState, BlokusGameState oldState)
        {
            int howManyPer = (int)Math.Sqrt(newState.BlokusBoard.Length);
            var diff = newState.BlokusBoard.Select((a, i) => new
            {
                X = i % howManyPer,
                Y = i / howManyPer,
                Value = (byte)(a - oldState.BlokusBoard[i])
            }).Where(a => a.Value == player.Id);

            bool hasAtLeastOneCorner = false;
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
                            if (oldValue == player.Id)
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

        public bool CheckAndPlacePiece(BlokusGameState newState, BlokusGameState oldstate, bool performMove)
        {
            byte[] changes = new byte[400];
            for (int i = 0; i < 400; i++)
            {
                if (oldstate.BlokusBoard[i] != newState.BlokusBoard[i])
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

            return IsPiece(newState, new Piece(piece), performMove);
        }

        public bool IsPiece(BlokusGameState gamestate, IPiece piece, bool performMove)
        {
            // bool found = false;
            foreach (IPiece checkpiece in gamestate.AvailablePieces)
            {
                if (checkpiece.Equals(piece))
                {
                    if (performMove)
                    {
                        gamestate.AvailablePieces.Remove(checkpiece);
                    }
                    return true;
                }
            }
            return false;
        }


    }

}
