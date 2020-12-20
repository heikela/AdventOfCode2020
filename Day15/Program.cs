using System;
using System.Collections.Generic;

namespace Day15
{
    class Program
    {
        static void Main(string[] args)
        {
            int t = 0;
            int latest = -1;
            int prev = -1;
            Dictionary<int, int> LastSeen = new Dictionary<int, int>();
            List<int> start = new List<int>() { 6, 3, 15, 13, 1, 0 };
            foreach (int n in start)
            {
                latest = n;
                //Console.WriteLine(latest);
                LastSeen[prev] = t;
                prev = latest;
                t++;
            }

            while (t < 30000000)
            {
                if (LastSeen.ContainsKey(latest))
                {
                    latest = t - LastSeen[latest];
                }
                else
                {
                    latest = 0;
                }
//                Console.WriteLine(latest);
                LastSeen[prev] = t;
                prev = latest;
                t++;
            }

            Console.WriteLine(latest);
        }
    }
}
