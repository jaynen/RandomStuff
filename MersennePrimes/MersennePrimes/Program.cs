using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace MersennePrimes
{
    internal class Program
    {
        // initialize these once at startup for speed
        private static readonly BigInteger Two = new BigInteger(2);
        private static readonly BigInteger Four = new BigInteger(4);

        private static void Main()
        {
            Console.WriteLine("------------------------------");
            Console.WriteLine("Mersenne Primes Generator");
            Console.WriteLine("------------------------------\n");
            Console.WriteLine("\nThis program will keep generating mersenne primes.");
            Console.WriteLine("Press P to print current primes list or Q to quit.\n");

            var currentTest = 2;
            var batchCount = Environment.ProcessorCount*5;
            var mersennePrimes = new ConcurrentBag<int>();
            while (true)
            {
                Console.Write("\rFound {0} mersenne primes so far...   " +
                              "Currently testing from {1} to {2}", mersennePrimes.Count,
                              currentTest, currentTest + batchCount);

                // do a batch calculation of primes in parallel, relative to available logical cores
                Parallel.For(currentTest, currentTest + batchCount,
                    new ParallelOptions { MaxDegreeOfParallelism = batchCount },
                    f =>
                    {
                        if (!IsMersennePrime(f)) return;
                        Console.WriteLine("\n{0} is a mersenne prime.", f);
                        mersennePrimes.Add(f);
                    });

                // increment for next batch
                currentTest += batchCount;

                if (!Console.KeyAvailable) continue;
                var key = Console.ReadKey(true).Key;
                if (key == ConsoleKey.Q) break;
                switch (key)
                {
                    case ConsoleKey.P:
                        Console.Clear();
                        Console.WriteLine("\nThis program will keep generating mersenne primes.");
                        Console.WriteLine("Press P to print current primes list or Q to quit.\n");
                        Console.WriteLine("Current primes: {0}\n", string.Join(",", mersennePrimes.OrderBy(x=>x)));
                        break;
                }
            }
        }

        /// <summary>
        /// checks if a number is a mersenne prime
        /// implemented similar to: https://en.wikipedia.org/wiki/Lucas%E2%80%93Lehmer_primality_test#The_test
        /// but with some modifications...
        /// </summary>
        private static bool IsMersennePrime(int p)
        {
            // MOD: because rest of the algorithm doesn't account for 2 (which is a mersenne prime)
            if (p == 2) return true;

            // MOD: optimization for non-prime
            for (var i = 3; i <= (int)Math.Sqrt(p); i += 2) if (p % i == 0) return false; 

            var s = Four;
            var m = BigInteger.Pow(Two, p) - BigInteger.One;
            for (var i = 3; i <= p; i++) s = (s*s - 2)%m;
            return s == BigInteger.Zero;
        }
    }
}
