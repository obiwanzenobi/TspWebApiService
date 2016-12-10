using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TspVer2.Models
{
    public class Population
    {

        public Population(Track[] population)
        {
            Workers = population;
        }

        public Track[] Workers { get; set; }
        public int Length
        {
            get
            {
                return Workers.Length;
            }
        }

        public Track this[int i]
        {
            get
            {
                return Workers[i];
            }

            set
            {
                Workers[i] = value;
            }
        }

    }
}
