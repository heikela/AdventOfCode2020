using System;
using System.Collections.Generic;

namespace Common
{
    public static class Utils
    {
        public static IEnumerable<T> Iterate<T>(T initial, Func<T, T> f)
        {
            T current = initial;
            while (true)
            {
                yield return current;
                current = f(current);
            }
        }

    }
}
