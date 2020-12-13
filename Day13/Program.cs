using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using Common;


namespace Day13
{
    class Program
    {
        static void Main(string[] args)
        {
            IEnumerable<string> input = File.ReadLines("../../../input.txt");
            long t0 = long.Parse(input.First());
            List<long> buses = input.Skip(1).First().Split(',').Where(s => s != "x")
                .Select(s => long.Parse(s)).ToList();
            long thebus = -1;
            bool found = false;
            long t = t0;
            while (!found)
            {
                foreach (long b in buses)
                {
                    if (t % b == 0)
                    {
                        thebus = b;
                        found = true;
                        break;
                    }
                }
                if (!found) ++t;
            }
            long solution = thebus * (t - t0);
            Console.WriteLine(solution);

            List<KeyValuePair<long, long>> offsets = input.Skip(1).First().Split(',')
                .Select(s => ((s == "x") ? 0 : long.Parse(s)))
                .ZipWithIndex().Where(kv => kv.Value != 0).Select(kv => new KeyValuePair<long, long>(kv.Value, kv.Key)).OrderBy(kv => kv.Key).ToList();

            t = 0;
            long step = 1;
            int busIdx = 0;

            while (busIdx < offsets.Count)
            {
                KeyValuePair<long, long> bus = offsets[busIdx];
                while ((t + bus.Value) % bus.Key != 0)
                {
                    t += step;
                }
                step = MathUtils.LCD(step, bus.Key);
                busIdx++;
            }

            Console.WriteLine(t);
        }
    }
}
