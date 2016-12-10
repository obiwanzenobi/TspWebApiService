using Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TspVer2.Implementations;
using TspVer2.Models;

namespace TspVer2
{
    public static class Program
    {
        static void Main(string[] args)
        {

            int trackPointsCount = 10;
            int populationCount = 5;
            Random randomGen = new Random();
            IWeightMatrixGeneratorService generatorService = new RandomWeightMatrixGeneratorService(randomGen);
            MatrixOfWeights matrix1 = generatorService.GenerateMatrixWithWeights(trackPointsCount);
            MatrixOfWeights matrix2 = generatorService.GenerateMatrixWithWeights(trackPointsCount);

            IPopulationGenerator populationGenerator = new RandomPopulationGeneratorService();
            Population population = populationGenerator.GeneratePopulation(populationCount, trackPointsCount);

            //Track track2 = population[0];
            ITrackMutator mutator = new TrackInversionMutationService(randomGen);
            IPopulationMutator popMutator = new PopulationMutatorService(mutator, randomGen);
            popMutator.MutatePopulation(0.5, population);


            //mutator.MutateTrack(track2);

            ITrackCrossover crosser = new OxTrackCrossover(randomGen);
            IPopulationCrossover popCrosser = new PopulationCrossoverService(randomGen, crosser);
            popCrosser.DoCrossoverOnPopulation(0.5, population);

            Console.ReadKey();
        }

    }
}
