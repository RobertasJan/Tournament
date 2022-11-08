using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tournament.Domain.Calculation
{
    public static class Calculations
    {
        public static int GetCountOfRounds(int playerCount)
        {
            if (playerCount < 2) return 1;
            var baseNumber = Math.Log2(playerCount);
            return (int)Math.Ceiling(baseNumber);
        }

        public static List<int> SortSeeds(int seedCount)
        {
            var seeds = Enumerable.Range(1, seedCount);
            var seedList = new List<int[]>();
            foreach (var seed in seeds)
            {
                seedList.Add(new int[] { seed });
            }
            var sorted = RecursiveSeedMatch(seedList);
            var sortedList = new List<int>();
            foreach (var sortedArray in sorted)
            {
                foreach (var array in sortedArray)
                {
                    sortedList.Add(array);
                }
            }
            return sortedList;
        }
        private static List<int[]> RecursiveSeedMatch(List<int[]> seeds)
        {
            if (seeds.Count == 2)
            {
                seeds[1] = seeds[1].Reverse().ToArray();
                return seeds;
            }
            var newList = new List<int[]>();
            for (var i = 0; i < seeds.Count / 2; i++)
            {
                var seed = seeds[i];
                var newSeed = seed.Concat(seeds[seeds.Count - 1 - i]).ToArray();
                newList.Add(newSeed);
            }

            return RecursiveSeedMatch(newList);
        }
    }
}
