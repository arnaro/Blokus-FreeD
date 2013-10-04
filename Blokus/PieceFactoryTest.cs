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
            string input = "x";

            var test = solution(input);
            var test1 = solution("asdsa");
            Assert.AreEqual(3,extreme(new[]{9, 4, -3, -10}));
            //Assert.AreEqual(4, adjacency(new[] { 1, 0,0,0,1 }));
            Assert.AreEqual(4, adjacency(new[] { 1, 1, 0, 1, 0, 0 }));

            Assert.AreEqual(21, PieceFactory.GetPieces().Count);
            string formString = "XXX" + Environment.NewLine + "  X" + Environment.NewLine + "   " + Environment.NewLine;
            Assert.AreEqual(formString, PieceFactory.GetPieces()[7].ToString());
        }

        public int adjacency(int[] A)
        {
            int n = A.Length;
            int result = 0;
            for (int i = 0; i < n - 1; i++)
            {
                if (A[i] == A[i + 1])
                    result = result + 1;
            }
            int r = 0;
            for (int i = 0; i < n; i++)
            {
                int count = 0;
                if (i > 0)
                {
                    if (A[i - 1] != A[i])
                        count = count + 1;
                    else
                        count = count - 1;
                }
                if (i < n - 1)
                {
                    if (A[i + 1] != A[i])
                        count = count + 1;
                    else
                        count = count - 1;
                }
                r = Math.Max(r, count);
            }
            return result + r;
        }

        public int extreme(int[] A)
        {
            if (A.Length == 0)
            {
                return -1;
            }
            var M = 0;
            foreach (int value in A)
            {
                M += value;
            }
            M /= 4;
            double maxExtreme = double.MinValue;
            int extremeIndex = -1;
            for (int i = 0; i < A.Length; i++)
            {
                double deviation = (A[i] - M);
                if (deviation < 0)
                {
                    deviation *= -1;
                }
                if (deviation >= maxExtreme)
                {
                    maxExtreme = deviation;
                    extremeIndex = i;
                }
            }
            return extremeIndex;
        }

        public int solution(string S)
        {

            if (S.Length == 0)
                return -1;
            if (S.Length == 1)
                return 0;

            for (int i = 0; i < S.Length; i++)
            {
                char[] left = S.Substring(0, i).ToCharArray();
                char[] right = S.Substring(i+1).ToCharArray();
                Array.Reverse(left);
                if (left == right)
                {
                    return i;
                }
            }
            return -1;
        }
    }
}
