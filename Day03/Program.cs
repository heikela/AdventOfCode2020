using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using static Common.Utils;

namespace Day03
{
    public record Point
    {
        public int X { get; }
        public int Y { get; }

        public Point(int x, int y) => (X, Y) = (x, y);

        public static Point operator +(Point a, Point b) => new Point(a.X + b.X, a.Y + b.Y);
    }

    public class Terrain
    {
        private List<List<bool>> Trees;
        private int Width;
        public Terrain(IEnumerable<string> lines)
        {
            Trees = lines.Select(s => s.ToCharArray().Select(c => c == '#').ToList()).ToList();
            Width = Trees[0].Count;
        }

        public int CheckSlope(Point dir)
        {
            Point start = new Point(0, 0);

            Point move(Point pos) => pos + dir;
            bool withinBounds(Point pos) => pos.Y < Trees.Count;
            bool hasTree(Point pos) => Trees[pos.Y][pos.X % Width];

            return Iterate(start, move).TakeWhile(withinBounds).Count(hasTree);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Terrain terrain = new Terrain(File.ReadLines("../../../input.txt"));
            int count = terrain.CheckSlope(new Point(3, 1));
            Console.WriteLine(count);

            List<Point> directions = new List<Point>() {
                new Point(1, 1),
                new Point(3, 1),
                new Point(5, 1),
                new Point(7, 1),
                new Point(1, 2)
            };

            Console.WriteLine(directions.Select<Point, long>(dir => terrain.CheckSlope(dir)).Aggregate((a, b) => a * b));
        }

    }
}
