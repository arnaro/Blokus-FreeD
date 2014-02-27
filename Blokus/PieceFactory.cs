using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blokus.Model;

namespace Blokus
{
    class PieceFactory
    {
        public static Dictionary<byte[,], IPiece> PieceDictionary = new Dictionary<byte[,], IPiece>();

        static public IList<IPiece> GetPieces()
        {
            byte c = Piece.CONNECTION_POINT_VALUE;

            List<IPiece> ListOfPieces = new List<IPiece>();

            var baseForm = new byte[,]
                {
                    {c}
                };
            ListOfPieces.Add(new Piece(baseForm));

            baseForm = new byte[,]
                {
                    {c,0},
                    {c,0}
                };
            ListOfPieces.Add(new Piece(baseForm));
            baseForm = new byte[,]
                {
                    {c,c},
                    {c,c}
                };
            ListOfPieces.Add(new Piece(baseForm));
            baseForm = new byte[,]
                {
                    {0,c,0},
                    {0,1,0},
                    {0,c,0}
                };
            ListOfPieces.Add(new Piece(baseForm));
            baseForm = new byte[,]
                {
                    {0,c,0,0},
                    {0,1,0,0},
                    {0,1,0,0},
                    {0,c,0,0}
                };
            ListOfPieces.Add(new Piece(baseForm));
            baseForm = new byte[,]
                {
                    {0,0,c,0,0},
                    {0,0,1,0,0},
                    {0,0,1,0,0},
                    {0,0,1,0,0},
                    {0,0,c,0,0}
                };
            ListOfPieces.Add(new Piece(baseForm));
            baseForm = new byte[,]
                {
                    {c,c},
                    {0,c}
                };
            ListOfPieces.Add(new Piece(baseForm));
            baseForm = new byte[,]
                {
                    {c,1,c},
                    {0,0,c},
                    {0,0,0}
                };
            ListOfPieces.Add(new Piece(baseForm));
            baseForm = new byte[,]
                {
                    {c,1,c},
                    {0,c,0},
                    {0,0,0}
                };
            ListOfPieces.Add(new Piece(baseForm));
            baseForm = new byte[,]
                {
                    {c,1,c},
                    {c,0,c},
                    {0,0,0}
                };
            ListOfPieces.Add(new Piece(baseForm));
            baseForm = new byte[,]
                {
                    {0,c,0},
                    {c,1,c},
                    {0,c,0}
                };
            ListOfPieces.Add(new Piece(baseForm));
            baseForm = new byte[,]
                {
                    {c,c,0},
                    {0,c,c},
                    {0,0,0}
                };
            ListOfPieces.Add(new Piece(baseForm));
            baseForm = new byte[,]
                {
                    {0,0,c},
                    {c,1,c},
                    {c,0,0}
                };
            ListOfPieces.Add(new Piece(baseForm));
            baseForm = new byte[,]
                {
                    {c,1,c},
                    {c,c,0},
                    {0,0,0}
                };
            ListOfPieces.Add(new Piece(baseForm));
            baseForm = new byte[,]
                {
                    {c,0,0},
                    {c,1,c},
                    {0,c,0}
                };
            ListOfPieces.Add(new Piece(baseForm));
            baseForm = new byte[,]
                {
                    {0,c,c},
                    {c,c,0},
                    {c,0,0}
                };
            ListOfPieces.Add(new Piece(baseForm));
            baseForm = new byte[,]
                {
                    {c,1,c},
                    {0,1,0},
                    {0,c,0}
                };
            ListOfPieces.Add(new Piece(baseForm));
            baseForm = new byte[,]
                {
                    {c,1,c},
                    {0,0,1},
                    {0,0,c}
                };
            ListOfPieces.Add(new Piece(baseForm));
            baseForm = new byte[,]
                {
                    {0,0,0,0},
                    {c,1,1,c},
                    {0,0,0,c},
                    {0,0,0,0}
                };
            ListOfPieces.Add(new Piece(baseForm));
            baseForm = new byte[,]
                {
                    {0,0,0,0},
                    {c,1,1,c},
                    {0,0,c,0},
                    {0,0,0,0}
                    
                };
            ListOfPieces.Add(new Piece(baseForm));
            baseForm = new byte[,]
                {
                    {0,0,0,0},
                    {c,1,c,0},
                    {0,0,c,c},
                    {0,0,0,0}
                    
                };
            ListOfPieces.Add(new Piece(baseForm));

            ListOfPieces.ForEach(a => a.Rotations.ForEach(b => PieceDictionary[b] = a));
            return ListOfPieces;
        }
    }
}
