﻿using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blokus
{
    [TestFixture]
    public class BlokusGameTests
    {
        private BlokusGame bg;
        private List<BlockusUnitTestPlayer> players;

        [SetUp]
        public void setup()
        {
            players = new List<BlockusUnitTestPlayer> { new BlockusUnitTestPlayer { Id = 1 }, new BlockusUnitTestPlayer { Id = 2 }, new BlockusUnitTestPlayer { Id = 3 }, new BlockusUnitTestPlayer { Id = 4 } };
            bg = new BlokusGame(players.Cast<IBlokusPlayer>().ToList());
        }

        [Test]
        public void GamePlayTestEveryonePasses()
        {
            byte[] gamestate = new byte[400];
            for (int i = 0; i < 5; i++)
            {
                gamestate[i] = 1;
            }
            byte[] oldgamestate = new byte[400];

            BlokusGameState oldstate = new BlokusGameState(oldgamestate, PieceFactory.GetPieces());
            BlokusGameState newstate = new BlokusGameState(gamestate, PieceFactory.GetPieces());

            bg.PlayGame();
            Assert.IsTrue(bg.IsGameOver());
        }


        [Test]
        public void PieceAvailable()
        {
            Validator validator = new Validator();
            byte[] gamestate = new byte[400];
            for (int i = 0; i < 5; i++)
            {
                gamestate[i] = 1;
            }
            byte[] oldgamestate = new byte[400];
            BlokusGameState oldstate = new BlokusGameState(oldgamestate, PieceFactory.GetPieces());
            BlokusGameState newstate = new BlokusGameState(gamestate, PieceFactory.GetPieces());
            bool result = validator.CheckAndPlacePiece(newstate, oldstate, true);

            Assert.AreEqual(true, result);
        }

        [Test]
        public void PieceNotAvailable()
        {
            Validator validator = new Validator();
            byte[] oldgamestate = new byte[400];
            BlokusGameState oldstate = new BlokusGameState(oldgamestate, PieceFactory.GetPieces());
            byte[] gamestate = new byte[400];
            for (int i = 0; i < 6; i++)
            {
                gamestate[i] = 1;
            }

            BlokusGameState newstate = new BlokusGameState(gamestate, PieceFactory.GetPieces());
            bool result = validator.CheckAndPlacePiece(newstate, oldstate, true);

            Assert.AreEqual(false, result);
        }
        [Test]
        public void SinglePieceAvailable()
        {
            Validator validator = new Validator();
            byte[] oldgamestate = new byte[400];
            BlokusGameState oldstate = new BlokusGameState(oldgamestate, PieceFactory.GetPieces());
            byte[] gamestate = new byte[400];
            for (int i = 0; i < 1; i++)
            {
                gamestate[i] = 1;
            }

            BlokusGameState newstate = new BlokusGameState(gamestate, PieceFactory.GetPieces());
            bool result = validator.CheckAndPlacePiece(newstate, oldstate, true);

            Assert.AreEqual(true, result);
        }
        [Test]
        public void MultiRowPieceAvailable()
        {
            Validator validator = new Validator();
            byte[] oldgamestate = new byte[400];
            BlokusGameState oldstate = new BlokusGameState(oldgamestate, PieceFactory.GetPieces());
            byte[] gamestate = new byte[400];
            for (int i = 0; i < 2; i++)
            {
                gamestate[i] = 1;
                gamestate[i + 20] = 1;
            }

            BlokusGameState newstate = new BlokusGameState(gamestate, PieceFactory.GetPieces());
            bool result = validator.CheckAndPlacePiece(newstate, oldstate, true);

            Assert.AreEqual(true, result);
        }
        
        [Test]
        public void TestPlayerHacking()
        {
            // setup next player to cheat
            players[0].next_available_pieces = new List<IPiece>();
            bg.NextMove();
            // exception?
        }

        [Test]
        public void TestPlayerHackingPieces()
        {
            // setup next player to cheat
            players[0].next_available_pieces = new List<IPiece>();
            players[0].next_blokus_board = new byte[400];
            for (int i = 0; i < 3; i++)
            {
                players[0].next_blokus_board[i] = 1;
            }
            bg.NextMove();
            // exception?
        }

        [Test]
        public void TestFirstPiece()
        {
            Validator validator = new Validator();

            byte[] oldgamestate = new byte[400];
            BlokusGameState oldstate = new BlokusGameState(oldgamestate, PieceFactory.GetPieces());

            byte[] gamestate = new byte[400];
            for (int i = 0; i < 3; i++)
            {
                gamestate[i] = 1;
            }
            BlokusGameState newstate = new BlokusGameState(gamestate, PieceFactory.GetPieces());

            bool isOk = validator.Validate(players[0], newstate, oldstate);
            Assert.AreEqual(true, isOk);
        }
        [Test]
        public void TestFirstPieceWrongPlace()
        {
            Validator validator = new Validator();

            byte[] oldgamestate = new byte[400];
            BlokusGameState oldstate = new BlokusGameState(oldgamestate, PieceFactory.GetPieces());

            byte[] gamestate = new byte[400];
            for (int i = 17; i < 20; i++)
            {
                gamestate[i] = 1;
            }
            BlokusGameState newstate = new BlokusGameState(gamestate, PieceFactory.GetPieces());

            bool isOk = validator.Validate(players[0], newstate, oldstate);
            Assert.AreEqual(false, isOk);

            // Change to player2... who should work
            for (int i = 17; i < 20; i++)
            {
                gamestate[i] = 2;
            }
            isOk = validator.Validate(players[1], newstate, oldstate);
            Assert.AreEqual(true, isOk);
        }

        [Test,Ignore("Fails, Need to handle no changes in code")]
        public void NoChanges()
        {
            Validator validator = new Validator();

            byte[] oldgamestate = new byte[400];
            BlokusGameState oldstate = new BlokusGameState(oldgamestate, PieceFactory.GetPieces());
            byte[] gamestate = new byte[400];

            BlokusGameState newstate = new BlokusGameState(gamestate, PieceFactory.GetPieces());
            bool result = validator.CheckAndPlacePiece(newstate, oldstate, true);

            Assert.AreEqual(false, result);
        }



        [Test]
        public void TestIsNotOnTopOf()
        {
            Validator validator = new Validator();

            BlokusGameState oldState = new BlokusGameState(new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 });
            BlokusGameState newState = new BlokusGameState(new byte[] { 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 });

            bool isOnTop = validator.IsCorrectPlayerOnEmptySpace(players[0], newState, oldState);
            Assert.AreEqual(true, isOnTop);
        }

        [Test]
        public void TestIsOnTopOf()
        {
            Validator validator = new Validator();

            BlokusGameState newState = new BlokusGameState(new byte[] { 2, 2, 2, 2,   0, 0, 0, 0,   0, 0, 0, 0,   0, 0, 0, 0 });
            BlokusGameState oldState = new BlokusGameState(new byte[] { 2, 1, 2, 2,   0, 1, 0, 0,   0, 1, 0, 0,   0, 1, 0, 0 });

            bool isOnTop = validator.IsCorrectPlayerOnEmptySpace(players[0], newState, oldState);
            Assert.AreEqual(false, isOnTop);
        }

        [Test]
        public void TestIsOnTopOfEmpty()
        {
            Validator validator = new Validator();

            BlokusGameState newState = new BlokusGameState(new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 });
            BlokusGameState oldState = new BlokusGameState(new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 });

            bool isOnTop = validator.IsCorrectPlayerOnEmptySpace(players[0], newState, oldState);
            Assert.AreEqual(true, isOnTop);
        }

        [Test]
        public void TestIsNotOnTopOfWrongPlayer()
        {
            Validator validator = new Validator();

            BlokusGameState oldState = new BlokusGameState(new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 });
            BlokusGameState newState = new BlokusGameState(new byte[] { 3, 3, 3, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 });

            bool isOnTop = validator.IsCorrectPlayerOnEmptySpace(players[0], newState, oldState);
            Assert.AreEqual(false, isOnTop);
        }

        [Test]
        public void TestScore()
        {
            BlokusGameState state = new BlokusGameState(new byte[] {2, 2, 2, 1, 2, 0, 0, 0, 0, 3, 0, 0, 1, 1, 0, 0});
            Assert.AreEqual(3, bg.GetScore(1, state));
            Assert.AreEqual(4, bg.GetScore(2, state));
            Assert.AreEqual(1, bg.GetScore(3, state));
            Assert.AreEqual(0, bg.GetScore(4, state));
        }

        public void CornerToCornerSimple()
        {
            Validator validator = new Validator();

            BlokusGameState oldState = new BlokusGameState(new byte[] { 0, 1, 0, 0,   0, 0, 0, 0,   0, 0, 0, 0,   0, 0, 0, 0 });
            BlokusGameState newState = new BlokusGameState(new byte[] { 0, 1, 0, 0,   1, 0, 0, 0,   0, 0, 0, 0,   0, 0, 0, 0 });

            bool corner = validator.IsCornerToCorner(players[0], newState, oldState);
            Assert.AreEqual(true, corner);
        }

        [Test]
        public void NoCornerToCornerSimple()
        {
            Validator validator = new Validator();

            BlokusGameState oldState = new BlokusGameState(new byte[] { 0, 1, 0, 0,  0, 0, 0, 0,  0, 0, 0, 0,  0, 0, 0, 0 });
            BlokusGameState newState = new BlokusGameState(new byte[] { 0, 1, 0, 0,  0, 0, 0, 0,  0, 0, 0, 0,  0, 0, 0, 1 });

            bool corner = validator.IsCornerToCorner(players[0], newState, oldState);
            Assert.AreEqual(false, corner);
        }

        [Test]
        public void SideToSideSimple()
        {
            Validator validator = new Validator();

            BlokusGameState oldState = new BlokusGameState(new byte[] { 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 });
            BlokusGameState newState = new BlokusGameState(new byte[] { 0, 1, 0, 0,  0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 });

            bool corner = validator.IsCornerToCorner(players[0], newState, oldState);
            Assert.AreEqual(false, corner);
        }
        [Test]
        public void CornerToCornerComplex()
        {
            Validator validator = new Validator();

            BlokusGameState oldState = new BlokusGameState(new byte[] { 1, 1, 1, 0,  0, 0, 1, 0,  0, 0, 0, 0,  0, 0, 0, 0 });
            BlokusGameState newState = new BlokusGameState(new byte[] { 1, 1, 1, 0,  0, 0, 1, 0,  0, 1, 0, 1,  0, 1, 1, 1 });

            bool corner = validator.IsCornerToCorner(players[0], newState, oldState);
            Assert.AreEqual(true, corner);
        }

        [Test]
        public void NoCornerToCornerComplex()
        {
            Validator validator = new Validator();

            BlokusGameState oldState = new BlokusGameState(new byte[] { 1, 1, 1, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0 });
            BlokusGameState newState = new BlokusGameState(new byte[] { 1, 1, 1, 0, 0, 0, 1, 0, 1, 0, 0, 0, 1, 1, 1, 1 });

            bool corner = validator.IsCornerToCorner(players[0], newState, oldState);
            Assert.AreEqual(false, corner);
        }

        [Test]
        public void SideToSideComplex()
        {
            Validator validator = new Validator();

            BlokusGameState oldState = new BlokusGameState(new byte[] { 1, 1, 1, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0 });
            BlokusGameState newState = new BlokusGameState(new byte[] { 1, 1, 1, 0, 0, 0, 1, 0, 1, 1, 1, 1, 0, 0, 0, 0 });

            bool corner = validator.IsCornerToCorner(players[0], newState, oldState);
            Assert.AreEqual(false, corner);
        }

        [Test]
        public void CornerToCornerComplexColors()
        {
            Validator validator = new Validator();

            BlokusGameState oldState = new BlokusGameState(new byte[]
            {
                1, 1, 1, 2,
                3, 3, 1, 2,
                3, 0, 2, 2,
                3, 0, 0, 0
            });
            BlokusGameState newState = new BlokusGameState(new byte[]
            {
                1, 1, 1, 2,
                3, 3, 1, 2,
                3, 1, 2, 2,
                3, 1, 1, 1
            });

            bool corner = validator.IsCornerToCorner(players[0], newState, oldState);
            Assert.AreEqual(true, corner);
        }


        [Test]
        public void NoCornerToCornerComplexColors()
        {
            Validator validator = new Validator();

            BlokusGameState oldState = new BlokusGameState(new byte[]
            {
                1, 1, 1, 2,
                3, 3, 1, 2,
                3, 0, 2, 2,
                3, 0, 0, 0
            });
            BlokusGameState newState = new BlokusGameState(new byte[]
            {
                1, 1, 1, 2,
                3, 3, 1, 2,
                3, 0, 2, 2,
                3, 1, 1, 1
            });

            bool corner = validator.IsCornerToCorner(players[0], newState, oldState);
            Assert.AreEqual(false, corner);
        }

        [Test]
        public void SideToSideComplexColors()
        {
            Validator validator = new Validator();

            BlokusGameState oldState = new BlokusGameState(new byte[]
            {
                1, 1, 1, 2,
                3, 0, 1, 0,
                3, 0, 2, 0,
                3, 0, 0, 0
            });
            BlokusGameState newState = new BlokusGameState(new byte[]
            {
                1, 1, 1, 2,
                3, 0, 1, 1,
                3, 0, 2, 1,
                3, 0, 0, 0
            });

            bool corner = validator.IsCornerToCorner(players[0], newState, oldState);
            Assert.AreEqual(false, corner);
        }

        [Test]
        public void FindCornersTest()
        {
            BlokusGameState state = new BlokusGameState(new byte[]
                {
                    1, 1, 0, 0,
                    0, 1, 0, 0,
                    0, 0, 0, 0,
                    0, 0, 0, 0
                } );

            List<int> expectedCorners = new List<int>{8,10};
            Assert.AreEqual(expectedCorners, state.GetCorners(1));

            //-------
             state = new BlokusGameState(new byte[]
                {
                    1, 1, 0, 0, 0, 0,
                    0, 1, 0, 0, 0, 0,
                    1, 0, 0, 0, 0, 0,
                    1, 1, 1, 0, 0, 0,
                    0, 0, 0, 0, 0, 0,
                    0, 0, 0, 0, 0, 0
                } );

            expectedCorners = new List<int>{15,27};
            Assert.AreEqual(expectedCorners, state.GetCorners(1));

            //-------
            state = new BlokusGameState(new byte[]
                {
                    1, 1, 0, 0, 0, 0,
                    0, 1, 0, 0, 0, 0,
                    0, 1, 1, 2, 2, 0,
                    0, 0, 0, 0, 2, 2,
                    0, 0, 0, 0, 2, 0,
                    0, 0, 0, 2, 2, 2,
                } );

            var expectedCorners1 = new List<int>{9, 21, 18};
            var expectedCorners2 = new List<int>{8, 11, 20, 26};
            Assert.IsTrue(expectedCorners1.All(a => state.GetCorners(1).Contains(a)));
            Assert.IsTrue(expectedCorners2.All(a => state.GetCorners(2).Contains(a)));
        }
    }

    public class BlockusUnitTestPlayer : BlokusBasePlayer
    {
        public List<IPiece> next_available_pieces= null;
        public byte[] next_blokus_board = null;

        public override BlokusGameState PlayRound(BlokusGameState gamestate)
        {
            if (next_available_pieces != null)
            {
                gamestate.AvailablePieces = next_available_pieces;
            }

            if (next_blokus_board != null)
            {
                gamestate = new BlokusGameState(next_blokus_board, gamestate.AvailablePieces);
            }

            return gamestate;
        }
    }
}
