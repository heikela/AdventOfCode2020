using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace Day03
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
        public static IEnumerable<T> Iterate<T>(T initial, Func<T, T> f)
        {
            T current = initial;
            while (true)
            {
                yield return current;
                current = f(current);
            }
        }

        static void Main(string[] args)
        {
            List<List<bool>> trees = File.ReadLines("../../../input.txt").Select(s => s.ToCharArray().Select(c => c == '#').ToList()).ToList();
            int count = CheckSlope(trees, new Point(3, 1));
            Console.WriteLine(count);

            List<Point> directions = new List<Point>() {
                new Point(1, 1),
                new Point(3, 1),
                new Point(5, 1),
                new Point(7, 1),
                new Point(1, 2)
            };

            Console.WriteLine(directions.Select<Point, long>(dir => CheckSlope(trees, dir)).Aggregate((a, b) => a * b));
        }

        private static int CheckSlope(List<List<bool>> trees, Point dir)
        {
            
            int width = trees[0].Count;
            return Iterate(new Point(0, 0), p => p + dir).TakeWhile(p => p.Y < trees.Count).Count(p => trees[p.Y][p.X % width]);
        }
    }
}
