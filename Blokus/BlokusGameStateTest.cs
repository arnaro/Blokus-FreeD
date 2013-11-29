using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Blokus
{
    [TestFixture]
    class BlokusGameStateTest
    {
        [Test]
        public void empty_board_available_moves()
        {
            byte[]  board = new byte[20 * 20];
            BlokusGameState playerState = new BlokusGameState(board, new List<IPiece>(PieceFactory.GetPieces()));
            Assert.AreEqual(1, playerState.GetAvailableMoves(1).Count());
        }
    }
}
