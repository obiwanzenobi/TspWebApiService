using Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TspVer2.Models;

namespace TspVer2.Implementations
{
    public class TrackWeightCalcImpl : ITrackWeightCalc
    {
        public int CalculateWeightSum(Track track, MatrixOfWeights matrix)
        {
            int sum = 0;
            for (int i = 1; i < track.Length; i++)
            {

                sum += matrix[track[i - 1], track[i]];

            }

            return sum;
        }
    }
}
