using System.Drawing;
using Blokus.Model;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

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
            Assert.That(bg.IsGameOver(), Is.True);
        }

        [Test]
        public void ScoreGameNoMoves()
        {
            int score = bg.ScoreGame(1);
            Assert.That(score, Is.EqualTo(-89));
            score = bg.ScoreGame(2);
            Assert.That(score, Is.EqualTo(-89));
            score = bg.ScoreGame(3);
            Assert.That(score, Is.EqualTo(-89));
            score = bg.ScoreGame(4);
            Assert.That(score, Is.EqualTo(-89));
        }

        [Test]
        public void ScoreGameAllPiecesPlaced()
        {
            bg.mPlayers[0].Moves = PieceFactory.GetPieces().OrderByDescending(c=>c.GetScore()).Select(a=> new BlokusMove(new byte[400]){Piece = a}).ToList();
            bg.mPlayerStates[0].Pieces = new List<IPiece>();
            int score = bg.ScoreGame(1);
            Assert.That(score, Is.EqualTo(20));
            bg.mPlayers[0].Moves = bg.mPlayers[0].Moves.OrderBy(c => c.Piece.GetScore()).ToList();
            score = bg.ScoreGame(1);
            Assert.That(score, Is.EqualTo(15));
        }

        [Test]
        public void ScoreGamePartiallyPlaced()
        {
            int total = 21;
            int placed = 10;
            bg.mPlayers[0].Moves = PieceFactory.GetPieces().OrderByDescending(c => c.GetScore()).Select(a => new BlokusMove(new byte[400]) { Piece = a }).Take(placed).ToList();
            bg.mPlayerStates[0].Pieces = PieceFactory.GetPieces().OrderBy(c => c.GetScore()).Take(total-placed).ToList();
            int score = bg.ScoreGame(1);
            Assert.That(score, Is.EqualTo(-39));
            placed = 20;
            bg.mPlayers[0].Moves = PieceFactory.GetPieces().OrderByDescending(c => c.GetScore()).Select(a => new BlokusMove(new byte[400]) { Piece = a }).Take(placed).ToList();
            bg.mPlayerStates[0].Pieces = PieceFactory.GetPieces().OrderBy(c => c.GetScore()).Take(total - placed).ToList();
            score = bg.ScoreGame(1);
            Assert.That(score, Is.EqualTo(-1));
        }


        [Test]
        public void PieceAvailable()
        {
            GameValidator gameValidator = new GameValidator();
            byte[] gamestate = new byte[400];
            for (int i = 0; i < 5; i++)
            {
                gamestate[i] = 1;
            }
            byte[] oldgamestate = new byte[400];
            BlokusGameState oldstate = new BlokusGameState(oldgamestate, PieceFactory.GetPieces());
            BlokusGameState newstate = new BlokusGameState(gamestate, PieceFactory.GetPieces());
            BlokusMove move = new BlokusMove(newstate.BlokusBoard) { Piece = PieceFactory.GetPieces().ElementAt(5) };
            bool result = gameValidator.CheckPiecePlacement(move, oldstate);

            Assert.That(result, Is.True);
        }

        [Test]
        public void PieceNotAvailable()
        {
            GameValidator gameValidator = new GameValidator();
            byte[] oldgamestate = new byte[400];
            BlokusGameState oldstate = new BlokusGameState(oldgamestate, PieceFactory.GetPieces());
            byte[] gamestate = new byte[400];
            for (int i = 0; i < 6; i++)
            {
                gamestate[i] = 1;
            }

            BlokusGameState newstate = new BlokusGameState(gamestate, PieceFactory.GetPieces());
            BlokusMove move = new BlokusMove(newstate.BlokusBoard) { Piece = PieceFactory.GetPieces().ElementAt(5) };
            bool result = gameValidator.CheckPiecePlacement(move,oldstate);

            Assert.That(result, Is.False);
        }
        [Test]
        public void SinglePieceAvailable()
        {
            GameValidator gameValidator = new GameValidator();
            byte[] oldgamestate = new byte[400];
            BlokusGameState oldstate = new BlokusGameState(oldgamestate, PieceFactory.GetPieces());
            byte[] gamestate = new byte[400];
            for (int i = 0; i < 1; i++)
            {
                gamestate[i] = 1;
            }

            BlokusGameState newstate = new BlokusGameState(gamestate, PieceFactory.GetPieces());
            BlokusMove move = new BlokusMove(newstate.BlokusBoard) { Piece = PieceFactory.GetPieces().ElementAt(0) };
            bool result = gameValidator.CheckPiecePlacement(move, oldstate);

            Assert.That(result, Is.True);
        }
        [Test]
        public void MultiRowPieceAvailable()
        {
            GameValidator gameValidator = new GameValidator();
            byte[] oldgamestate = new byte[400];
            BlokusGameState oldstate = new BlokusGameState(oldgamestate, PieceFactory.GetPieces());
            byte[] gamestate = new byte[400];
            for (int i = 0; i < 2; i++)
            {
                gamestate[i] = 1;
                gamestate[i + 20] = 1;
            }

            BlokusGameState newstate = new BlokusGameState(gamestate, PieceFactory.GetPieces());
            BlokusMove move = new BlokusMove(newstate.BlokusBoard) { Piece = PieceFactory.GetPieces().ElementAt(2) };
            bool result = gameValidator.CheckPiecePlacement(move, oldstate);

            Assert.That(result, Is.True);
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
            GameValidator validator = new GameValidator();

            byte[] oldgamestate = new byte[400];
            BlokusGameState oldstate = new BlokusGameState(oldgamestate, PieceFactory.GetPieces());

            byte[] gamestate = new byte[400];
            for (int i = 0; i < 3; i++)
            {
                gamestate[i] = 1;
            }
            BlokusGameState newstate = new BlokusGameState(gamestate, PieceFactory.GetPieces());

            BlokusMove move = new BlokusMove(gamestate) {Piece = PieceFactory.GetPieces().ElementAt(3)};

            bool isOk = validator.Validate(1, move, oldstate);
            Assert.That(isOk, Is.True);
        }
        [Test]
        public void TestFirstPieceWrongPlace()
        {
            GameValidator validator = new GameValidator();

            byte[] oldgamestate = new byte[400];
            BlokusGameState oldstate = new BlokusGameState(oldgamestate, PieceFactory.GetPieces());

            byte[] gamestate = new byte[400];
            for (int i = 17; i < 20; i++)
            {
                gamestate[i] = 1;
            }
            BlokusGameState newstate = new BlokusGameState(gamestate, PieceFactory.GetPieces());

            BlokusMove move = new BlokusMove(gamestate) { Piece = PieceFactory.GetPieces().ElementAt(3) };

            bool isOk = validator.Validate(1, move, oldstate);
            Assert.That(isOk, Is.False);

            // Change to player2... who should work
            for (int i = 17; i < 20; i++)
            {
                gamestate[i] = 2;
            }
            move = new BlokusMove(gamestate) { Piece = PieceFactory.GetPieces().ElementAt(3) };

            isOk = validator.Validate(2, move, oldstate);
            Assert.That(isOk, Is.True);
        }

        [Test,Ignore("Fails, Need to handle no changes in code")]
        public void NoChanges()
        {
            GameValidator gameValidator = new GameValidator();

            byte[] oldgamestate = new byte[400];
            BlokusGameState oldstate = new BlokusGameState(oldgamestate, PieceFactory.GetPieces());
            byte[] gamestate = new byte[400];

            BlokusGameState newstate = new BlokusGameState(gamestate, PieceFactory.GetPieces());
            BlokusMove move = new BlokusMove(gamestate) { Piece = PieceFactory.GetPieces().ElementAt(3) };

            bool result = gameValidator.CheckPiecePlacement(move, oldstate);

            Assert.That(result, Is.False);
        }



        [Test]
        public void TestIsNotOnTopOf()
        {
            GameValidator gameValidator = new GameValidator();

            BlokusGameState oldState = new BlokusGameState(new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 });
            BlokusGameState newState = new BlokusGameState(new byte[] { 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 });
            BlokusMove move = new BlokusMove(newState.BlokusBoard) { Piece = PieceFactory.GetPieces().ElementAt(4) };


            bool isOnTop = gameValidator.IsCorrectPlayerOnEmptySpace(1, move, oldState);
            Assert.That(isOnTop, Is.True);
        }

        [Test]
        public void TestIsOnTopOf()
        {
            GameValidator gameValidator = new GameValidator();

            BlokusGameState newState = new BlokusGameState(new byte[] { 2, 2, 2, 2,   0, 0, 0, 0,   0, 0, 0, 0,   0, 0, 0, 0 });
            BlokusGameState oldState = new BlokusGameState(new byte[] { 2, 1, 2, 2,   0, 1, 0, 0,   0, 1, 0, 0,   0, 1, 0, 0 });
            BlokusMove move = new BlokusMove(newState.BlokusBoard) { Piece = PieceFactory.GetPieces().ElementAt(3) };


            bool isOnTop = gameValidator.IsCorrectPlayerOnEmptySpace(1, move, oldState);
            Assert.That(isOnTop, Is.False);
        }

        [Test]
        public void TestIsOnTopOfEmpty()
        {
            GameValidator gameValidator = new GameValidator();

            BlokusGameState newState = new BlokusGameState(new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 });
            BlokusGameState oldState = new BlokusGameState(new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 });
            BlokusMove move = new BlokusMove(newState.BlokusBoard) { Piece = PieceFactory.GetPieces().ElementAt(3) };

            bool isOnTop = gameValidator.IsCorrectPlayerOnEmptySpace(1, move, oldState);
            Assert.That(isOnTop, Is.True);
        }

        [Test]
        public void TestIsNotOnTopOfWrongPlayer()
        {
            GameValidator gameValidator = new GameValidator();

            BlokusGameState oldState = new BlokusGameState(new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 });
            BlokusGameState newState = new BlokusGameState(new byte[] { 3, 3, 3, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 });
            BlokusMove move = new BlokusMove(newState.BlokusBoard) { Piece = PieceFactory.GetPieces().ElementAt(3) };
            bool isOnTop = gameValidator.IsCorrectPlayerOnEmptySpace(1, move, oldState);
            Assert.That(isOnTop, Is.False);
        }

        [Test]
        public void TestScore()
        {
            BlokusGameState state = new BlokusGameState(new byte[] {2, 2, 2, 1, 2, 0, 0, 0, 0, 3, 0, 0, 1, 1, 0, 0});
            Assert.That(bg.GetScore(1, state), Is.EqualTo(3));
            Assert.That(bg.GetScore(2, state), Is.EqualTo(4));
            Assert.That(bg.GetScore(3, state), Is.EqualTo(1));
            Assert.That(bg.GetScore(4, state), Is.EqualTo(0));
        }

        public void CornerToCornerSimple()
        {
            GameValidator gameValidator = new GameValidator();

            BlokusGameState oldState = new BlokusGameState(new byte[] { 0, 1, 0, 0,   0, 0, 0, 0,   0, 0, 0, 0,   0, 0, 0, 0 });
            BlokusGameState newState = new BlokusGameState(new byte[] { 0, 1, 0, 0,   1, 0, 0, 0,   0, 0, 0, 0,   0, 0, 0, 0 });
            BlokusMove move = new BlokusMove(newState.BlokusBoard) { Piece = PieceFactory.GetPieces().ElementAt(3) };
            bool corner = gameValidator.IsCornerToCorner(1, move, oldState);
            Assert.That(corner, Is.True);;
        }

        [Test]
        public void NoCornerToCornerSimple()
        {
            GameValidator gameValidator = new GameValidator();

            BlokusGameState oldState = new BlokusGameState(new byte[] { 0, 1, 0, 0,  0, 0, 0, 0,  0, 0, 0, 0,  0, 0, 0, 0 });
            BlokusGameState newState = new BlokusGameState(new byte[] { 0, 1, 0, 0,  0, 0, 0, 0,  0, 0, 0, 0,  0, 0, 0, 1 });
            BlokusMove move = new BlokusMove(newState.BlokusBoard) { Piece = PieceFactory.GetPieces().ElementAt(3) };
            bool corner = gameValidator.IsCornerToCorner(1, move, oldState);
            Assert.That(corner, Is.False);
        }

        [Test]
        public void SideToSideSimple()
        {
            GameValidator gameValidator = new GameValidator();

            BlokusGameState oldState = new BlokusGameState(new byte[] { 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 });
            BlokusGameState newState = new BlokusGameState(new byte[] { 0, 1, 0, 0,  0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 });
            BlokusMove move = new BlokusMove(newState.BlokusBoard) { Piece = PieceFactory.GetPieces().ElementAt(3) };
            bool corner = gameValidator.IsCornerToCorner(1, move, oldState);
            Assert.That(corner, Is.False);
        }
        [Test]
        public void CornerToCornerComplex()
        {
            GameValidator gameValidator = new GameValidator();

            BlokusGameState oldState = new BlokusGameState(new byte[] { 1, 1, 1, 0,  0, 0, 1, 0,  0, 0, 0, 0,  0, 0, 0, 0 });
            BlokusGameState newState = new BlokusGameState(new byte[] { 1, 1, 1, 0,  0, 0, 1, 0,  0, 1, 0, 1,  0, 1, 1, 1 });
            BlokusMove move = new BlokusMove(newState.BlokusBoard) { Piece = PieceFactory.GetPieces().ElementAt(3) };
            bool corner = gameValidator.IsCornerToCorner(1, move, oldState);
            Assert.That(corner, Is.True);
        }

        [Test]
        public void NoCornerToCornerComplex()
        {
            GameValidator gameValidator = new GameValidator();

            BlokusGameState oldState = new BlokusGameState(new byte[] { 1, 1, 1, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0 });
            BlokusGameState newState = new BlokusGameState(new byte[] { 1, 1, 1, 0, 0, 0, 1, 0, 1, 0, 0, 0, 1, 1, 1, 1 });
            BlokusMove move = new BlokusMove(newState.BlokusBoard) { Piece = PieceFactory.GetPieces().ElementAt(3) };
            bool corner = gameValidator.IsCornerToCorner(1, move, oldState);
            Assert.That(corner, Is.False);
        }

        [Test]
        public void SideToSideComplex()
        {
            GameValidator gameValidator = new GameValidator();

            BlokusGameState oldState = new BlokusGameState(new byte[] { 1, 1, 1, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0 });
            BlokusGameState newState = new BlokusGameState(new byte[] { 1, 1, 1, 0, 0, 0, 1, 0, 1, 1, 1, 1, 0, 0, 0, 0 });
            BlokusMove move = new BlokusMove(newState.BlokusBoard) { Piece = PieceFactory.GetPieces().ElementAt(3) };
            bool corner = gameValidator.IsCornerToCorner(1, move, oldState);
            Assert.That(corner, Is.False);
        }

        [Test]
        public void CornerToCornerComplexColors()
        {
            GameValidator gameValidator = new GameValidator();

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
            BlokusMove move = new BlokusMove(newState.BlokusBoard) { Piece = PieceFactory.GetPieces().ElementAt(3) };
            bool corner = gameValidator.IsCornerToCorner(1, move, oldState);
            Assert.That(corner, Is.True);;
        }


        [Test]
        public void NoCornerToCornerComplexColors()
        {
            GameValidator gameValidator = new GameValidator();

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
            BlokusMove move = new BlokusMove(newState.BlokusBoard) { Piece = PieceFactory.GetPieces().ElementAt(3) };
            bool corner = gameValidator.IsCornerToCorner(1, move, oldState);
            Assert.That(corner, Is.False);
        }

        [Test]
        public void SideToSideComplexColors()
        {
            GameValidator gameValidator = new GameValidator();

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
            BlokusMove move = new BlokusMove(newState.BlokusBoard) { Piece = PieceFactory.GetPieces().ElementAt(3) };
            bool corner = gameValidator.IsCornerToCorner(1, move, oldState);
            Assert.That(corner, Is.False);
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
            Assert.That(state.GetCorners(1), Is.EqualTo(expectedCorners));

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
            Assert.That(state.GetCorners(1), Is.EqualTo(expectedCorners));

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
            Assert.That(expectedCorners1.All(a => state.GetCorners(1).Contains(a)), Is.True);
            Assert.That(expectedCorners2.All(a => state.GetCorners(2).Contains(a)), Is.True);
        }

        [Test]
        public void FindAvailableMovesTest()
        {
            var originalBoard = new byte[]
                {
                    1, 1, 0, 0, 0, 0, 1, 1, 0, 0, 1, 1, 0, 0, 0, 0, 1, 1, 0, 0,
                    0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0,
                    0, 1, 1, 2, 2, 0, 0, 1, 1, 2, 0, 1, 1, 2, 2, 0, 0, 1, 1, 2,
                    0, 0, 0, 0, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 0, 0, 0, 0,
                    0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0,
                    0, 0, 0, 2, 2, 2, 0, 0, 0, 2, 0, 0, 0, 2, 2, 2, 0, 0, 0, 2,
                    1, 1, 0, 0, 0, 0, 1, 1, 0, 0, 1, 1, 0, 0, 0, 0, 1, 1, 0, 0,
                    0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0,
                    0, 1, 1, 2, 2, 0, 0, 1, 1, 2, 0, 1, 1, 2, 2, 0, 0, 1, 1, 2,
                    0, 0, 0, 0, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 0, 0, 0, 0,
                    1, 1, 0, 0, 0, 0, 1, 1, 0, 0, 1, 1, 0, 0, 0, 0, 1, 1, 0, 0,
                    0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0,
                    0, 1, 1, 2, 2, 0, 0, 1, 1, 2, 0, 1, 1, 2, 2, 0, 0, 1, 1, 2,
                    0, 0, 0, 0, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 0, 0, 0, 0,
                    0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0,
                    0, 0, 0, 2, 2, 2, 0, 0, 0, 2, 0, 0, 0, 2, 2, 2, 0, 0, 0, 2,
                    1, 1, 0, 0, 0, 0, 1, 1, 0, 0, 1, 1, 0, 0, 0, 0, 1, 1, 0, 0,
                    0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0,
                    0, 1, 1, 2, 2, 0, 0, 1, 1, 2, 0, 1, 1, 2, 2, 0, 0, 1, 1, 2,
                    0, 0, 0, 0, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2, 0, 0, 0, 0,
                };

            var gameBoard = new BlokusGameState(originalBoard);

            var piece = new byte[1,1];
            piece[0, 0] = 9;

            gameBoard.AvailablePieces.Add(new Piece(piece));

            BlokusMove move = gameBoard.TryPlacePieceOnCorner(piece, new Point(0, 3), new Point(0, 0), 1);
            Assert.That(move.BlokusBoard[60], Is.EqualTo(1));


            gameBoard = new BlokusGameState(originalBoard);
            var piece2 = new byte[3,2];
            piece2[0, 0] = 9;
            piece2[1, 0] = 1;
            piece2[2, 0] = 9;

            piece2[0, 1] = 9;
            piece2[1, 1] = 0;
            piece2[2, 1] = 0;

            gameBoard.AvailablePieces.Add(new Piece(piece2));

            move = gameBoard.TryPlacePieceOnCorner(piece2, new Point(2, 1), new Point(0, 1), 2);

            Assert.That(move.BlokusBoard[2], Is.EqualTo(2));
            Assert.That(move.BlokusBoard[3], Is.EqualTo(2));
            Assert.That(move.BlokusBoard[4], Is.EqualTo(2));
        }
    }

    public class BlockusUnitTestPlayer : BlokusBasePlayer
    {
        public List<IPiece> next_available_pieces= null;
        public byte[] next_blokus_board = null;

        public override BlokusMove PlayRound(IBlokusGameState gamestate)
        {
            if (next_available_pieces != null)
            {
                gamestate = new BlokusGameState(gamestate.BlokusBoard, next_available_pieces);
            }

            if (next_blokus_board != null)
            {
                gamestate = new BlokusGameState(next_blokus_board, gamestate.AvailablePieces);
            }
            //Todo fix when available moves is done
            return null;
        }
    }
}
