using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;

namespace Day09
{
    class Program
    {
        static void Main(string[] args)
        {
            IEnumerable<long> entries = File.ReadLines("../../../input.txt").Select(long.Parse);
            Queue<long> available = new Queue<long>(entries.Take(25));
            IEnumerator<long> toCheck = entries.Skip(25).GetEnumerator();

            bool IsValid(long number) {
                List<long> sorted = available.AsEnumerable().OrderBy(x => x).ToList();
                int lowIdx = 0;
                int highIdx = sorted.Count() - 1;
                while (lowIdx < highIdx)
                {
                    long sum = sorted[lowIdx] + sorted[highIdx];
                    if (sum == number)
                    {
                        return true;
                    }
                    else if (sum < number)
                    {
                        ++lowIdx;
                    }
                    else
                    {
                        --highIdx;
                    }
                }
                return false;
            }

            while (toCheck.MoveNext() && IsValid(toCheck.Current))
            {
                available.Dequeue();
                available.Enqueue(toCheck.Current);
            }
            Console.WriteLine(toCheck.Current);
        }
    }
}
