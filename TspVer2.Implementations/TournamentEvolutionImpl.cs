using Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TspVer2.Models;

namespace TspVer2.Implementations
{
    public class TournamentEvolutionImpl : IPopulationEvolution
    {
        private Random _random;
        private ITrackWeightCalc _calc;

        public TournamentEvolutionImpl(Random random, ITrackWeightCalc calc)
        {
            _random = random;
            _calc = calc;
        }

        public Population EvolvePopulation(Population basePopulation, MatrixOfWeights[] weightMatrixArray)
        {
            int partsOfTournament = weightMatrixArray.Length;
            Track[] tracks = new Track[basePopulation.Length];

            int index = 0;
            int tournamentsPerPart = basePopulation.Length / partsOfTournament;
            for (int i = 0; i < partsOfTournament; i++)
            {
                for (int j = 0; j < tournamentsPerPart; j++)
                {
                    Track randomTrack = basePopulation[_random.Next(0, basePopulation.Length)];
                    int nextTrackWeightSum = _calc.CalculateWeightSum(basePopulation[index], weightMatrixArray[i]);
                    int randomTrackWeightSum = _calc.CalculateWeightSum(randomTrack, weightMatrixArray[i]);
                    tracks[index] = Math.Min(nextTrackWeightSum, randomTrackWeightSum) == nextTrackWeightSum ? basePopulation[index] : randomTrack;
                    index++;
                }
            }

            return new Population(tracks);
        }
    }
}
