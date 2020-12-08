using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace Day08
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> program = File.ReadLines("../../../input.txt").ToList();
            int result = RunProgram(program);
            Console.WriteLine(result);
            IEnumerable<List<string>> variations = Enumerable.Range(0, program.Count).SelectMany(i => MutateProgram(i, program));
            IEnumerable<int> results = variations.SelectMany(RunProgram2);

            Console.WriteLine(results.First());
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

        private static int RunProgram(List<string> program)
        {
            HashSet<int> visited = new HashSet<int>();
            int pc = 0;
            int acc = 0;
            while (!visited.Contains(pc))
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

            return acc;
        }
        private static IEnumerable<int> RunProgram2(List<string> program)
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

            if (pc == program.Count) {
                yield return acc;
                yield break;
            }
            else {
                yield break;
            }
        }
    }
}
