using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WordFrequency
{
    internal class Program
    {
        private const int MaxFileSize = 1024 * 1024 * 10;
        private const int FrequencyLimit = 10;

        private static void Main()
        {
            Console.WriteLine("------------------------------");
            Console.WriteLine("Word Frequency Counter");
            Console.WriteLine("------------------------------\n");

            while (true)
            {
                Console.WriteLine("\nPlease enter a path to a non-binary text file: ");
                Console.WriteLine("Or enter Q to quit.");
                Console.Write("\n> ");
                var input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input)) continue;
                if (input.Trim().ToLower().Equals("q")) break;

                try
                {
                    // check if the file exists
                    if (File.Exists(input))
                    {
                        Console.WriteLine("File exists, checking file size...");
                        var fileSize = new FileInfo(input).Length;
                        if (fileSize == 0 || fileSize > MaxFileSize)
                        {
                            Console.WriteLine("File is empty or size exceeds max. of {0} bytes", MaxFileSize);
                            continue;
                        }

                        // read entire file into buffer
                        var data = File.ReadAllText(input);

                        // now we could parse the data manually within a loop, but it sounds messy
                        // let's use the power of LINQ to do this better...
                        var words =
                            // let's remove unix-style linefeeds
                            data.Replace("\r\n", " ")
                                // strip out what spaces we can, initially
                                .Split(' ')
                                // only accept letters (words) 
                                .Select(x => new string(x.Where(char.IsLetter).ToArray()))
                                // convert to lower (in case of same word, but different casing scenarios)
                                .Select(x => x.ToLower())
                                .ToList()
                                // remove words that consist solely of whitespace
                                .FindAll(x => x.Trim().Length > 0)
                                // now let's group/count the words and order them
                                .GroupBy(x => x)
                                .OrderByDescending(x => x.Count())
                                // project each word group into a key/value pair
                                .Select(x => new KeyValuePair<string, int>(x.Key, x.Count()))
                                // only take the words we need
                                .Take(FrequencyLimit)
                                // now project each k/v in to a dictionary item
                                .ToDictionary(x => x.Key, x => x.Value);

                        // do we have word rankings to print?
                        if (words.Count > 0)
                        {
                            var counter = 0;
                            Console.WriteLine("\nWord Occurrences:");
                            foreach (var word in words)
                                Console.WriteLine("{0}. {1} - {2} occurrences", ++counter, word.Key, word.Value);
                        }
                    }
                    else
                    {
                        Console.WriteLine("File cannot be found.");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("[ERROR] {0}", e.Message);
                }
            }
        }
    }
}
