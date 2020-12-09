using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using System.IO;
using Common; 

namespace Day07
{
    public record Bag
    {
        public string Colour;
        public List<string> Contents;

        public static Bag Parse(string line)
        {
            Match ruleMatch = Regex.Match(line, @"^(.*) bags contain (,? ?\d+ (.*?) bags?)+.$");
            return new Bag
            {
                Colour = ruleMatch.Groups[1].Value,
                Contents = ruleMatch.Groups[3].Captures.Cast<Capture>().Select(c => c.Value).ToList()
            };
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            IEnumerable<Bag> rules = File.ReadLines("../../../input.txt")
                .Select(Bag.Parse);

            ConcreteGraph<string> contains = new ConcreteGraph<string>();
            foreach (Bag bag in rules)
            {
                foreach (string content in bag.Contents)
                {
                    contains.AddEdge(bag.Colour, content);
                }
            }
            ConcreteGraph<string> containedIn = contains.ReverseGraph();

            int possibilities = 0;
            containedIn.BfsFrom("shiny gold", (container, _) => possibilities++);
            Console.WriteLine(possibilities - 1); // "shiny gold" alone is included in the count from BFS
        }
    }
}
