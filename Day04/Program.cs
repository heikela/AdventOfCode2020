using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Common;
using System.Text.RegularExpressions;

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

        static bool PassportHasRequiredFields(Dictionary<string, string> document)
        {
            return RequiredFields.All(field => document.ContainsKey(field));
        }

        static bool IsValidPassport(Dictionary<string, string> document)
        {
            if (!PassportHasRequiredFields(document))
            {
                return false;
            }
            foreach (var kv in document)
            {
                switch (kv.Key)
                {
                    case "byr":
                        var match = Regex.Match(kv.Value, @"^(\d+)$");
                        if (!match.Success)
                        {
                            return false;
                        }
                        int val = int.Parse(match.Groups[1].Value);
                        if (val < 1920 | val > 2002)
                        {
                            return false;
                        }
                        break;
                    case "iyr":
                        break;
                    case "eyr":
                        break;
                    case "hgt":
                        break;
                    case "hcl":
                        break;
                    case "ecl":
                        break;
                    case "pid":
                        break;
                    default: break;
                }
            }
            return true;
        }

        static void Main(string[] args)
        {
            IEnumerable<Dictionary<string, string>> passports = File.ReadLines("../../../input.txt").Paragraphs().Select(ParsePassport);
            Console.WriteLine(passports.Count(PassportHasRequiredFields));
            Console.WriteLine(passports.Count(IsValidPassport));
        }
    }
}
