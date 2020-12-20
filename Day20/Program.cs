using System;
using System.IO;
using Common;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Day20
{
    public record Point
    {
        public int X { get; }
        public int Y { get; }

        public Point(int x, int y) => (X, Y) = (x, y);

        public static Point operator +(Point a, Point b) => new Point(a.X + b.X, a.Y + b.Y);

        public static Point operator *(int m, Point a) => new Point(m * a.X, m * a.Y);
    }

    public record Connection(int TileId, int EdgeCode);

    public static class SparseGrid
    {
        public static Dictionary<Point, char> Read(IEnumerable<string> lines)
        {
            return lines
                .ZipWithIndex()
                .Select(indexedLine =>
                indexedLine.Value
                    .ZipWithIndex()
                    .Select(indexedChar => KeyValuePair.Create<Point, char>(
                        new Point(indexedChar.Key, indexedLine.Key), indexedChar.Value))
                )
                .SelectMany(x => x)
                .ToDictionary();
        }

        public static Dictionary<Point, char> ReadFromFile(string filename)
        {
            return Read(File.ReadLines(filename));
        }

        public static void Print<T>(Dictionary<Point, T> grid, Func<T, char> elemPrinter)
        {
            int minY = grid.Keys.Min(p => p.Y);
            int maxY = grid.Keys.Max(p => p.Y);
            int minX = grid.Keys.Min(p => p.X);
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
                Console.WriteLine(y);
            }
        }
    }
    class Program
    {
        static IEnumerable<int> PotentialEdges(Dictionary<Point, char> tile)
        {
            int minX = tile.Keys.Select(p => p.X).Min();
            int minY = tile.Keys.Select(p => p.Y).Min();
            int maxX = tile.Keys.Select(p => p.X).Max();
            int maxY = tile.Keys.Select(p => p.Y).Max();

            int maxForward = 0;
            int maxBackward = 0;
            int minForward = 0;
            int minBackward = 0;
            for (int dx = 0; dx <= maxX - minX; ++dx)
            {
                maxForward *= 2;
                maxBackward *= 2;
                minForward *= 2;
                minBackward *= 2;
                maxForward += tile[new Point(minX + dx, maxY)] == '#' ? 1 : 0;
                minForward += tile[new Point(minX + dx, minY)] == '#' ? 1 : 0;
                maxBackward += tile[new Point(maxX - dx, maxY)] == '#' ? 1 : 0;
                minBackward += tile[new Point(maxX - dx, minY)] == '#' ? 1 : 0;
            }
            yield return maxForward;
            yield return maxBackward;
            yield return minForward;
            yield return minBackward;

            maxForward = 0;
            maxBackward = 0;
            minForward = 0;
            minBackward = 0;
            for (int dy = 0; dy <= maxY - minY; ++dy)
            {
                maxForward *= 2;
                maxBackward *= 2;
                minForward *= 2;
                minBackward *= 2;
                maxForward += tile[new Point(maxX, minY + dy)] == '#' ? 1 : 0;
                minForward += tile[new Point(minX, minY + dy)] == '#' ? 1 : 0;
                maxBackward += tile[new Point(maxX, maxY - dy)] == '#' ? 1 : 0;
                minBackward += tile[new Point(minX, maxY - dy)] == '#' ? 1 : 0;
            }
            yield return maxForward;
            yield return maxBackward;
            yield return minForward;
            yield return minBackward;
        }

        static IEnumerable<Dictionary<Point, char>> Orientations(Dictionary<Point, char> tile)
        {
            int minX = tile.Keys.Select(p => p.X).Min();
            int minY = tile.Keys.Select(p => p.Y).Min();
            int maxX = tile.Keys.Select(p => p.X).Max();
            int maxY = tile.Keys.Select(p => p.Y).Max();
            foreach (Point start in new List<Point>() {
                new Point(minX, minY),
                new Point(minX, maxY),
                new Point(maxX, minY),
                new Point(maxX, maxY)})
            {
                Point DirXfromX = start.X < maxX ? new Point(1, 0) : new Point(-1, 0);
                Point DirXfromY = start.Y < maxY ? new Point(0, 1) : new Point(0, -1);
                foreach (Point DirX in new List<Point>() { DirXfromX, DirXfromY })
                {
                    Point DirY;
                    if (DirX.X != 0)
                    {
                        DirY = start.Y < maxY ? new Point(0, 1) : new Point(0, -1);
                    }
                    else
                    {
                        DirY = start.X < maxX ? new Point(1, 0) : new Point(-1, 0);
                    }
                    Dictionary<Point, char> output = new Dictionary<Point, char>();
                    int y = 0;
                    while (tile.ContainsKey(start + y * DirY))
                    {
                        int x = 0;
                        Point pos = start + (y * DirY) + (x * DirX);
                        while (tile.ContainsKey(pos))
                        {
                            output.Add(new Point(x, y), tile[pos]);
                            ++x;
                            pos = start + (y * DirY) + (x * DirX);
                        }
                        ++y;
                    }
                    yield return output;
                }
            }
        }

        static Dictionary<Point, char> RemoveEdge(Dictionary<Point, char> original)
        {
            Dictionary<Point, char> result = new Dictionary<Point, char>();
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    result.Add(new Point(x, y), original[new Point(x + 1, y + 1)]);
                }
            }
            return result;
        }

        static void Main(string[] args)
        {
            string input = "../../../input.txt";
            string sample = "../../../sample.txt";
            int gridDim = 12;
            var tiles = File.ReadLines(input).Paragraphs();

            var tilesById = tiles.Select(p => new KeyValuePair<int, Dictionary<Point, char>>(int.Parse(p.First().Substring(5, 4)), SparseGrid.Read(p.Skip(1)))).ToDictionary();

            var tilesByEdge = new Dictionary<int, HashSet<int>>();
            foreach (var tile in tilesById)
            {
                foreach (var edge in PotentialEdges(tile.Value)) {
                    if (tilesByEdge.ContainsKey(edge))
                    {
                        tilesByEdge[edge].Add(tile.Key);
                    }
                    else
                    {
                        tilesByEdge[edge] = new HashSet<int>() { tile.Key };
                    }
                }
            }

            var tileToTile = new Dictionary<int, HashSet<Connection>>();



            foreach (var tile in tilesById)
            {
                int tileId = tile.Key;
                tileToTile.Add(tileId, new HashSet<Connection>());
                foreach(var edge in PotentialEdges(tile.Value))
                {
                    foreach (var otherTile in tilesByEdge[edge])
                    {
                        if (otherTile == tile.Key)
                        {
                            continue;
                        }
                        tileToTile[tileId].Add(new Connection(otherTile, edge));
                    }
                }
            }

            tileToTile.Values.GroupBy(s => s.Count()).Select(g => { Console.WriteLine($"{g.Count()} tiles with {g.Key} possible connections"); return 0; }).ToList();
            tilesByEdge.Values.GroupBy(s => s.Count()).Select(g => { Console.WriteLine($"{g.Count()} possible edges with {g.Key} tiles sharing them"); return 0; }).ToList();

            Console.WriteLine(tileToTile.Where(kv => kv.Value.Count() == 4).Select(kv => kv.Key).Aggregate(1L, (a, b) => a * b));

            Dictionary<Point, KeyValuePair<int, Dictionary<Point, Char>>> tilesByPos = new Dictionary<Point, KeyValuePair<int, Dictionary<Point, char>>>();

            int topLeftId = tileToTile.Where(kv => kv.Value.Count() == 4).First().Key;
            var topLeftTile = tilesById[topLeftId];

            int firstEdge = tileToTile[topLeftId].First().EdgeCode;
            IEnumerable<int> secondEdgeCandidates = tileToTile[topLeftId].Skip(1).Select(x => x.EdgeCode);

            int RightEdge(Dictionary<Point, Char> tile) => PotentialEdges(tile).Skip(4).First();
            int ReverseRightEdge(Dictionary<Point, Char> tile) => PotentialEdges(tile).Skip(5).First();
            int BottomEdge(Dictionary<Point, Char> tile) => PotentialEdges(tile).Skip(0).First();
            int LeftEdge(Dictionary<Point, Char> tile) => PotentialEdges(tile).Skip(6).First();
            int TopEdge(Dictionary<Point, Char> tile) => PotentialEdges(tile).Skip(2).First();

            var rightMatches = Orientations(topLeftTile).Where(tile => RightEdge(tile) == firstEdge || ReverseRightEdge(tile) == firstEdge);
            var bottomMatch = rightMatches.First(tile => secondEdgeCandidates.Any(s => BottomEdge(tile) == s));

            Point tilePos = new Point(0, 0);

            tilesByPos.Add(tilePos, new KeyValuePair<int, Dictionary<Point, Char>>(topLeftId, bottomMatch));

            int y = 0;
            for (int x = 1; x < gridDim; ++x)
            {
                var prev = tilesByPos[new Point(x - 1, y)];
                int edgeToMatch = RightEdge(prev.Value);
                int matchingTileId = tilesByEdge[edgeToMatch].First(id => id != prev.Key);
                var matchingOrientation = Orientations(tilesById[matchingTileId]).First(tile => LeftEdge(tile) == edgeToMatch);
                tilesByPos.Add(new Point(x, y), new KeyValuePair<int, Dictionary<Point, char>>(matchingTileId, matchingOrientation));
            }

            for (y = 1; y < gridDim; ++y)
            {
                for (int x = 0; x < gridDim; ++x)
                {
                    var above = tilesByPos[new Point(x, y - 1)];
                    int edgeToMatch = BottomEdge(above.Value);
                    int matchingTileId = tilesByEdge[edgeToMatch].First(id => id != above.Key);
                    var matchingOrientation = Orientations(tilesById[matchingTileId]).First(tile => TopEdge(tile) == edgeToMatch);
                    tilesByPos.Add(new Point(x, y), new KeyValuePair<int, Dictionary<Point, char>>(matchingTileId, matchingOrientation));
                }
            }

            for (y = 0; y < gridDim; ++y)
            {
                for (int x = 0; x < gridDim; ++x)
                {
                    Console.Write($"{tilesByPos[new Point(x, y)].Key} ");
                }
                Console.WriteLine(" ");
            }

            Dictionary<Point, char> combined = new Dictionary<Point, char>();

            for (int gy = 0; gy < gridDim; gy++)
            {
                for (int gx = 0; gx < gridDim; gx++)
                {
                    var tile = tilesByPos[new Point(gx, gy)].Value;
                    for (y = 0; y < 10; y++)
                    {
                        for (int x = 0; x < 10; ++x)
                        {
                            combined.Add(new Point(10 * gx + x, 10 * gy + y), tile[new Point(x, y)]);
                        }
                    }
                }
            }
            SparseGrid.Print(combined, c => c);

            combined = new Dictionary<Point, char>();

            for (int gy= 0; gy < gridDim; gy++)
            {
                for (int gx = 0; gx < gridDim; gx++)
                {
                    var tile = RemoveEdge(tilesByPos[new Point(gx, gy)].Value);
                    for (y = 0; y < 8; y++)
                    {
                        for (int x = 0; x < 8; ++x)
                        {
                            combined.Add(new Point(8 * gx + x, 8 * gy + y), tile[new Point(x, y)]);
                        }
                    }
                }
            }
            SparseGrid.Print(combined, c => c);

            var seaMonster = SparseGrid.ReadFromFile("../../../seamonster.txt");

            var filtered = combined.ToDictionary();

            foreach (var monster in Orientations(seaMonster))
            {
                Console.WriteLine("Checking for Monster:");
                Console.WriteLine("------");
                SparseGrid.Print(monster, c => c);
                Console.WriteLine("------");
                for (int y0 = 0; y0 < gridDim * 8; ++y0)
                {
                    for (int x0 = 0; x0 < gridDim * 8; ++x0)
                    {
                        Point pos0 = new Point(x0, y0);
                        var toCheck = monster.Where(kv => kv.Value == '#').Select(kv => kv.Key + pos0);
                        if (toCheck.All(pos => combined.GetOrElse(pos, '.') == '#'))
                        {
                            foreach (var pos in toCheck)
                            {
                                filtered[pos] = 'O';
                            }
                        }
                    }
                }
            }

            SparseGrid.Print(filtered, c => c);
            Console.WriteLine(filtered.Count(kv => kv.Value == '#'));
            // 5328 is too high
            // 4747 is too high
            // 2179 is too high
        }
    }
}
