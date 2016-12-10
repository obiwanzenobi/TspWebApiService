using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TspVer2.Models;

namespace Interface
{
    public interface IPopulationGenerator
    {
        Population GeneratePopulation(int size, int trackPointsCount);
    }
}
