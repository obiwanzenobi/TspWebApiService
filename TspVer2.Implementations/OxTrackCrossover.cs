using Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TspVer2.Models;

namespace TspVer2.Implementations
{
    public class OxTrackCrossover : ITrackCrossover
    {
        private Random _random;

        public OxTrackCrossover(Random random)
        {
            _random = random;
        }

        // order crossover, random segment
        public void DoCrossover(Track first, Track second)
        {
            List<int> firstList = GetChildOf(first, second);
            List<int> secondList = GetChildOf(second, first);

            first.TrackPoints = firstList;
            second.TrackPoints = secondList;
        }

        private List<int> GetChildOf(Track first, Track second)
        {
            int startIndex = _random.Next(0, first.Length - 1);
            int range = _random.Next(startIndex + 1, first.Length) - startIndex;
            int lastIndex = startIndex + range;

            int[] firstArray = Enumerable.Repeat<int>(-1, first.Length).ToArray();

            for (int i = startIndex; i < startIndex + range; i++)
            {
                firstArray[i] = first[i];
            }

            bool filled = false;
            bool indexReset = false;

            int destinationIndex = lastIndex;
            int parentIndex = lastIndex;

            while (!filled)
            {

                if (!firstArray.Contains(second[parentIndex]))
                {
                    firstArray[destinationIndex] = second[parentIndex];
                    destinationIndex++;

                    if (destinationIndex >= firstArray.Length && !indexReset)
                    {
                        destinationIndex = 0;
                        indexReset = true;
                    }
                    else if (destinationIndex >= startIndex && indexReset)
                    {
                        filled = true;
                    }
                }

                parentIndex++;
                if (parentIndex >= second.Length)
                {
                    parentIndex = 0;
                }
            }

            return firstArray.ToList();
        }
    }
}
