using Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TspVer2.Models;

namespace TspVer2.Implementations
{
    public class TrackInversionMutationService : ITrackMutator
    {

        private Random _random;

        public TrackInversionMutationService(Random rnd)
        {
            _random = rnd;
        }

        public void MutateTrack(Track track)
        {
            int startIndex = _random.Next(0, track.Length - 1);
            int range = _random.Next(startIndex + 1, track.Length) - startIndex;

            List<int> tempList = new List<int>(track.TrackPoints.GetRange(startIndex, range).ToList());
            tempList.Reverse();

            for (int i = 0; i < tempList.Count; i++)
            {
                track[startIndex + i] = tempList[i];
            }
        }
    }
}
