namespace Blokus
{
    public class BlokusMove
    {
        public BlokusMove(byte[] blokusBoard)
        {
            BlokusBoard = new byte[blokusBoard.Length];
            blokusBoard.CopyTo(BlokusBoard,0);
        }

        private BlokusMove()
        {

        }

        public byte[] BlokusBoard { get; private set; }
        public IPiece Piece { get; set; }
    }
}
