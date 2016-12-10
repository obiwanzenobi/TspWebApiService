using Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TspVer2.Implementations;
using TspVer2.Models;
using TspVer2.Service.Models;

namespace TspVer2.Service.Controllers
{
    public class TspResolveController : ApiController
    {

        // POST: api/TspResolve
        public HttpResponseMessage Post(TspResolveRequest req)
        {
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

            return null;
        }

    }
}
