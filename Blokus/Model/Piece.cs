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
        private int size;  //Danger! Will change after Trim

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

            array = Trim(array);
            return PruneForms(array);
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

        public List<byte[,]> Trim(List<byte[,]> pieces)
        {
            List<byte[,]> ret = new List<byte[,]>();
            foreach (var piece in pieces)
            {
                ret.Add(Trim(piece));
            }
            return ret;
        }

        public byte[,] Trim(byte[,] piece)
        {
            List<List<byte>> newList = new List<List<byte>>();

            for (int x = 0; x < size; x++)
            {
                List<byte> row = new List<byte>();
                bool empty = true;
                for (int y = 0; y < size; y++)
                {
                    if (piece[x, y] > 0)
                    {
                        empty = false;
                        break;
                    }
                }
                if (!empty)
                {
                    for (int i = 0; i < size; i++)
                    {
                        row.Add(piece[x, i]);
                    }
                    newList.Add(row);
                }
            }

            for (int x = size-1; x >= 0; x--)
            {
                bool empty = true;
                foreach (var row in newList)
                {
                    if (row[x] > 0)
                    {
                        empty = false;
                        break;
                    }
                }
                if (empty)
                {
                    foreach (var row in newList)
                    {
                        row.RemoveAt(x);
                    }
                }
            }

            //Change to byte array
            byte[,] retArray = new byte[newList.Count,newList[0].Count];
            for (int y = 0; y < newList[0].Count; y++)
            {
                for (int x = 0; x < newList.Count; x++)
                {
                    retArray[x, y] = newList[x][y];
                }
            }

            return retArray;
        }

        public List<byte[,]> PruneForms(List<byte[,]> ListOfRotations)
        {
            return ListOfRotations.Distinct(new ByteArrayComparer()).ToList();
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

        public override bool Equals(object obj)
        {
            if (!(obj is Piece))
            {
                return false;
            }
            Piece pieceA = this;
            Piece pieceB = obj as Piece;

            var rotationsA = pieceA.ListRoations();
            var rotationsB = pieceB.ListRoations();

            ByteArrayComparer byteComparer = new ByteArrayComparer();
            foreach (var rotation in rotationsA)
            {
                if (byteComparer.Equals(rotation, rotationsB[0]))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool operator ==(Piece a, Piece b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(Piece a, Piece b)
        {
            return !(a == b);
        }

        public class ByteArrayComparer : IEqualityComparer<byte[,]>
        {
            public bool Equals(byte[,] x, byte[,] y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (x == null || y == null) return false;
                if (x.Length != y.Length) return false;
                if (x.GetLength(1) != y.GetLength(1)) return false;
                if (x.GetLength(0) != y.GetLength(0)) return false;
                
                for (int i = 0; i < x.GetLength(1); i++)
                {
                    for (int j = 0; j < x.GetLength(0); j++)
                    {
                        if (x[j, i] != y[j, i])
                        {
                            return false;
                        }
                    }
                }
                return true;
            }

            public int GetHashCode(byte[,] obj)
            {
                return base.GetHashCode();
            }
        }
    }
}
