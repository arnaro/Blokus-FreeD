using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blokus
{
    public interface IPiece
    {
        List<byte[,]> ListRoations();
    }
}
