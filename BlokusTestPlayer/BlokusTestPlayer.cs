using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Blokus;

namespace BlokusTestPlayer
{
    public class BlokusTestPlayer : BlokusBasePlayer
    {
        public override BlokusGameState PlayRound(BlokusGameState gamestate)
        {
            return gamestate;
        }
    }
}
