using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blokus.Model;
using NUnit.Framework;

namespace Blokus
{
    [TestFixture]
    class PieceTests
    {
        [Test]
        public void PieceFormTest()
        {
            var basicForm = new byte[1,1];
            basicForm[0,0] = 1;

            Piece piece = new Piece(basicForm);
            Assert.AreEqual(8,piece.ListRoations().Count);

            basicForm = new byte[2,2]
                {
                    {1, 1},
                    {1, 0}
                };
            var Form2 = new byte[2, 2]
                {
                    {1, 1},
                    {0, 1}
                };

            piece = new Piece(basicForm);
            Assert.AreEqual(Form2, piece.ListRoations()[1]);

            var bigBaseForm = new byte[4, 4]
                {
                    {0, 0, 1, 0},
                    {0, 1, 1, 0},
                    {0, 1, 0, 0},
                    {0, 1, 0, 0},
                };
            var formFlip = new byte[4, 4]
                {
                    {0, 1, 0, 0},
                    {0, 1, 1, 0},
                    {0, 0, 1, 0},
                    {0, 0, 1, 0},
                };
            piece = new Piece(bigBaseForm);
            Assert.AreEqual(formFlip, piece.ListRoations()[4]);
        }

        [Test]
        public void PieceToStringTest()
        {
            var bigBaseForm = new byte[4, 4]
                {
                    {0, 0, 1, 0},
                    {0, 1, 1, 0},
                    {0, 1, 0, 0},
                    {0, 1, 0, 0},
                };
            IPiece Piece = new Piece(bigBaseForm);

            string formString = "  X " + Environment.NewLine + " XX " + Environment.NewLine + " X  " + Environment.NewLine + " X  " + Environment.NewLine;
            Assert.AreEqual(formString, Piece.ToString());
        }
    }
}
