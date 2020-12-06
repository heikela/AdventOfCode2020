using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Common;

namespace Day06
{
    class Program
    {
        static void Main(string[] args)
        {
            IEnumerable<List<string>> grouped = File.ReadLines("../../../input.txt").Paragraphs();
            int result = grouped
                .Select(g => string.Concat(g).ToHashSet().Count)
                .Sum();
            Console.WriteLine(result);

            int result2 = grouped
                .Select(g => g.Select(s => s.ToHashSet()).Aggregate((a, b) => { a.IntersectWith(b); return a; }).Count)
                .Sum();
            Console.WriteLine(result2);
        }
    }
}
