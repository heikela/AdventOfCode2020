using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Common;

namespace Day19
{
    class Program
    {
        public record Rule;

        public record Or(List<Seq> Alternatives) : Rule;

        public record Seq(List<int> Parts) : Rule;

        public record Literal(Char C) : Rule;

        public record Special : Rule
        {
            public int Number;

            public string Regex(Dictionary<int, Rule> rules)
            {
                string r42 = RuleToRegex(rules[42], rules);
                string r31 = RuleToRegex(rules[31], rules);
                if (Number == 8)
                {
                    return $"(({r42})+)";
                }
                if (Number == 11)
                {
                    return $@"(((?<Open>{r42})|(?<-Open>{r31}))+(?(Open)(?!)))";
                }
                throw new Exception("Unknown special rule");
            }
        }

        public static KeyValuePair<int, Rule> ParseRule(string line)
        {
            string LiteralPattern = @"(\d+): ""(\w)""";
            Match match = Regex.Match(line, LiteralPattern);
            if (match.Success)
            {
                return new KeyValuePair<int, Rule>(int.Parse(match.Groups[1].Value), new Literal(match.Groups[2].Value[0]));
            }
            string SeqPattern = @"(\d+): (\d+\s*)+$";
            match = Regex.Match(line, SeqPattern);
            if (match.Success)
            {
                return new KeyValuePair<int, Rule>(int.Parse(match.Groups[1].Value), new Seq(match.Groups[2].Captures.Select(g => int.Parse(g.Value.Trim())).ToList()));
            }
            string OrPattern = @"(\d+): (\d+\s*)+ \| (\d+\s*)+";
            match = Regex.Match(line, OrPattern);
            if (match.Success)
            {
                return new KeyValuePair<int, Rule>(int.Parse(match.Groups[1].Value),
                    new Or(new List<Seq>() {
                        new Seq(match.Groups[2].Captures.Select(g => int.Parse(g.Value.Trim())).ToList()),
                        new Seq(match.Groups[3].Captures.Select(g => int.Parse(g.Value.Trim())).ToList()),
                    }));
            }
            throw new Exception("Cannot parse rule " + line);
        }

        public static string RuleToRegex(Rule rule, Dictionary<int, Rule> rules)
        {
            Rule GetRule(int idx)
            {
                return rules[idx];
            }

            string Parenthesize(string s) => $"({s})";

            if (rule is Literal)
            {
                return (rule as Literal).C.ToString();
            }
            if (rule is Seq)
            {
                return string.Join("", (rule as Seq).Parts.Select(i => RuleToRegex(GetRule(i), rules)).Select(Parenthesize));
            }
            if (rule is Or)
            {
                return string.Join("|", (rule as Or).Alternatives.Select(r => RuleToRegex(r, rules)).Select(Parenthesize));
            }
            if (rule is Special)
            {
                return (rule as Special).Regex(rules);
            }
            return "";
        }

        static void Main(string[] args)
        {
            var input = File.ReadLines("../../../input.txt").Paragraphs().ToList();
            //var input = File.ReadLines("../../../sample.txt").Paragraphs().ToList();
            var rules = input[0].Select(ParseRule).ToDictionary();

            rules[8] = new Special() { Number = 8 };
            rules[11] = new Special() { Number = 11};

            string test = $"^({RuleToRegex(rules[0], rules)})$";

            int count = 0;

            foreach (string line in input[1])
            {
                Match match = Regex.Match(line, test);
                if (match.Success && match.Value == line)
                {
                    Console.Write("#");
                    ++count;
                }
                else
                {
                    Console.Write(".");
                }
            }
            Console.WriteLine();

            int result = input[1].Count(line => Regex.IsMatch(line, test));
            Console.WriteLine(result);
            Console.WriteLine(count);

            // IT'S NOT 487
        }
    }
}
