using System;
using System.IO;
using Common;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Day17
{
    public record Point3D
    {
        public int X;
        public int Y;
        public int Z;
        public int W;

        public Point3D(int x, int y, int z = 0, int w = 0) => (X, Y, Z, W) = (x, y, z, w);

        public static Point3D operator +(Point3D a, Point3D b) => new Point3D(a.X + b.X, a.Y + b.Y, a.Z + b.Z, a.W + b.W);
    }

    class Program
    {
        public static IEnumerable<Point3D> Directions()
        {
            for (int x = -1; x < 2; ++x)
            {
                for (int y = -1; y < 2; ++y)
                {
                    for (int z = -1; z < 2; ++z)
                    {
                        for (int w = -1; w < 2; ++w)
                        {
                            if (x == 0 && y == 0 && z == 0 && w == 0)
                            {
                                continue;
                            }
                            else
                            {
                                yield return new Point3D(x, y, z, w);
                            }
                        }
                    }
                }
            }
        }

        public static IEnumerable<Point3D> Neighbours(Point3D p)
        {
            return Directions().Select(d => p + d);
        }

        static void Main(string[] args)
        {
            HashSet<Point3D> start = new HashSet<Point3D>();
            int y = 0;
//            foreach (string line in File.ReadLines("../../../sample.txt"))
            foreach (string line in File.ReadLines("../../../input.txt"))
                {
                    int x = 0;
                foreach (char c in line)
                {
                    if (c == '#')
                    {
                        start.Add(new Point3D(x, y));
                    }
                    ++x;
                }
                y++;
            }

            int t = 0;
            HashSet<Point3D> current = start;
            while (t < 6)
            {
                HashSet<Point3D> considered = new HashSet<Point3D>();
                HashSet<Point3D> next = new HashSet<Point3D>();
                foreach (var old in current)
                {
                    foreach(var point in Neighbours(old))
                    {
                        if (considered.Contains(point))
                        {
                            continue;
                        }
                        else
                        {
                            int activeNeighbours = Neighbours(point).Count(n => current.Contains(n));
                            if (current.Contains(point))
                            {
                                if (activeNeighbours == 2 || activeNeighbours == 3)
                                {
                                    next.Add(point);
                                }
                            }
                            else
                            {
                                if (activeNeighbours == 3)
                                {
                                    next.Add(point);
                                }
                            }
                            considered.Add(point);
                        }
                    }
                }
                current = next;
                ++t;
            }
            Console.WriteLine(current.Count());
        }
    }
}
