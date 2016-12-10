using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TspVer2.Models
{
    public class MatrixOfWeights
    {

        public MatrixOfWeights(int size)
        {
            matrix = new int[size, size];
        }

        public int[,] matrix { get; }

        public int this[int x, int y]
        {
            get
            {
                return matrix[x, y];
            }
            
            set
            {
                matrix[x, y] = value;
            }
        }

        public int GetSize()
        {
            return matrix.GetLength(0);
        }
    }
}
