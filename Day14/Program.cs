using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Common;

namespace Day14
{
    class Program
    {
        static void Main(string[] args)
        {
            IEnumerable<string> instructions = File.ReadLines("../../../input.txt");

            ulong andMask = 0;
            ulong orMask = 0;
            Dictionary<ulong, ulong> memory = new Dictionary<ulong, ulong>();
            foreach (string instruction in instructions)
            {
                Match maskMatch = Regex.Match(instruction, @"^mask = (\w+)$");
                if (instruction.StartsWith("mask"))
                {
                    string maskString = maskMatch.Groups[1].Value;
                    andMask = Convert.ToUInt64(maskString.Replace('X', '1'), 2);
                    orMask = Convert.ToUInt64(maskString.Replace('X', '0'), 2);
                    continue;
                }
                Match memMatch = Regex.Match(instruction, @"^mem\[(\d+)\] = (\d+)$");
                if (memMatch.Success)
                {
                    ulong address = ulong.Parse(memMatch.Groups[1].Value);
                    ulong value = ulong.Parse(memMatch.Groups[2].Value);
                    ulong valToWrite = value & andMask | orMask;
                    memory[address] = valToWrite;
                    continue;
                }
                throw new Exception("Unknown instruction");
            }
            Console.WriteLine(memory.Values.AsEnumerable().Aggregate((a, b) => a + b));

            String mask = "";
            memory = new Dictionary<ulong, ulong>();
            foreach (string instruction in instructions)
            {
                Match maskMatch = Regex.Match(instruction, @"^mask = (\w+)$");
                if (instruction.StartsWith("mask"))
                {
                    mask = maskMatch.Groups[1].Value;
                    continue;
                }
                Match memMatch = Regex.Match(instruction, @"^mem\[(\d+)\] = (\d+)$");
                if (memMatch.Success)
                {
                    ulong address = ulong.Parse(memMatch.Groups[1].Value);
                    ulong value = ulong.Parse(memMatch.Groups[2].Value);
                    mask.AsEnumerable().Reverse()
                        .ZipWithIndex()
                        .Aggregate<KeyValuePair<int, char>, IEnumerable<ulong>>(new List<ulong>() { address }, (addresses, byteMask) =>
                        {
                            switch (byteMask.Value)
                            {
                                case '0':
                                    return addresses;
                                case '1':
                                    return addresses.Select(a => a | (ulong)Math.Pow(2.0, byteMask.Key));
                                case 'X':
                                    return addresses.SelectMany(a => new List<ulong>()
                                    {
                                        a, a ^ (ulong)Math.Pow(2.0, byteMask.Key)
                                    });
                                default: throw new Exception("Unknown address mask" + mask);
                            }
                        })
                        .Select(a => memory[a] = value)
                        .ToList();
                    continue;
                }
                throw new Exception("Unknown instruction");
            }
            Console.WriteLine(memory.Values.AsEnumerable().Aggregate((a, b) => a + b));
        }
    }
}
