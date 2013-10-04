using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blokus.Model
{
    class Piece : IPiece
    {
        protected byte[,] baseForm;
        private int size;

        public Piece(byte[,] matrix)
        {
            baseForm = matrix;
            size = (int) Math.Sqrt(baseForm.Length);
        }

        public List<byte[,]> ListRoations()
        {
            var array = new List<byte[,]>(8);
            array.Add(baseForm);
            int i;
            for (i = 1; i < 4; i++)
            {
                array.Add(RotateMatrix(array[i-1]));
            }
            var flippedForm = FlipMatrix(baseForm);
            array.Add(flippedForm);
            for (i = 5; i < 8; i++)
            {
                array.Add(RotateMatrix(array[i - 1]));
            }
            return array.Distinct(new PieceComparer()).ToList();
            return array;
            //return PruneForms(array);
        }

        byte[,] FlipMatrix(byte[,] matrix)
        {
            var flippedMatrix = new byte[size,size];
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    flippedMatrix[i, size - j - 1] = matrix[i, j];
                }
            }
            return flippedMatrix;
        }

        public List<byte[,]> PruneForms(List<byte[,]> ListOfRotations)
        {
            return new List<byte[,]>();
            //return (ListOfRotations.Distinct(new PieceComparer());


            //if (ListOfRotations[0].ToString() == ListOfRotations[4].ToString())
            //{
            //    ListOfRotations.RemoveRange(4,4);
            //}

            //ListOfRotations.RemoveAll(a => a.ToString() == ListOfRotations[0].ToString() && a != ListOfRotations[0]);
            //return ListOfRotations;
        }

        byte[,] RotateMatrix(byte[,] matrix)
        {
            byte[,] ret = new byte[size, size];

            for (int i = 0; i < size; ++i)
            {
                for (int j = 0; j < size; ++j)
                {
                    ret[i, j] = matrix[size - j - 1, i];
                }
            }
            return ret;
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < size; ++i)
            {
                for (int j = 0; j < size; ++j)
                {
                    stringBuilder.Append(baseForm[i, j] == 1 ? "X" : " ");
                }
                stringBuilder.Append(Environment.NewLine);
            }
            return stringBuilder.ToString();
        }

        public class PieceComparer : IEqualityComparer<byte[,]>
        {
            public bool Equals(byte[,] x, byte[,] y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (x == null || y == null) return false;
                if (x.Length != y.Length) return false;
                for (int i = 0; i < x.Length; i++)
                {
                    for (int j = 0; j < x.LongLength; j++)
                    {
                        if (x[i,j] != y[i,j]) return false;
                    }
                }
                return true;

            }

            public int GetHashCode(byte[,] obj)
            {
                int result = 13 * obj.Length;
                for (int i = 0; i < obj.Length; i++)
                {
                    for (int j = 0; j < obj.LongLength; j++)
                    {
                        result = (17*result) + obj[i,j];
                    }
                }
                return result;
            }
        }
    }
}
