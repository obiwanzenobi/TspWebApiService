using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TspVer2.Models;

namespace Interface
{
    public interface IPopulationCrossover
    {

        void DoCrossoverOnPopulation(double crossoverChance, Population population);

    }
}
