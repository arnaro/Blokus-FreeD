using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blokus.Players
{
    public interface BlokusPlayer
    {
        byte[] PlayRound(byte[] gamestate);
        void Initialize(int playernumber);
    }
}
