using System;
using System.IO;
using Common;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Day18
{
    class Program
    {
        enum TokenType
        {
            Unknown,
            Num,
        };

        enum EvalState
        {
            StartExpression,
            PlusExpression,
            Expression
        }
        static IEnumerable<string> Tokenize(string line)
        {
            int start = 0;
            TokenType currentType = TokenType.Unknown;
            int i = 0;
            while (i < line.Length)
            {
                char c = line[i];
                if (currentType == TokenType.Unknown)
                {
                    if (c >= '0' && c <= '9')
                    {
                        start = i;
                        currentType = TokenType.Num;
                    }
                    else if (c == ' ')
                    {
                        start++;
                    }
                    else
                    {
                        start++;
                        yield return c.ToString();
                    }
                    i++;
                }
                else
                {
                    if (c >= '0' && c < '9')
                    {
                        ++i;
                        continue;
                    }
                    else
                    {
                        currentType = TokenType.Unknown;
                        yield return line.Substring(start, i - start);
                        if (c != ' ')
                        {
                            yield return c.ToString();
                        }
                        ++i;
                    }
                }
            }
            if (currentType == TokenType.Num)
            {
                yield return line.Substring(start, i - start);
            }
        }

        static long EvalExpression(IEnumerator<string> tokens)
        {
            long acc = EvalPlusExpression(tokens);
            while (tokens.Current == "*")
            {
                if (!tokens.MoveNext())
                {
                    throw new Exception("Unexpected end of input");
                }
                acc *= EvalPlusExpression(tokens);
            }
            return acc;
        }

        static long EvalParensExpression(IEnumerator<string> tokens)
        {
            if (tokens.Current != "(")
            {
                throw new Exception("Expected (");
            }
            if (!tokens.MoveNext())
            {
                throw new Exception("Unexpected end of input after (");
            }
            long result = EvalExpression(tokens);
            if (tokens.Current != ")")
            {
                throw new Exception("Expected )");
            }
            return result;
        }

        static long EvalPlusExpression(IEnumerator<string> tokens)
        {
            long acc = 0;
            bool finished = false;
            while (tokens.Current != "*" && !finished)
            {
                string token = tokens.Current;
                if (token == "(")
                {
                    acc += EvalParensExpression(tokens);
                    finished = !tokens.MoveNext();
                }
                else if (token == ")")
                {
                    return acc;
                }
                else if (token == "+")
                {
                    if (!tokens.MoveNext())
                    {
                        throw new Exception("Unexpected end of input");
                    }
                }
                else
                {
                    long right = long.Parse(token);
                    acc += right;
                    finished = !tokens.MoveNext();
                }
            }
            return acc;
        }

        static void Main(string[] args)
        {
//            string fileName = "../../../sample2.txt";
            string fileName = "../../../input.txt";
            long sum = 0;
            foreach (string line in File.ReadLines(fileName))
            {
                Stack<long> acc = new Stack<long>();
                acc.Push(0);
                Stack<char> curOp = new Stack<char>();
                curOp.Push('(');

                void Operate()
                {
                    long curNumber = acc.Pop();
                    char op = curOp.Pop();
                    long left = acc.Pop();
                    long result;
                    if (op == '+')
                    {
                        result = left + curNumber;
                    }
                    else if (op == '*')
                    {
                        result = left * curNumber;
                    }
                    else if (op == '(')
                    {
                        result = curNumber;
                    }
                    else
                    {
                        throw new Exception("unknown operand " + op);
                    }
                    acc.Push(result);
                }

                List<string> tokens = Tokenize(line).ToList();
                foreach (string token in tokens)
                {
                    if (token == "(") {
                        acc.Push(0);
                        curOp.Push('(');
                    }
                    else if (token == "+" || token == "*")
                    {
                        Operate();
                        curOp.Push(token[0]);
                    }
                    else if (token == ")")
                    {
                        Operate();
                    }
                    else
                    {
                        acc.Push(long.Parse(token));
                    }
                }
                Operate();
                long oneResult = acc.Pop();
                sum += oneResult;

            }
            Console.WriteLine(sum);


            sum = 0;
            foreach (string line in File.ReadLines(fileName))
            {
                EvalState state = EvalState.StartExpression;
                IEnumerator<string> tokens = Tokenize(line).GetEnumerator();
                tokens.MoveNext();
                long oneResult = EvalExpression(tokens);
                Console.WriteLine(oneResult);
                sum += oneResult;

            }
            Console.WriteLine(sum);


        }
    }
}
