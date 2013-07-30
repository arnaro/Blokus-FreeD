using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blokus.Players
{
    class BlokusTestPlayer : BlokusPlayer
    {
        private int playerid;
        public byte[] PlayRound(byte[] gamestate)
        {
            return gamestate;
        }
        public void Initialize(int playernumber)
        {
            playerid = playernumber;
        }
    }
}
