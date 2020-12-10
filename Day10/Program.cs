using System;
using Common;
using System.Linq;
using System.Collections.Generic;
using System.IO;

namespace Day10
{
    class Program
    {
        static void Main(string[] args)
        {
            List<int> joltages = File.ReadLines("../../../input.txt").Select(int.Parse).OrderBy(x => x).ToList();
            List<int> differenceCount = new List<int>() { 0, 0, 1 };
            int prev = 0;
            for (int i = 0; i < joltages.Count(); ++i)
            {
                int curr = joltages[i];
                int diff = curr - prev;
                differenceCount[diff - 1]++;
                prev = curr;
            }
            Console.WriteLine(differenceCount[0] * differenceCount[2]);

            joltages.Add(joltages[joltages.Count - 1] + 3);

            Dictionary<int, long> waysToGetTo = new Dictionary<int, long>();
            waysToGetTo.Add(0, 1);
            foreach (int joltage in joltages)
            {
                long ways = 0;
                for (prev = joltage - 3; prev < joltage; ++prev)
                {
                    ways += waysToGetTo.GetOrElse(prev, 0);
                }
                waysToGetTo.Add(joltage, ways);
            }
            Console.WriteLine(waysToGetTo.Values.Max());
        }
    }
}
