using System;
using Common;
using System.Linq;
using System.Collections.Generic;
using System.IO;

namespace Day11
{
    public record Point
    {
        public int X { get; }
        public int Y { get; }

        public Point(int x, int y) => (X, Y) = (x, y);

        public static Point operator +(Point a, Point b) => new Point(a.X + b.X, a.Y + b.Y);
    }

    class Program
    {
        static bool IsSame(Dictionary<Point, Char> a, Dictionary<Point, Char> b)
        {
            foreach (var kv in a)
            {
                if (!b.Contains(kv))
                {
                    return false;
                }
            }
            foreach (var kv in b)
            {
                if (!a.Contains(kv))
                {
                    return false;
                }
            }
            return true;
        }

        static IEnumerable<Point> Directions()
        {
            for (int dx = -1; dx < 2; ++dx)
            {
                for (int dy = -1; dy < 2; ++dy)
                {
                    if (dx == 0 && dy == 0)
                    {
                        continue;
                    }
                    yield return new Point(dx, dy);
                }
            }
            yield break;
        }

        static Char ApplyRule(Point pos, Dictionary<Point, Char> old)
        {
            int occupiedNeighbours = Directions().Count(dir => old.GetOrElse(pos + new Point(dx, dy), '.') == '#');
            switch (old[pos])
            {
                case '.': return '.';
                case 'L':
                    if (occupiedNeighbours == 0)
                    {
                        return '#';
                    }
                    else
                    {
                        return 'L';
                    }
                case '#':
                    if (occupiedNeighbours >= 4)
                    {
                        return 'L';
                    }
                    else
                    {
                        return '#';
                    }
                default: return '.';
            }
        }

        static Char ApplyRule2(Point pos, Dictionary<Point, Char> old)
        {
            int occupiedNeighbours = 0;
            for (int dx = -1; dx < 2; ++dx)
            {
                for (int dy = -1; dy < 2; ++dy)
                {
                    if (dx == 0 && dy == 0)
                    {
                        continue;
                    }
                    Point dir = new Point(dx, dy);
                    Char neighbour = old.GetOrElse(
                        Utils.Iterate(pos + dir, pos => pos + dir).SkipWhile(pos => old.ContainsKey(pos) && old[pos] == '.').First(),
                        '.');
                    if (neighbour == '#')
                    {
                        ++occupiedNeighbours;
                    }
                }
            }
            switch (old[pos])
            {
                case '.': return '.';
                case 'L':
                    if (occupiedNeighbours == 0)
                    {
                        return '#';
                    }
                    else
                    {
                        return 'L';
                    }
                case '#':
                    if (occupiedNeighbours >= 5)
                    {
                        return 'L';
                    }
                    else
                    {
                        return '#';
                    }
                default: return '.';
            }
        }

        static void Main(string[] args)
        {
            Dictionary<Point, Char> start = new Dictionary<Point, char>();
            int y = 0;
            foreach (string line in File.ReadLines("../../../input.txt"))
            {
                int x = 0;
                foreach (Char c in line)
                {
                    start.Add(new Point(x, y), c);
                    ++x;
                }
                ++y;
            }

            Dictionary<Point, Char> current = start.ToDictionary();

            Dictionary<Point, Char> old;
            do
            {
                old = current;
                current = new Dictionary<Point, char>();
                foreach (var pos in old.Keys)
                {
                    current.Add(pos, ApplyRule(pos, old));
                }
            } while (!IsSame(current, old));

            Console.WriteLine(current.Values.Count(c => c == '#'));

            // part 2
            current = start.ToDictionary();

            do
            {
                old = current;
                current = new Dictionary<Point, char>();
                foreach (var pos in old.Keys)
                {
                    current.Add(pos, ApplyRule2(pos, old));
                }
            } while (!IsSame(current, old));

            Console.WriteLine(current.Values.Count(c => c == '#'));

        }
    }
}
