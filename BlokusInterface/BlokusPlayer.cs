using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blokus
{
    public interface IBlokusPlayer
    {
        BlokusGameState PlayRound(BlokusGameState gamestate);
        void Initialize(int playernumber);
    }
}
