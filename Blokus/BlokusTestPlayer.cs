using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Blokus;

namespace BlokusTestPlayer
{
    public class BlokusTestPlayer : IBlokusPlayer
    {
        public string Name { get; set; }
        public int Id { get; set; }

        public BlokusGameState PlayRound(BlokusGameState gamestate)
        {
            return gamestate;
        }
        public void Initialize(int playernumber)
        {
            Id = playernumber;
        }
    }
}
