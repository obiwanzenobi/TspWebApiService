using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TspVer2.Service.Models
{
    public class TspResolveResponse
    {

        public Iteration[] Iterations { get; set; }
        public class Iteration
        {

            public int Id { get; set; }
            public List<ResolveContainer> Resolves { get; set; }

            public int BestFirstFuncResolveIndex { get; set; }
            public int BestSecondFuncResolveIndex { get; set; }
        }

        public class ResolveContainer
        {
            public double FirstCost { get; set; }
            public double SecondCost { get; set; }

            public List<int> OrderedIdList { get; set; }
        }


    }
}