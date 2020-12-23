using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;
using Common;

namespace Day23
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = "467528193"; // real
            //var input = "389125467"; // sample
            LinkedList<int> cups = new LinkedList<int>(input.ToCharArray().Select(c => int.Parse(c.ToString())));
            int max = cups.Max();
            for (int i = max + 1; i <= 1000000; ++i)
            {
                cups.AddLast(i);
            }
            max = cups.Max();
            int cup;

            for (int t = 0; t < 10000000; ++t)
            {
                //Console.WriteLine($"-- Move {t + 1}");
                //Console.WriteLine($"cups: ({cups.First()}) {string.Join(" ", cups.Skip(1))})");
                cup = cups.First();
                LinkedList<int> pickedUp = new LinkedList<int>(cups.Skip(1).Take(3));
                //Console.WriteLine($"pick up: {string.Join(", ", pickedUp)})");
                cups.RemoveFirst();
                cups.RemoveFirst();
                cups.RemoveFirst();
                cups.RemoveFirst();
                cups.AddLast(cup);
                int dest = cup - 1;
                if (dest == 0)
                {
                    dest = max;
                }
                while (!cups.Contains(dest))
                {
                    dest = dest - 1;
                    if (dest == 0)
                    {
                        dest = max;
                    }
                }
                //Console.WriteLine($"destination: {dest}");
                //Console.WriteLine();
                var destNode = cups.Find(dest);
                while (pickedUp.Any())
                {
                    int firstToPrepend = pickedUp.Last.Value;
                    pickedUp.RemoveLast();
                    cups.AddAfter(destNode, firstToPrepend);
                }
            }

            while (cups.First() != 1)
            {
                cup = cups.First();
                cups.RemoveFirst();
                cups.AddLast(cup);
            }
            cups.RemoveFirst();
            //Console.WriteLine(string.Join("", cups.Select(i => i.ToString())));
            var one = cups.Find(1);
            long first = one.Next.Value;
            long second = one.Next.Next.Value;
            Console.WriteLine(first * second);

            // Not 93467528
        }
    }
}
