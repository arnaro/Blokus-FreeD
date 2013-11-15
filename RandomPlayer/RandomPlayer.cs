using System.Collections.Generic;
using Blokus;
using System;

namespace RandomPlayer
{
    public class RandomPlayer : BlokusBasePlayer
    {
        public override BlokusMove PlayRound(IBlokusGameState gamestate)
        {
            IList<BlokusMove> AvailableMoves = gamestate.GetAvailableMoves(Id);
            if (AvailableMoves.Count == 0)
            {
                return gamestate;
            }
            Random r = new Random();
            return new BlokusMove(AvailableMoves[r.Next(AvailableMoves.Count)].BlokusBoard);
        }
    }
}
