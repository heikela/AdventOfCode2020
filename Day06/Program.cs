using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Day06
{
    class Program
    {
        public static IEnumerable<List<TElem>> SplitWhen<TElem>(IEnumerable<TElem> seq,
            Func<TElem, TElem, bool> splitPredicate)
        {
            List<TElem> prefix = new List<TElem>();
            foreach (TElem next in seq)
            {
                if (prefix.Count >= 1)
                {
                    if (splitPredicate(prefix.Last(), next))
                    {
                        yield return prefix;
                        prefix = new List<TElem>();
                    }
                }
                prefix.Add(next);
            }
            if (prefix.Any())
            {
                yield return prefix;
            }
            yield break;
        }

        static void Main(string[] args)
        {
            int result = SplitWhen(File.ReadLines("../../../input.txt"), (p, n) => (n.Trim().Count() == 0))
                .Select(g => string.Concat(g).ToHashSet().Count)
                .Sum();
            Console.WriteLine(result);

            int result2 = SplitWhen(File.ReadLines("../../../input.txt"), (p, n) => (n.Trim().Count() == 0))
                .Select(g => g.Where(s => s.Trim().Length > 0).Select(s => s.ToHashSet()).Aggregate((a, b) => { a.IntersectWith(b); return a; }).Count)
                .Sum();
            Console.WriteLine(result2);
        }
    }
}
