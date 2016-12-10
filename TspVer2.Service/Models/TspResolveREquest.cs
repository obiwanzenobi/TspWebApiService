using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TspVer2.Service.Models
{
    public class TspResolveRequest
    {
        public int popSize { get; set; }
        public double mutationRate { get; set; }
        public double crossoverRate { get; set; }
        public int iterationsNumber { get; set; }
        public List<int> idList { get; set; }
    }
}