using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    public record Point
    {
        public int X { get; }
    public int Y { get; }

    public Point(int x, int y) => (X, Y) = (x, y);

    public static Point operator +(Point a, Point b) => new Point(a.X + b.X, a.Y + b.Y);
}
}
