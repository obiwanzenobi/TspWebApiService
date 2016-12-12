using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TspVer2.Service.Models
{
    public class TspResolveRequest
    {
        public int PopSize { get; set; }
        public double MutationRate { get; set; }
        public double CrossoverRate { get; set; }
        public int IterationsNumber { get; set; }
        public int[] IdList { get; set; }
        public int[,] WeightMatrix { get; set; }
    }
}