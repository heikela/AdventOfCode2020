using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Day04
{
    class Program
    {
        static List<Dictionary<string, string>> Parse(IEnumerable<string> lines)
        {
            List<Dictionary<string, string>> passports = new List<Dictionary<string, string>>();

            Dictionary<string, string> passport = new Dictionary<string, string>();

            foreach (var line in lines)
            {
                if (Regex.IsMatch(line, @"^\s*$")) {
                    passports.Add(passport);
                    passport = new Dictionary<string, string>();
                }
                else
                {
                    var entries = Regex.Matches(line, @"(\S+:\S+)").Select(m => m.Value);
                    foreach (var entry in entries) {
                        var fieldAndValue = entry.Split(':', 2);
                        passport.Add(fieldAndValue[0], fieldAndValue[1]);
                    }
                }
            }
            if (passport.Count > 0)
            {
                passports.Add(passport);
            }
            return passports;
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
            List<Dictionary<string, string>> passports = Parse(File.ReadLines("../../../input.txt"));
            Console.WriteLine(passports.Count(IsValidPassport));
        }
    }
}
