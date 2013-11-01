using Blokus;
using System;

namespace RandomPlayer
{
    public class RandomPlayer : BlokusBasePlayer
    {
        public override BlokusGameState PlayRound(BlokusGameState gamestate)
        {
            if (gamestate.Moves.Count == 0)
            {
                return gamestate;
            }

            Random r = new Random();
            return new BlokusGameState(gamestate.Moves[r.Next(gamestate.Moves.Count)].BlokusBoard);
        }
    }
}
