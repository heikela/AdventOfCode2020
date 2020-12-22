using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using Common;
using System.Numerics;

namespace Day22
{
    class Program
    {
        static void Main(string[] args)
        {
            var fileName = "input.txt";
            var initialdecks = File.ReadLines("../../../" + fileName).Paragraphs().Select(lines => lines.Skip(1).Select(line => int.Parse(line))).ToList();

            Dictionary<int, Queue<int>> decks = new Dictionary<int, Queue<int>>();
            int winner;

            for (int i = 0; i < 2; ++i)
            {
                decks[i] = new Queue<int>();
                foreach (int card in initialdecks[i])
                {
                    decks[i].Enqueue(card);
                }
            }

            while (decks.All(deck => deck.Value.Any()))
            {
                var pairs = decks.Select(kv =>
                {
                    int topCard = kv.Value.Dequeue();
                    return new KeyValuePair<int, int>(kv.Key, topCard);
                }).ToList().OrderByDescending(kv => kv.Value);
                winner = pairs.First().Key;
                decks[winner].Enqueue(pairs.First().Value);
                decks[winner].Enqueue(pairs.Skip(1).First().Value);
            }

            int Score(IEnumerable<int> deck) => deck.Reverse().ZipWithIndex().Aggregate(0, (sum, pair) => sum + (pair.Key + 1) * pair.Value);
            Console.WriteLine(Score(decks[0].ToList()));
            Console.WriteLine(Score(decks[1].ToList()));

            decks = new Dictionary<int, Queue<int>>();

            for (int i = 0; i < 2; ++i)
            {
                decks[i] = new Queue<int>();
                foreach (int card in initialdecks[i])
                {
                    decks[i].Enqueue(card);
                }
            }

            winner = Play(decks);
            Console.WriteLine(Score(decks[winner].ToList()));
            Console.WriteLine(Score(decks[(winner + 1) % 2].ToList()));
        }

        static int Score(IEnumerable<int> deck) => deck.Reverse().ZipWithIndex().Aggregate(0, (sum, pair) => sum + (pair.Key + 1) * pair.Value);

        static (BigInteger, BigInteger) State(IEnumerable<IEnumerable<int>> decks) => (decks.First().ZipWithIndex().Aggregate(new BigInteger(0), (sum, pair) => sum + (BigInteger)Math.Pow(50, pair.Key) * pair.Value),
            decks.Skip(1).First().ZipWithIndex().Aggregate(new BigInteger(0), (sum, pair) => sum + (BigInteger)Math.Pow(51, pair.Key) * pair.Value));

        static int Play(Dictionary<int, Queue<int>> decks)
        {
            var seenStates = new HashSet<(BigInteger, BigInteger)>();

            while (decks.All(deck => deck.Value.Any()))
            {
                var state = State(decks.Values);
                if (seenStates.Contains(state))
                {
                    return 0;
                }
                seenStates.Add(state);

                var pairs = decks.Select(kv =>
                {
                    int topCard = kv.Value.Dequeue();
                    return new KeyValuePair<int, int>(kv.Key, topCard);
                }).ToList();
                int winner = -1;

                if (pairs.All(kv => decks[kv.Key].Count() >= kv.Value))
                {
                    winner = Play(decks.Select(kv => new KeyValuePair<int, Queue<int>>(kv.Key, new Queue<int>(kv.Value.Take(pairs[kv.Key].Value)))).ToDictionary());
                }
                else
                {
                    winner = pairs.OrderByDescending(kv => kv.Value).First().Key;
                }
                decks[winner].Enqueue(pairs[winner].Value);
                decks[winner].Enqueue(pairs[(winner + 1) % 2].Value);
            }
            return decks.OrderByDescending(kv => kv.Value.Count()).First().Key;
        }
    }
}
