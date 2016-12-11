using Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TspVer2.Models;

namespace TspVer2.Implementations
{
    public class PopulationCrossoverService : IPopulationCrossover
    {
        private Random _random;
        private ITrackCrossover _crossover;

        public PopulationCrossoverService(Random random, ITrackCrossover crossover)
        {
            _random = random;
            _crossover = crossover;
        }

        public void DoCrossoverOnPopulation(double crossoverChance, Population population)
        {

            for (int i = 1; i < population.Length; i += 2)
            {
                double lottery = _random.NextDouble();
                if (lottery > crossoverChance)
                {
                    _crossover.DoCrossover(population[i - 1], population[i]);
                }
            }

            population.Workers.Shuffle(_random);

        }
    }
}
