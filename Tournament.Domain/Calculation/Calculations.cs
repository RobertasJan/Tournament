﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
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

        public static string DisplayPlayerAgeGroup(DateTime birthDate)
        {

            var age = DateTime.Now.Year - birthDate.Year;
            if (birthDate > DateTime.Now.AddYears(-age))
            {
                age--;
            }

            if (age < 11)
            {
                return "U11";
            }
            else if (age >= 11 && age < 13)
            {
                return "U13";
            }
            else if (age >= 13 && age < 15)
            {
                return "U15";
            }
            else if (age >= 15 && age < 17)
            {
                return "U17";
            }
            else if (age >= 17 && age < 19)
            {
                return "U19";
            }
            else if (age >= 19 && age < 35)
            {
                return "Adult";
            }
            else
            {
                return "Veteran";
            }
        }
    }
}
