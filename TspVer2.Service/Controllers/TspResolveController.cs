﻿using Interface;
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

            //MatrixOfWeights matrix1 = generatorService.GenerateMatrixWithWeights(req.PopSize);
            MatrixOfWeights matrix2 = generatorService.GenerateMatrixWithWeights(req.PopSize);

            MatrixOfWeights matrix = new MatrixOfWeights(req.IdList.ToList().Count);
            matrix.Matrix = req.WeightMatrix;
            //MatrixOfWeights matrix = generatorService.GenerateMatrixWithWeights(req.IdList.Count);

            IPopulationGenerator populationGenerator = new RandomPopulationGeneratorService();
            Population population = populationGenerator.GeneratePopulation(req.PopSize, req.IdList.ToList().Count);

            TspResolveResponse response = new TspResolveResponse();
            response.Iterations = new TspResolveResponse.Iteration[req.IterationsNumber];

            ITrackWeightCalc calc = new TrackWeightCalcImpl();
            IPopulationEvolution evolution = new TournamentEvolutionImpl(randomGen, calc);

            ITrackMutator mutator = new TrackInversionMutationService(randomGen);
            IPopulationMutator popMutator = new PopulationMutatorService(mutator, randomGen);
            popMutator.MutatePopulation(req.MutationRate, population);

            population.Workers.Shuffle(randomGen);

            ITrackCrossover crosser = new OxTrackCrossover(randomGen);
            IPopulationCrossover popCrosser = new PopulationCrossoverService(randomGen, crosser);
            popCrosser.DoCrossoverOnPopulation(req.CrossoverRate, population);

            for (int i = 0; i < req.IterationsNumber; i++)
            {

                response.Iterations[i] = new TspResolveResponse.Iteration();
                response.Iterations[i].Resolves = new List<TspResolveResponse.ResolveContainer>();

                population = evolution.EvolvePopulation(population, new MatrixOfWeights[] { matrix, matrix2 });

                int initialWeightSum = calc.CalculateWeightSum(population.Workers[0], matrix);

                int lowestFirstIndex = 0;
                double lowestFirstValue = calc.CalculateWeightSum(population.Workers[0], matrix);

                int lowestSecondIndex = 0;
                double lowestSecondValue = calc.CalculateWeightSum(population.Workers[0], matrix2);

                for (int j = 0; j < req.PopSize; j++)
                {
                    TspResolveResponse.ResolveContainer resolve = new TspResolveResponse.ResolveContainer();

                    resolve.FirstCost = calc.CalculateWeightSum(population.Workers[j], matrix);
                    resolve.SecondCost = calc.CalculateWeightSum(population.Workers[j], matrix2);

                    if (resolve.FirstCost > lowestFirstValue)
                    {
                        lowestFirstIndex = j;
                        lowestFirstValue = resolve.FirstCost;
                    }
                    if (resolve.SecondCost > lowestSecondValue)
                    {
                        lowestSecondIndex = j;
                        lowestSecondValue = resolve.SecondCost;
                    }

                    resolve.OrderedIdList = population.Workers[j].TrackPoints;

                    response.Iterations[i].Resolves.Add(resolve);
                }
                response.Iterations[i].Id = i;
                response.Iterations[i].BestFirstFuncResolveIndex = lowestFirstIndex;
                response.Iterations[i].BestSecondFuncResolveIndex = lowestSecondIndex;

            }

            return Request.CreateResponse(HttpStatusCode.OK, response, "application/json");
        }

        //private int SumWeights(MatrixOfWeights matrix, Track track)
        //{
        //    int sum = 0;
        //    for (int i = 1; i < track.Length; i++)
        //    {

        //        sum += matrix[track[i - 1], track[i]];

        //    }

        //    return sum;
        //}
    }
}
