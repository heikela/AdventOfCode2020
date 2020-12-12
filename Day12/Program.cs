using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;

namespace Day12
{
    public record Move
    {
        public Char Action;
        public int Number;

        public static Move Parse(string line)
        {
            return new Move
            {
                Action = line[0],
                Number = int.Parse(line.Substring(1))
            };
        }
    };

    public record Point
    {
        public int X { get; }
        public int Y { get; }

        public Point(int x, int y) => (X, Y) = (x, y);

        public static Point operator +(Point a, Point b) => new Point(a.X + b.X, a.Y + b.Y);
    };


    public record Ship
    {
        public Point Pos;
        public Point Heading;

        public Ship Move(Move move)
        {
            switch (move.Action)
            {
                case 'N':
                    return this with { Pos = this.Pos + new Point(0, -move.Number) };
                case 'S':
                    return this with { Pos = this.Pos + new Point(0, move.Number) };
                case 'E':
                    return this with { Pos = this.Pos + new Point(move.Number, 0) };
                case 'W':
                    return this with { Pos = this.Pos + new Point(-move.Number, 0) };
                case 'R':
                    switch (move.Number)
                    {
                        case 90:
                            return this with { Heading = new Point(-this.Heading.Y, this.Heading.X) };
                        case 180:
                            return this with { Heading = new Point(-this.Heading.X, -this.Heading.Y) };
                        case 270:
                            return this with { Heading = new Point(this.Heading.Y, -this.Heading.X) };
                        default:
                            throw new Exception("Unknown turn");
                    }
                case 'L':
                    return Move(new Move { Action = 'R', Number = 360 - move.Number });
                case 'F':
                    return this with { Pos = this.Pos + new Point(this.Heading.X * move.Number, this.Heading.Y * move.Number) };
                default: throw new Exception("Unknown move");
            }
        }
    }

    public record Ship2
    {
        public Point Pos;
        public Point Waypoint;

        public Ship2 Move(Move move)
        {
            switch (move.Action)
            {
                case 'N':
                    return this with { Waypoint = this.Waypoint + new Point(0, -move.Number) };
                case 'S':
                    return this with { Waypoint = this.Waypoint + new Point(0, move.Number) };
                case 'E':
                    return this with { Waypoint = this.Waypoint + new Point(move.Number, 0) };
                case 'W':
                    return this with { Waypoint = this.Waypoint + new Point(-move.Number, 0) };
                case 'R':
                    switch (move.Number)
                    {
                        case 90:
                            return this with { Waypoint = new Point(-this.Waypoint.Y, this.Waypoint.X) };
                        case 180:
                            return this with { Waypoint = new Point(-this.Waypoint.X, -this.Waypoint.Y) };
                        case 270:
                            return this with { Waypoint = new Point(this.Waypoint.Y, -this.Waypoint.X) };
                        default:
                            throw new Exception("Unknown turn");
                    }
                case 'L':
                    return Move(new Move { Action = 'R', Number = 360 - move.Number });
                case 'F':
                    return this with { Pos = this.Pos + new Point(this.Waypoint.X * move.Number, this.Waypoint.Y * move.Number) };
                default: throw new Exception("Unknown move");
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            IEnumerable<Move> moves = File.ReadLines("../../../input.txt")
                .Select(Move.Parse);
            Ship final = moves.Aggregate(new Ship() { Heading = new Point(1, 0), Pos = new Point(0, 0) }, (ship, move) => ship.Move(move));
            Console.WriteLine(Math.Abs(final.Pos.X) + Math.Abs(final.Pos.Y));

            Ship2 final2 = moves.Aggregate(new Ship2() { Waypoint = new Point(10, -1), Pos = new Point(0, 0) }, (ship, move) => ship.Move(move));
            Console.WriteLine(Math.Abs(final2.Pos.X) + Math.Abs(final2.Pos.Y));
        }
    }
}
