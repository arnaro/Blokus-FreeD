using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blokus
{
    public interface IBlokusPlayer
    {
        string Name { get; set; }
        int Id { get; set; }

        BlokusGameState PlayRound(BlokusGameState gamestate);
        void Initialize(int playernumber);
    }
}
