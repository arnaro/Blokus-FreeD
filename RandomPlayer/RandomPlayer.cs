using Blokus;
using System;

namespace RandomPlayer
{
    public class RandomPlayer : BlokusBasePlayer
    {
        public override BlokusGameState PlayRound(BlokusGameState gamestate)
        {
            return gamestate;
        }
    }
}
