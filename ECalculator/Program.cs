using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ECalculator
{
    class Program
    {
        private static int precision = 10000;
        private static int threads_count = 4;
        private static bool quietMode = false;
        private static string filePath = "";

        static void Main(string[] args)
        {
            if (!validateInputArguments(args))
            {
                Console.WriteLine("Invalid input!");
                return;
            }

            runCalculations();
        }

        private static bool validateInputArguments(String[] args)
        {
            for (int i = 0; i < args.Length; i += 2)
            {
                switch (args[i])
                { 
                    case "-p":
                    if (!int.TryParse(args[i + 1], out precision))
                    {
                        return false;
                    }
                        break;
                    case "-t":
                    if (!int.TryParse(args[i + 1], out threads_count))
                    {
                        return false;
                    }
                    break;
                    case "-o":
                    filePath = args[i + 1];
                    if (!filePath.StartsWith("./"))
                    {
                        filePath = "./" + filePath;
                    }
                    break;
                    case "-q":
                    quietMode = true;
                    i--;
                    break;
                }
            }

            return true;
        }

        public static void runCalculations()
        {
            List<BigDecimal> results = new List<BigDecimal>();
            Thread[] threads = new Thread[threads_count];

            Stopwatch watch = new Stopwatch();
            watch.Start();

            for (int i = 0; i < threads_count; i++)
            {
                ECalculator erunnable = new ECalculator(results, i, threads_count, precision, quietMode, filePath);
                Thread t = new Thread(new ThreadStart(erunnable.run));
                threads[i] = t;
                t.Start();
            }

            BigDecimal sum = new BigDecimal(0, precision);

            for (int i = 0; i < threads_count; i++)
            {
                threads[i].Join();
                sum.Add(results[i]);
            }
            watch.Stop();

            Console.WriteLine();
            Console.WriteLine("Final Result: " + sum);
            Console.WriteLine("Elapsed Time : " + (watch.ElapsedMilliseconds) + " milliseconds");


            if (!string.IsNullOrEmpty(filePath))
            {
                writeToFile("Final Result:" + sum);
                writeToFile("Elapsed Time : " + (watch.ElapsedMilliseconds) + " milliseconds");
            }

            Console.ReadKey();
        }

        public static void writeToFile(string content)
        {
            File.AppendAllLines(filePath + ".txt", new string[] { "\n" + content });
        }
    }

}
