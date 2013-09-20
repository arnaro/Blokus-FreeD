using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blokus
{
    public class BlokusGameState
    {
        public byte[] BlokusBoard { get; set; }

        List<object> AvailableBloks { get; set; } 

    }
}
