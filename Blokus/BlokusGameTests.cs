using NUnit.Framework;
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
        private List<IBlokusPlayer> players;

        [SetUp]
        public void setup()
        {
            players = new List<IBlokusPlayer> {new BlockusUnitTestPlayer{Id = 1}, new BlockusUnitTestPlayer { Id = 2}};
            bg = new BlokusGame(players);
        }

        [Test]
        public void TestIsNotOnTopOf()
        {
            BlokusGameState oldState = new BlokusGameState(new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, null);
            BlokusGameState newState = new BlokusGameState(new byte[] { 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, null);

            bool isOnTop = bg.IsCorrectPlayerOnEmptySpace(players[0], newState, oldState);
            Assert.AreEqual(true, isOnTop);
        }

        [Test]
        public void TestIsOnTopOf()
        {
            BlokusGameState newState = new BlokusGameState(new byte[] { 2, 2, 2, 2,   0, 0, 0, 0,   0, 0, 0, 0,   0, 0, 0, 0 }, null);
            BlokusGameState oldState = new BlokusGameState(new byte[] { 2, 1, 2, 2,   0, 1, 0, 0,   0, 1, 0, 0,   0, 1, 0, 0 }, null);

            bool isOnTop = bg.IsCorrectPlayerOnEmptySpace(players[0], newState, oldState);
            Assert.AreEqual(false, isOnTop);
        }

        [Test]
        public void TestIsOnTopOfEmpty()
        {
            BlokusGameState newState = new BlokusGameState(new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, null);
            BlokusGameState oldState = new BlokusGameState(new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, null);

            bool isOnTop = bg.IsCorrectPlayerOnEmptySpace(players[0], newState, oldState);
            Assert.AreEqual(true, isOnTop);
        }

        [Test]
        public void TestIsNotOnTopOfWrongPlayer()
        {
            BlokusGameState oldState = new BlokusGameState(new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, null);
            BlokusGameState newState = new BlokusGameState(new byte[] { 3, 3, 3, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, null);

            bool isOnTop = bg.IsCorrectPlayerOnEmptySpace(players[0], newState, oldState);
            Assert.AreEqual(false, isOnTop);
        }
    }

    public class BlockusUnitTestPlayer : IBlokusPlayer
    {

        public string Name { get; set; }

        public int Id { get; set; }

        public BlokusGameState PlayRound(BlokusGameState gamestate)
        {
            return gamestate;
        }

        public void Initialize(int playernumber)
        {
            Id = playernumber;
        }
    }
}
