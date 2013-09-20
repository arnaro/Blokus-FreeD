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
        static public IList<IPiece> GetPieces()
        {
            IList<IPiece> ListOfPieces = new List<IPiece>();

            var baseForm = new byte[,]
                {
                    {1}
                };
            ListOfPieces.Add(new Piece(baseForm));

            baseForm = new byte[,]
                {
                    {1,0},
                    {1,0}
                };
            ListOfPieces.Add(new Piece(baseForm));
            baseForm = new byte[,]
                {
                    {1,1},
                    {1,1}
                };
            ListOfPieces.Add(new Piece(baseForm));
            baseForm = new byte[,]
                {
                    {0,1,0},
                    {0,1,0},
                    {0,1,0}
                };
            ListOfPieces.Add(new Piece(baseForm));
            baseForm = new byte[,]
                {
                    {0,1,0,0},
                    {0,1,0,0},
                    {0,1,0,0},
                    {0,1,0,0}
                };
            ListOfPieces.Add(new Piece(baseForm));
            baseForm = new byte[,]
                {
                    {0,0,1,0,0},
                    {0,0,1,0,0},
                    {0,0,1,0,0},
                    {0,0,1,0,0},
                    {0,0,1,0,0}
                };
            ListOfPieces.Add(new Piece(baseForm));
            baseForm = new byte[,]
                {
                    {1,1},
                    {0,1}
                };
            ListOfPieces.Add(new Piece(baseForm));
            baseForm = new byte[,]
                {
                    {1,1,1},
                    {0,0,1},
                    {0,0,0}
                };
            ListOfPieces.Add(new Piece(baseForm));
            baseForm = new byte[,]
                {
                    {1,1,1},
                    {0,1,0},
                    {0,0,0}
                };
            ListOfPieces.Add(new Piece(baseForm));
            baseForm = new byte[,]
                {
                    {1,1,1},
                    {1,0,1},
                    {0,0,0}
                };
            ListOfPieces.Add(new Piece(baseForm));
            baseForm = new byte[,]
                {
                    {0,1,0},
                    {1,1,1},
                    {0,1,0}
                };
            ListOfPieces.Add(new Piece(baseForm));
            baseForm = new byte[,]
                {
                    {1,1,0},
                    {0,1,1},
                    {0,0,0}
                };
            ListOfPieces.Add(new Piece(baseForm));
            baseForm = new byte[,]
                {
                    {0,0,1},
                    {1,1,1},
                    {1,0,0}
                };
            ListOfPieces.Add(new Piece(baseForm));
            baseForm = new byte[,]
                {
                    {1,1,1},
                    {1,1,0},
                    {0,0,0}
                };
            ListOfPieces.Add(new Piece(baseForm));
            baseForm = new byte[,]
                {
                    {1,0,0},
                    {1,1,1},
                    {0,1,0}
                };
            ListOfPieces.Add(new Piece(baseForm));
            baseForm = new byte[,]
                {
                    {0,1,1},
                    {1,1,0},
                    {1,0,0}
                };
            ListOfPieces.Add(new Piece(baseForm));
            baseForm = new byte[,]
                {
                    {1,1,1},
                    {0,1,0},
                    {0,1,0}
                };
            ListOfPieces.Add(new Piece(baseForm));
            baseForm = new byte[,]
                {
                    {1,1,1},
                    {0,0,1},
                    {0,0,1}
                };
            ListOfPieces.Add(new Piece(baseForm));
            baseForm = new byte[,]
                {
                    {0,0,0,0},
                    {1,1,1,1},
                    {0,0,0,1},
                    {0,0,0,0}
                };
            ListOfPieces.Add(new Piece(baseForm));
            baseForm = new byte[,]
                {
                    {0,0,0,0},
                    {1,1,1,1},
                    {0,0,1,0},
                    {0,0,0,0}
                    
                };
            ListOfPieces.Add(new Piece(baseForm));
            baseForm = new byte[,]
                {
                    {0,0,0,0},
                    {1,1,1,0},
                    {0,0,1,1},
                    {0,0,0,0}
                    
                };
            ListOfPieces.Add(new Piece(baseForm));

            return ListOfPieces;
        }
    }
}
