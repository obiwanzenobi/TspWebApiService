using Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TspVer2.Models;

namespace TspVer2.Implementations
{
    public class RandomPopulationGeneratorService : IPopulationGenerator
    {
        public Population GeneratePopulation(int size, int trackPointsCount)
        {
            Random rnd = new Random();
            Track[] track = new Track[size];

            for (int i = 0; i < size; i++)
            {
                List<int> population = Enumerable.Range(0, trackPointsCount).ToList();
                population.Shuffle(rnd);

                track[i] = new Track(population);
            }

            return new Population(track);
        }

    }
}
