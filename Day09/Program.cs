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
            List<long> entries = File.ReadLines("../../../input.txt").Select(long.Parse).ToList();
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

            long invalidNumber = toCheck.Current;
            Console.WriteLine(invalidNumber);

            // part 2

            for (int i = 0; i < entries.Count(); ++i)
            {
                long sum = 0;
                for (int j = i; j < entries.Count(); ++j)
                {
                    sum += entries[j];
                    if (sum == invalidNumber)
                    {
                        IEnumerable<long> range = entries.Skip(i).Take(j - i + 1);
                        Console.WriteLine(range.Min() + range.Max());
                    }
                    if (sum > invalidNumber)
                    {
                        break;
                    }
                }
            }
        }
    }
}
