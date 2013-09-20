using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blokus.Players
{
    class BlokusTestPlayer : IBlokusPlayer
    {
        private int playerid;
        public BlokusGameState PlayRound(BlokusGameState gamestate)
        {
            return gamestate;
        }
        public void Initialize(int playernumber)
        {
            playerid = playernumber;
        }
    }
}
