using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;


namespace AdventOfCode2020
{
    class Program
    {
        static void Main(string[] args)
        {
            List<int> input = File.ReadLines("../../../input.txt").Select(s => int.Parse(s)).ToList();

            for (int i = 0; i < input.Count - 1; ++i)
            {
                for (int j = i + 1; j < input.Count; ++j)
                {
                    int first = input[i];
                    int second = input[j];
                    if (first + second == 2020)
                    {
                        Console.WriteLine(first * second);
                    }
                }
            }


            for (int i = 0; i < input.Count - 2; ++i)
            {
                for (int j = i + 1; j < input.Count - 1; ++j)
                {
                    for (int k = j + 1; k < input.Count; ++k)
                    {
                        int first = input[i];
                        int second = input[j];
                        int third = input[k];
                        if (first + second + third == 2020)
                        {
                            Console.WriteLine(first * second * third);
                        }
                    }
                }
            }
        }
    }
}

