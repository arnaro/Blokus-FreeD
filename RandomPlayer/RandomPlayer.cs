using System.Collections.Generic;
using Blokus;
using System;

namespace RandomPlayer
{
    public class RandomPlayer : BlokusBasePlayer
    {
        public override BlokusMove PlayRound(IBlokusGameState gamestate)
        {
            //Todo fix when available moves is done
            IList<BlokusMove> AvailableMoves = gamestate.GetAvailableMoves(Id);
            if (AvailableMoves.Count == 0)
            {
                //Todo fix when available moves is done
                return null;
            }
            Random r = new Random();
            int p = r.Next(AvailableMoves.Count);
            return AvailableMoves[p];
        }
    }
}
