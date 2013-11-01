using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blokus
{
    public class BlokusMove
    {
        public byte[] BlokusBoard { get; private set; }
        public IPiece Piece { get; set; }
    }
}
