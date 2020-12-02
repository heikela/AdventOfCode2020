using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Day02
{
    class Program
    {

        public struct Item
        {
            public int Min;
            public int Max;
            public Char Letter;
            public String Password;
        }

        public static Item Parse(string line)
        {
            var pwdMatch = Regex.Match(line, "(\\d+)-(\\d+) (\\w): (\\w+)");
            if (!pwdMatch.Success)
            {
                throw new ArgumentException();
            }
            return new Item()
            {
                Min = int.Parse(pwdMatch.Groups[1].Value),
                Max = int.Parse(pwdMatch.Groups[2].Value),
                Letter = pwdMatch.Groups[3].Value[0],
                Password = pwdMatch.Groups[4].Value
            };
        }

        public static bool Check(Item pwdItem)
        {
            string password = pwdItem.Password;
            Char letter = pwdItem.Letter;
            int count = password.Count(c => c == letter);
            return count >= pwdItem.Min && count <= pwdItem.Max;
        }

        public static bool Check2(Item pwdItem)
        {
            string password = pwdItem.Password;
            Char letter = pwdItem.Letter;

            return password[pwdItem.Min - 1] == letter ^ password[pwdItem.Max - 1] == letter;
        }

        static void Main(string[] args)
        {
            List<Item> items = File.ReadLines("../../../input.txt").Select(s => Parse(s)).ToList();
            Console.WriteLine(items.Count(Check));
            Console.WriteLine(items.Count(Check2));
        }
    }
}
