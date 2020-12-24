using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;
using Common;

namespace Day24
{
    public record Point
    {
        public int X;
        public int Y;

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static Point operator +(Point a, Point b)
        {
            return new Point(a.X + b.X, a.Y + b.Y);
        }
    }

    class Program
    {
        static IEnumerable<Point> Directions()
        {
            yield return new Point(2, 0);
            yield return new Point(-2, 0);
            yield return new Point(1, -1);
            yield return new Point(1, 1);
            yield return new Point(-1, -1);
            yield return new Point(-1, 1);
        }

        static IEnumerable<Point> Neighbours(Point p)
        {
            return Directions().Select(d => d + p);
        }

        static IEnumerable<Point> ParseLine(string line)
        {
            IEnumerator<Char> iter = line.GetEnumerator();
            while (iter.MoveNext())
            {
                switch (iter.Current)
                {
                    case 'e':
                        yield return new Point(2, 0);
                        break;
                    case 'w':
                        yield return new Point(-2, 0);
                        break;
                    case 's':
                        iter.MoveNext();
                        if (iter.Current == 'e')
                        {
                            yield return new Point(1, 1);
                        }
                        else if (iter.Current == 'w')
                        {
                            yield return new Point(-1, 1);
                        }
                        else throw new Exception("sx");
                        break;
                    case 'n':
                        iter.MoveNext();
                        if (iter.Current == 'e')
                        {
                            yield return new Point(1, -1);
                        }
                        else if (iter.Current == 'w')
                        {
                            yield return new Point(-1, -1);
                        }
                        else throw new Exception("sx");
                        break;
                    default: throw new Exception("x");
                }
            }
        }

        static Dictionary<Point, bool> NextDay(Dictionary<Point, bool> tiles)
        {
            Dictionary<Point, bool> nextGen = new Dictionary<Point, bool>();
            HashSet<Point> toConsider = tiles.Keys.Union(tiles.Keys.SelectMany(p => Neighbours(p))).ToHashSet();
            foreach (Point p in toConsider)
            {
                bool current = tiles.GetOrElse(p, false);
                int neighbours = Neighbours(p).Count(n => tiles.GetOrElse(n, false));
                bool next = false;
                if (current)
                {
                    if (neighbours <= 2 && neighbours >= 1)
                    {
                        next = true; 
                    }
                    else
                    {
                        next = false;
                    }
                }
                else
                {
                    if (neighbours == 2)
                    {
                        next = true;
                    }
                    else
                    {
                        next = false;
                    }
                }
                if (next)
                {
                    nextGen[p] = true;
                }
            }
            return nextGen;
        }

        public static void Print<T>(Dictionary<Point, T> grid, Func<T, char> elemPrinter)
        {
            int minY = Math.Min(grid.Keys.Min(p => p.Y), -20);
            int maxY = grid.Keys.Max(p => p.Y);
            int minX = Math.Min(grid.Keys.Min(p => p.X), -20);
            int maxX = grid.Keys.Max(p => p.X);
            for (int y = minY; y <= maxY; ++y)
            {
                for (int x = minX; x <= maxX; ++x)
                {
                    Point pos = new Point(x, y);
                    if (grid.ContainsKey(pos))
                    {
                        Console.Write(elemPrinter(grid[pos]));
                    }
                    else
                    {
                        Console.Write(' ');
                    }
                }
                Console.WriteLine();
            }
        }

        static void Main(string[] args)
        {
            string fileName = "input.txt";
//            fileName = "sample.txt";
            IEnumerable<string> input = File.ReadLines("../../../" + fileName);
            Dictionary<Point, bool> blackTiles = new Dictionary<Point, bool>();
            foreach (string line in input)
            {
                Point pos = ParseLine(line).Aggregate((a, b) => a + b);
                blackTiles[pos] = !blackTiles.GetOrElse(pos, false);
            }
//            Print(blackTiles, b => b ? '#' : '.');
            var result = blackTiles.Count(kv => kv.Value);
            Console.WriteLine(result);
            Console.WriteLine("-------");
            for (int t = 0; t < 100; ++t)
            {
                blackTiles = NextDay(blackTiles);
                result = blackTiles.Count(kv => kv.Value);
                Console.WriteLine($"Generation {t + 1}");
//                Print(blackTiles, b => b ? '#' : '.');
//                Console.WriteLine(result);
//                Console.WriteLine("-------");
            }
            result = blackTiles.Count(kv => kv.Value);
            Console.WriteLine(result);
            // not 18, 442
        }
    }
}
