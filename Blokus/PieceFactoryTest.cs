using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Blokus
{
    [TestFixture]
    class PieceFactoryTest
    {
        [Test]
        public void TestPieceFactory()
        {
            Assert.AreEqual(21, PieceFactory.GetPieces().Count);
            string formString = "XXX" + Environment.NewLine + "  X" + Environment.NewLine + "   " + Environment.NewLine;
            Assert.AreEqual(formString, PieceFactory.GetPieces()[7].ToString());
        }
    }
}
