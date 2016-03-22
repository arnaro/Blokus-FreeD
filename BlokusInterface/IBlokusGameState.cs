using System.Collections.Generic;

namespace Blokus
{
    public interface IBlokusGameState
    {
        byte[] BlokusBoard { get; }
        IList<IPiece> AvailablePieces { get; }
        IList<BlokusMove> GetAvailableMoves(int playerId);
    }
}
