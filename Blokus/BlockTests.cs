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
    class BlockTests
    {
        [Test]
        public void BlockFormTest()
        {
            var basicForm = new byte[1,1];
            basicForm[0,0] = 1;

            Block block = new Block(basicForm);
            Assert.AreEqual(8,block.ListRoations().Count);

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

            block = new Block(basicForm);
            Assert.AreEqual(Form2, block.ListRoations()[1]);

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
            block = new Block(bigBaseForm);
            Assert.AreEqual(formFlip, block.ListRoations()[4]);
        }
    }
}
