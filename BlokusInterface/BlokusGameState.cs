using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blokus
{
    public class BlokusGameState
    {
        public BlokusGameState(byte[] pGameState, IList<IPiece> pPieces)
        {
            BlokusBoard = pGameState;
            AvailablePieces = pPieces;
        }

        public byte[] BlokusBoard { get; private set; }

        public IList<IPiece> AvailablePieces { get; private set; }
    }
}
