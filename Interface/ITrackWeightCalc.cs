using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TspVer2.Models;

namespace Interface
{
    public interface ITrackWeightCalc
    {
        int CalculateWeightSum(Track pop, MatrixOfWeights matrix);
    }
}
