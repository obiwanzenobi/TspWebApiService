using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TspVer2.Models
{
    public class Track
    {
        public Track(List<int> trackPoints)
        {
            this.TrackPoints = trackPoints;
        }

        public int[] TrackCosts { get; set; }
        public List<int> TrackPoints { get; set; }
        public int Length
        {
            get
            {
                return TrackPoints.Count;
            }
        }

        public int this[int i]
        {
            get
            {
                return TrackPoints[i];
            }
            set
            {
                TrackPoints[i] = value;
            }
        }
    }
}
