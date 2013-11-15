using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Blokus;

namespace BlokusTestPlayer
{
    public class BlokusTestPlayer : BlokusBasePlayer
    {
        public override BlokusMove PlayRound(IBlokusGameState gamestate)
        {
            return new BlokusMove(gamestate.BlokusBoard);
        }
    }
}
