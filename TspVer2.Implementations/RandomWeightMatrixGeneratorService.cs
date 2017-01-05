using Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TspVer2.Models;

namespace TspVer2.Implementations
{
    public class RandomWeightMatrixGeneratorService : IWeightMatrixGeneratorService
    {
        private Random _randomGen;

        public RandomWeightMatrixGeneratorService(Random random)
        {
            _randomGen = random;
        }
        
        public MatrixOfWeights GenerateMatrixWithWeights(int size)
        {
            MatrixOfWeights matrix = new MatrixOfWeights(size);

            int calculatedSize = matrix.GetSize();

            for (int i = 0; i < matrix.GetSize(); i++)
            {

                for (int j = 0; j < matrix.GetSize(); j++)
                {
                    if (i == j)
                    {
                        matrix[i, j] = 0;
                    }

                    if (j < i)
                    {
                        matrix[i, j] = _randomGen.Next(1, 10);
                    }
                }

            }

            for (int i = 0; i < matrix.GetSize(); i++)
            {

                for (int j = 0; j < matrix.GetSize(); j++)
                {
                    if (j < i)
                    {
                        matrix[j, i] = matrix[i, j];
                    }
                }
            }
            return matrix;
        }



    }
}
