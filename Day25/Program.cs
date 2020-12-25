using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;
using Common;
using System.Numerics;

namespace Day25
{
    class Program
    {
        static long Transform(long loopSize, long subject)
        {
            long val = 1;
            for (long i = 0; i < loopSize;  ++i)
            {
                val = val * subject;
                val = val % 20201227;
            }
            return val;
        }

        static void Main(string[] args)
        {
            string fileName = "input.txt";
            List<long> publicKeys = File.ReadLines("../../../" + fileName).Select(l => long.Parse(l)).ToList();

            Dictionary<int, long> secretKeys = new Dictionary<int, long>();

            long result = 1;
            for (long guess = 1; guess <= long.MaxValue && !secretKeys.Any();  ++guess)
            {
                result = result * 7;
                result = result % 20201227;
                for (int device = 0; device < 2; ++device)
                {
                    if (result == publicKeys[device])
                    {
                        secretKeys.Add(device, guess);
                    }
                }
            }


            Console.WriteLine(Transform(secretKeys.First().Value, publicKeys[(secretKeys.First().Key + 1) % 2]));
        }
    }
}
