using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blokus
{
    public class BlokusGameState
    {

        public BlokusGameState(byte[] pGameState)
        {
            BlokusBoard = pGameState;
            AvailablePieces = new List<IPiece>();
            Moves = new List<BlokusMove>();
        }

        public BlokusGameState(byte[] pGameState, IList<IPiece> pPieces)
        {
            BlokusBoard = pGameState;
            AvailablePieces = pPieces;
            Moves = new List<BlokusMove>();
        }

        public byte[] BlokusBoard { get; private set; }

        public IList<IPiece> AvailablePieces { get; set; }

        public IList<BlokusMove> Moves { get; set; } 
    }

}
