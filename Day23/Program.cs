using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;
using Common;

namespace Day23
{
    class Program
    {
        public record CircleNode {
            public HashSet<int> Contents;
            public LinkedList<int> Ordered;
            public int Count;

            public CircleNode()
            {
                Contents = new HashSet<int>();
                Ordered = new LinkedList<int>();
                Count = 0;
            }

            public void AddLast(int n)
            {
                Contents.Add(n);
                Ordered.AddLast(n);
                ++Count;
            }

            public void RemoveFirst()
            {
                int toRemove = Ordered.First();
                Ordered.RemoveFirst();
                Contents.Remove(toRemove);
                --Count;
            }

            public void AddAfter(LinkedListNode<int> cursor, int n)
            {
                Contents.Add(n);
                Ordered.AddAfter(cursor, n);
                ++Count;
            }
        }

        public class CupCircle
        {
            public class CircleCursor
            {
                public LinkedListNode<CircleNode> Outer;
                public LinkedListNode<int> Inner;

                public CircleCursor Next()
                {
                    if (Inner.Next != null)
                    {
                        return new CircleCursor()
                        {
                            Outer = this.Outer,
                            Inner = this.Inner.Next
                        };
                    }
                    else
                    {
                        LinkedListNode<CircleNode> nextOuter = Outer.Next != null ? Outer.Next : Outer.List.First;
                        return new CircleCursor()
                        {
                            Outer = nextOuter,
                            Inner = nextOuter.Value.Ordered.First
                        };
                    }
                }
            }

            private LinkedList<CircleNode> Nodes;
            public CupCircle(IEnumerable<int> initial)
            {
                Nodes = new LinkedList<CircleNode>();
                CircleNode currentNode = new CircleNode();
                Nodes.AddFirst(currentNode);
                foreach (int i in initial)
                {
                    if (currentNode.Count >= 1000)
                    {
                        currentNode = new CircleNode();
                        Nodes.AddLast(currentNode);
                    }
                    currentNode.AddLast(i);
                }
            }

            public IEnumerable<int> AsEnumerable()
            {
                foreach (var node in Nodes)
                {
                    foreach (int i in node.Ordered)
                    {
                        yield return i;
                    }
                }
            }

            public void AddLast(int n)
            {
                CircleNode lastNode = Nodes.Last();
                if (lastNode.Count >= 1000)
                {
                    Nodes.AddLast(new CircleNode());
                    lastNode = Nodes.Last();
                }
                lastNode.AddLast(n);
            }

            public int First()
            {
                return Nodes.First().Ordered.First();
            }

            public void RemoveFirst()
            {
                Nodes.First.Value.RemoveFirst();
                if (Nodes.First.Value.Count == 0)
                {
                    Nodes.RemoveFirst();
                }
            }

            public CircleCursor Find(int n)
            {
                LinkedListNode<CircleNode> outer = Nodes.First;
                while (!outer.Value.Contents.Contains(n))
                {
                    outer = outer.Next;
                }
                return new CircleCursor()
                {
                    Outer = outer,
                    Inner = outer.Value.Ordered.Find(n)
                };
            }

            public void AddAfter(CircleCursor cursor, IEnumerable<int> toAdd)
            {
                CircleNode node = cursor.Outer.Value;
                LinkedListNode<int> current = cursor.Inner;
                foreach (int n in toAdd)
                {
                    node.AddAfter(current, n);
                    current = current.Next;
                }
                if (node.Count > 2000)
                {
                    CircleNode newNode = new CircleNode();
                    Nodes.AddBefore(cursor.Outer, newNode);
                    for (int i = 0; i < 1000; ++i)
                    {
                        int val = node.Ordered.First.Value;
                        node.RemoveFirst();
                        newNode.AddLast(val);
                    }
                }
            }
        }

        static void Main(string[] args)
        {
            var input = "467528193"; // real
            //var input = "389125467"; // sample
            CupCircle cups = new CupCircle(input.ToCharArray().Select(c => int.Parse(c.ToString())));
            int max = cups.AsEnumerable().Max();
            for (int i = max + 1; i <= 1000000; ++i)
            {
                cups.AddLast(i);
            }
            max = 1000000;
            int cup;

            for (int t = 0; t < 10000000; ++t)
            {
                if (t % 100000 == 0)
                {
                    Console.WriteLine($"Reached step {t} at {DateTime.Now.ToShortTimeString()}");
                }
                //Console.WriteLine($"-- Move {t + 1}");
                //Console.WriteLine($"cups: ({cups.First()}) {string.Join(" ", cups.Skip(1))})");
                cup = cups.First();
                LinkedList<int> pickedUp = new LinkedList<int>(cups.AsEnumerable().Skip(1).Take(3));
                //Console.WriteLine($"pick up: {string.Join(", ", pickedUp)})");
                cups.RemoveFirst();
                cups.RemoveFirst();
                cups.RemoveFirst();
                cups.RemoveFirst();
                cups.AddLast(cup);
                int dest = cup - 1;
                if (dest == 0)
                {
                    dest = max;
                }
                while (pickedUp.Contains(dest))
                {
                    dest = dest - 1;
                    if (dest == 0)
                    {
                        dest = max;
                    }
                }
                //Console.WriteLine($"destination: {dest}");
                //Console.WriteLine();
                var destNode = cups.Find(dest);
                cups.AddAfter(destNode, pickedUp);
            }

            while (cups.First() != 1)
            {
                cup = cups.First();
                cups.RemoveFirst();
                cups.AddLast(cup);
            }
            cups.RemoveFirst();
            //Console.WriteLine(string.Join("", cups.Select(i => i.ToString())));
            var one = cups.Find(1);
            long first = one.Next().Inner.Value;
            long second = one.Next().Next().Inner.Value;
            Console.WriteLine(first * second);

            // Not 93467528
        }
    }
}
