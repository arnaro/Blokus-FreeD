using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blokus
{
    public interface IBlokusGameState
    {
        byte[] BlokusBoard { get; }
        IList<IPiece> AvailablePieces { get; }
    }
}
