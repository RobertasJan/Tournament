using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tournament.Shared.Games
{
    public static class Deuces
    {

        public static readonly Dictionary<int, int> DeucesList = new Dictionary<int, int>()
        {
            { 11, 15 },
            { 15, 17 },
            { 21, 30 },
        };
    }
}
