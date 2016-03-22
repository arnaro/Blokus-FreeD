using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blokus.Model
{
    class Piece : IPiece
    {
        public static readonly byte CONNECTION_POINT_VALUE = 9;

        protected byte[,] baseForm;
        private int size;  //Danger! Will change after Trim
        private List<byte[,]> mRots = null;

        public static IPiece GetPiece(byte[,] matrix)
        {
            if (PieceFactory.PieceDictionary.ContainsKey(matrix))
            {
                return PieceFactory.PieceDictionary[matrix];
            }
            else
            {
                throw new ArgumentException("Unknown matix");
            }
        }

        public Piece(byte[,] matrix)
        {
            if (matrix.GetLength(0) != matrix.GetLength(1))
            {
                throw new ArgumentException("Matrix must be nxn");
            }
            baseForm = matrix;
            size = (int)Math.Sqrt(baseForm.Length);
        }

        public List<byte[,]> Rotations
        {
            get
            {
                if (mRots == null)
                {
                    mRots = ListRoations();
                }
                return mRots;
            }
        }

        public int GetScore()
        {
            return (from byte item in baseForm
             where item > 0
             select item).Count();
        }

        private List<byte[,]> ListRoations()
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
            //If is square
            int size = piece.GetLength(0);
            if (size == piece.GetLength(1))
            {

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

                for (int x = size - 1; x >= 0; x--)
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
                byte[,] retArray = new byte[newList.Count, newList[0].Count];
                for (int y = 0; y < newList[0].Count; y++)
                {
                    for (int x = 0; x < newList.Count; x++)
                    {
                        retArray[x, y] = newList[x][y];
                    }
                }

                return retArray;
            }
            else
            {
                return piece;
            }
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
        //byte[,] AirFill(byte[,] piece)
        //{
        //    int max = Math.Max(piece.GetLength(0),piece.GetLength(1));
        //    byte[,] newpiece = new byte[max,max];
        //    for(int x = 0;x<piece.GetLength(0);x++)
        //    {
        //        for(int y=0;y<piece.GetLength(1);y++)
        //        {
        //            newpiece[x,y] = piece[x,y];
        //        }
        //    }
        //    return newpiece;
        //}

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < size; ++i)
            {
                for (int j = 0; j < size; ++j)
                {
                    stringBuilder.Append(baseForm[i, j] == 1 || baseForm[i, j] == Piece.CONNECTION_POINT_VALUE ? "X" : " ");
                }
                stringBuilder.Append(Environment.NewLine);
            }
            return stringBuilder.ToString();
        }

        public override bool Equals(object obj)
        {
            ByteArrayComparer byteComparer = new ByteArrayComparer();

            var arrayBytes = obj as byte[,];
            if (arrayBytes != null)
            {
                arrayBytes = Trim(arrayBytes);
                // do something
                var rotations = this.Rotations;
                foreach (var rotation in rotations)
                {
                    if (byteComparer.Equals(rotation, arrayBytes))
                    {
                        return true;
                    }
                }
            }

            if (!(obj is Piece))
            {
                return false;
            }
            Piece pieceA = this;
            Piece pieceB = (obj as Piece);

            //pieceA = new Piece(AirFill(pieceA.Trim(pieceA.baseForm)));
            //pieceB = new Piece(AirFill(pieceB.Trim(pieceB.baseForm)));

            var rotationsA = pieceA.Rotations;
            var rotationsB = pieceB.Rotations;

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

        public override int GetHashCode()
        {
            return baseForm.GetHashCode();
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
                        if ((x[j, i] == 0) != (y[j, i] == 0))
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
