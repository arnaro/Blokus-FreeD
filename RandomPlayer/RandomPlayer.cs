using System.Collections.Generic;
using Blokus;
using System;

namespace RandomPlayer
{
    public class RandomPlayer : BlokusBasePlayer
    {
        public override BlokusGameState PlayRound(BlokusGameState gamestate)
        {
            IList<BlokusMove> AvailableMoves = gamestate.GetAvailableMoves(Id);
            if (AvailableMoves.Count == 0)
            {
                return gamestate;
            }
            Random r = new Random();
            return new BlokusGameState(AvailableMoves[r.Next(AvailableMoves.Count)].BlokusBoard);
        }
    }
}
