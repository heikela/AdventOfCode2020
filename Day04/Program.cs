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

        static bool CheckNumber(string number, int min, int max)
        {
            Match match = Regex.Match(number, @"^(\d+)$");
            if (!match.Success)
            {
                return false;
            }
            int val = int.Parse(match.Groups[1].Value);
            if (val < min | val > max)
            {
                return false;
            }
            return true;
        }

        static string Right(string s, int n)
        {
            return s.Substring(s.Length - n);
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
                        if (!CheckNumber(kv.Value, 1920, 2002)) {
                            return false;
                        }
                        break;
                    case "iyr":
                        if (!CheckNumber(kv.Value, 2010, 2020))
                        {
                            return false;
                        }
                        break;
                    case "eyr":
                        if (!CheckNumber(kv.Value, 2020, 2030))
                        {
                            return false;
                        }
                        break;
                    case "hgt":
                        Match heightMatch = Regex.Match(kv.Value, @"([0-9]+)(\w+)");
                        if (!heightMatch.Success)
                        {
                            return false;
                        }
                        string unit = heightMatch.Groups[2].Value;
                        string value = heightMatch.Groups[1].Value;
                        if (unit == "cm") {
                            if (!CheckNumber(value, 150, 193)) {
                                return false;
                            }
                        }
                        else if (unit == "in") {
                            if (!CheckNumber(value, 59, 76)) {
                                return false;
                            }
                        }
                        else
                        {
                            return false;
                        }
                        break;
                    case "ecl":
                        if (!Regex.IsMatch(kv.Value, @"^amb|blu|brn|gry|grn|hzl|oth$"))
                        {
                            return false;
                        }
                        break;
                    case "hcl":
                        if (!Regex.IsMatch(kv.Value, @"^#[0-9a-f]{6}$"))
                        {
                            return false;
                        }
                        break;
                    case "pid":
                        if (!Regex.IsMatch(kv.Value, @"^[0-9]{9}$"))
                        {
                            return false;
                        }
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
