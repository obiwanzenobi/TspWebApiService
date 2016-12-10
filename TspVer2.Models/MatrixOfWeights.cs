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
            Matrix = new int[size, size];
        }

        public int[,] Matrix { get; set; }

        public int this[int x, int y]
        {
            get
            {
                return Matrix[x, y];
            }
            
            set
            {
                Matrix[x, y] = value;
            }
        }

        public int GetSize()
        {
            return Matrix.GetLength(0);
        }
    }
}
