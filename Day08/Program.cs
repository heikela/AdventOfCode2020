using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace Day08
{
    public record ProgramResult
    {
        public int Acc;
        public bool Success;
    }

    class Program
    {
        static void Main(string[] args)
        {
            List<string> program = File.ReadLines("../../../input.txt").ToList();
            Console.WriteLine(RunProgram(program).Acc);
            IEnumerable<List<string>> variations = Enumerable.Range(0, program.Count).SelectMany(i => MutateProgram(i, program));
            IEnumerable<ProgramResult> results = variations.Select(RunProgram);

            Console.WriteLine(results.First(r => r.Success).Acc);
        }

        private static IEnumerable<List<string>> MutateProgram(int i, List<string> source)
        {
            List<string> newProgram = source.ToList();
            string instruction = newProgram[i];
            switch (instruction.Substring(0, 3))
            {
                case "jmp":
                    newProgram[i] = "nop" + instruction.Substring(4);
                    yield return newProgram;
                    yield break;
                case "nop":
                    newProgram[i] = "jmp" + instruction.Substring(4);
                    yield return newProgram;
                    yield break;
                case "acc":
                    yield break;
            }
        }

        private static ProgramResult RunProgram(List<string> program)
        {
            HashSet<int> visited = new HashSet<int>();
            int pc = 0;
            int acc = 0;
            while (!visited.Contains(pc) && pc < program.Count)
            {
                visited.Add(pc);
                string instruction = program[pc];
                int arg = int.Parse(instruction.Substring(4));
                switch (instruction.Substring(0, 3))
                {
                    case "jmp":
                        pc += arg;
                        break;
                    case "acc":
                        acc += arg;
                        pc++;
                        break;
                    case "nop":
                        pc++;
                        break;
                }
            }

            return new ProgramResult()
            {
                Acc = acc,
                Success = pc == program.Count
            };
        }
    }
}
