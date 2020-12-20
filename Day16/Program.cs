using System;
using System.IO;
using Common;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Day16
{
    public record FieldRange(int Min, int Max);

    class Program
    {
        static void Main(string[] args)
        {
            var inputSections = File.ReadLines("../../../input.txt").Paragraphs().ToList(); ;

            var rules = inputSections[0]
                .Select(line =>
                {
                    Match ruleMatch = Regex.Match(line, @"^.*?: (\d+)-(\d+) or (\d+)-(\d+)$");
                    if (!ruleMatch.Success)
                    {
                        throw new Exception("Unknown rule format: " + line);
                    }
                    return new List<FieldRange>() {
                        new FieldRange(int.Parse(ruleMatch.Groups[1].Value),
                            int.Parse(ruleMatch.Groups[2].Value)),
                        new FieldRange(
                            int.Parse(ruleMatch.Groups[3].Value),
                            int.Parse(ruleMatch.Groups[4].Value)) };

                }).ToList();

            int result = inputSections[2].Skip(1).SelectMany(line =>
                line.Split(',')
                .SelectMany(field =>
                {
                    int val = int.Parse(field);
                    if (!rules.SelectMany(l => l).Any(rule => rule.Min <= val && rule.Max >= val))
                    {
                        return new List<int>() { val };
                    }
                    else { return new List<int>(); }
                }))
                .Sum();

            IEnumerable<string> validTickets = inputSections[2].Skip(1).Where(line =>
                line.Split(',')
                .All(field =>
                {
                    int val = int.Parse(field);
                    return rules.SelectMany(l => l).Any(rule => rule.Min <= val && rule.Max >= val);
                    })
                );

            IEnumerable<List<int>> validTicketFields = validTickets.Select(line => line.Split(',').Select(field => int.Parse(field)).ToList());

            IEnumerable<List<HashSet<int>>> possibleFields = validTicketFields.Select(ticket =>
            ticket.Select(field =>
            {
                IEnumerable<int> possible = rules
                       .ZipWithIndex()
                       .Where(kv => kv.Value.Any(rule => rule.Min <= field && rule.Max >= field))
                       .Select(kv => kv.Key);
                return new HashSet<int>(possible);
            })
           .ToList());

            List<HashSet<int>> rulesForPos =
                rules.ZipWithIndex()
                .Select(kv => kv.Key)
                .Select(pos =>
                {
                    HashSet<int> possible = new HashSet<int>(Enumerable.Range(0, 30));
                    foreach (var ticket in possibleFields)
                    {
                        possible.IntersectWith(ticket[pos]);
                    }
                    return possible;
                }).ToList();

            while (rulesForPos.Any(possibles => possibles.Count() > 1))
            {
                for (int determinedPos = 0; determinedPos < rulesForPos.Count(); ++determinedPos)
                {
                    if (rulesForPos[determinedPos].Count() > 1)
                    {
                        continue;
                    }
                    else
                    {
                        int takenRule = rulesForPos[determinedPos].First();
                        for (int i = 0; i < rulesForPos.Count(); ++i)
                        {
                            if (i == determinedPos)
                            {
                                continue;
                            }
                            rulesForPos[i].Remove(takenRule);
                        }
                    }
                }
            }

            long result2 = inputSections[1].Skip(1).First().Split(',').Select(field => int.Parse(field))
                .ZipWithIndex()
                .Where(kv => rulesForPos[kv.Key].Any(rule => rule < 6))
                .Select(kv => (long)kv.Value)
                .Aggregate((x, y) => x * y);
            Console.WriteLine(result2);
        }
    }
}
