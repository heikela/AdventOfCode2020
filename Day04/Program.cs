using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Common;

namespace Day04
{
    class Program
    {
        static Dictionary<string, string> ParsePassport(IEnumerable<string> lines)
        {
            return lines
                .SelectMany(line => line.Split(' '))
                .Select(entry =>
                {
                    var fieldAndValue = entry.Split(':', 2);
                    return new KeyValuePair<string, string>(fieldAndValue[0], fieldAndValue[1]);
                })
                .ToDictionary();
        }

        static readonly List<string> RequiredFields = new List<string>() {
            "byr", "iyr", "eyr", "hgt", "hcl", "ecl", "pid"
        };

        static bool IsValidPassport(Dictionary<string, string> document)
        {
            return RequiredFields.All(field => document.ContainsKey(field));
        }

        static void Main(string[] args)
        {
            IEnumerable<Dictionary<string, string>> passports = File.ReadLines("../../../input.txt").Paragraphs().Select(ParsePassport);
            Console.WriteLine(passports.Count(IsValidPassport));
        }
    }
}
