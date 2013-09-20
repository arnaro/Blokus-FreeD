using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blokus.Model
{
    class Block
    {
        protected byte[,] baseForm;
        private int size;

        public Block(byte[,] matrix)
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
            return array;
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

        public void PruneForms()
        {
            
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
    }
}
