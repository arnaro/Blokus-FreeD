using System.Collections.Generic;

namespace Blokus
{
    public interface IPiece
    {

        List<byte[,]> Rotations { get; }
        int GetScore();
        bool Equals(object obj);
    }
}
