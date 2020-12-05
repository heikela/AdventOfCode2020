using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Day05
{
    class Program
    {
        static int Parse(string code)
        {
            int seat = 0;
            foreach (Char c in code)
            {
                seat *= 2;
                if (c == 'B' || c == 'R')
                {
                    seat += 1;
                }
            }
            return seat;
        }

        static void Main(string[] args)
        {
            HashSet<int> seats = File.ReadLines("../../../input.txt").Select(Parse).ToHashSet();
            Console.WriteLine(seats.Max());
            int i = seats.Min();
            while (seats.Contains(i))
            {
                ++i;
            }
            Console.WriteLine(i);
        }
    }
}
