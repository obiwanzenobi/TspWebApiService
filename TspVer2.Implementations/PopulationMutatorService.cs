using Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TspVer2.Models;

namespace TspVer2.Implementations
{
    public class PopulationMutatorService : IPopulationMutator
    {
        private Random _random;
        private ITrackMutator _mutator;

        public PopulationMutatorService(ITrackMutator mutator, Random random)
        {
            _random = random;
            _mutator = mutator;
        }

        public void MutatePopulation(double mutationChance, Population population)
        {

           
            for (int i = 0; i < population.Length; i++)
            {
                double lottery = _random.NextDouble();
                if (lottery > mutationChance)
                {
                    _mutator.MutateTrack(population[i]);
                }
            }

        }
    }
}
