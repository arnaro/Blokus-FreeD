using System;
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
            Assert.That(piece.Rotations.Count, Is.EqualTo(1));
            
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
            Assert.That(piece.Rotations[1], Is.EqualTo(Form2));
            
            var bigBaseForm = new byte[4, 4]
                {
                    {0, 0, 1, 0},
                    {0, 1, 1, 0},
                    {0, 1, 0, 0},
                    {0, 1, 0, 0},
                };
            var formFlip = new byte[4, 2]
                {
                    {1, 0},
                    {1, 1},
                    {0, 1},
                    {0, 1},
                };
            piece = new Piece(bigBaseForm);
            Assert.That(piece.Rotations[4], Is.EqualTo(formFlip));
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
            Assert.That(Piece.ToString(), Is.EqualTo(formString));
        }


        [Test]
        public void PieceTrimTest()
        {
            var bigBaseForm = new byte[4, 4]
                {
                    {0, 0, 1, 0},
                    {0, 1, 1, 0},
                    {0, 1, 0, 0},
                    {0, 1, 0, 0},
                };
            Piece Piece = new Piece(bigBaseForm);
            var expectedArray = new byte[4, 2]
                {
                    {0, 1},
                    {1, 1},
                    {1, 0},
                    {1, 0},
                };

            var piece = Piece.Trim(bigBaseForm);
            Assert.That(piece, Is.EqualTo(expectedArray));

            bigBaseForm = new byte[4, 4]
                {
                    {0, 0, 0, 0},
                    {0, 1, 1, 0},
                    {0, 1, 0, 0},
                    {0, 0, 0, 0},
                };
            Piece = new Piece(bigBaseForm);
            expectedArray = new byte[2, 2]
                {
                    {1, 1},
                    {1, 0}
                };

            piece = Piece.Trim(bigBaseForm);
            Assert.That(piece, Is.EqualTo(expectedArray));

            bigBaseForm = new byte[4, 4]
                {
                    {0, 0, 0, 0},
                    {1, 1, 1, 0},
                    {0, 1, 0, 1},
                    {0, 0, 0, 0},
                };
            Piece = new Piece(bigBaseForm);
            expectedArray = new byte[2, 4]
                {
                    {1, 1, 1, 0},
                    {0, 1, 0, 1},
                };

            piece = Piece.Trim(bigBaseForm);
            Assert.That(piece, Is.EqualTo(expectedArray));

            bigBaseForm = new byte[4, 4]
                {
                    {1, 0, 0, 0},
                    {1, 0, 0, 0},
                    {1, 0, 0, 0},
                    {1, 0, 0, 0},
                };
            Piece = new Piece(bigBaseForm);
            expectedArray = new byte[4, 1]
                {
                    {1},
                    {1},
                    {1},
                    {1}
                };

            piece = Piece.Trim(bigBaseForm);
            Assert.That(piece, Is.EqualTo(expectedArray));

            bigBaseForm = new byte[4, 4]
                {
                    {0, 0, 0, 1},
                    {0, 0, 0, 1},
                    {0, 0, 0, 1},
                    {0, 0, 0, 1},
                };
            Piece = new Piece(bigBaseForm);
            expectedArray = new byte[4, 1]
                {
                    {1},
                    {1},
                    {1},
                    {1}
                };

            piece = Piece.Trim(bigBaseForm);
            Assert.That(piece, Is.EqualTo(expectedArray));
        }

        [Test]
        public void PiecePruneTest()
        {
            var form = new byte[4, 4]
                {
                    {0, 0, 0, 1},
                    {0, 0, 0, 1},
                    {0, 0, 0, 1},
                    {0, 0, 0, 1},
                };

            IPiece piece = new Piece(form);

            Assert.That(piece.Rotations.Count, Is.EqualTo(2));
        }

        [Test]
        public void PieceCompareTest()
        {
            var form = new byte[4, 4]
                {
                    {0, 0, 0, 1},
                    {0, 0, 0, 1},
                    {0, 0, 0, 1},
                    {0, 0, 0, 1},
                };

            IPiece pieceA = new Piece(form);
            IPiece pieceB = new Piece(form);

            Assert.That(pieceA.Equals(pieceB), Is.True);

            form = new byte[4, 4]
                {
                    {0, 0, 0, 1},
                    {0, 0, 0, 1},
                    {0, 0, 0, 1},
                    {0, 0, 1, 1},
                };
            IPiece pieceC = new Piece(form);

            Assert.That(pieceA.Equals(pieceC), Is.False);

            form = new byte[4, 4]
                {
                    {0, 0, 0, 0},
                    {0, 0, 0, 0},
                    {0, 0, 0, 0},
                    {1, 1, 1, 1},
                };
            IPiece pieceD = new Piece(form);

            Assert.That(pieceA.Equals(pieceD), Is.True);
        }
    }

}
