using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TspVer2.Models
{
    class Response
    {
        List<Iteration> iterations;
        public class Iteration
        {

            int id;
            List<ResolveContainer> resolves;
        }

        public class ResolveContainer
        {
            int cost1;
            int cost2;

            List<int> orderedIdList;
        }


    }
}
