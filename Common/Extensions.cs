using System.Linq;
using System.Collections.Generic;
using System;

namespace Common
{
    public static class Extensions
    {
        public static IEnumerable<List<TElem>> SplitWhen<TElem>(this IEnumerable<TElem> seq,
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
    }
}
