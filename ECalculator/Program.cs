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
        private static int precision;
        private static int threads_count;
        private static bool quietMode;
        private static string filePath;
        

        private static bool validateInputArguments(String[] args)
        {
            quietMode = false;

            filePath = "";
            precision = 10000;
            threads_count = 4;

            if (args.Length > 0)
            {
                if (args[args.Length - 1].Equals("-q"))
                {
                    quietMode = true;
                }
            }

            for (int i = 0; i < args.Length - 1; i += 2)
            {
                if (args[i].Equals("-p"))
                {
                    if (!int.TryParse(args[i + 1], out precision))
                    {
                        return false;
                    }

                }
                else if (args[i].Equals("-t"))
                {
                    if (!int.TryParse(args[i + 1], out threads_count))
                    {
                        return false;
                    }

                }
                else if (args[i].Equals("-o"))
                {
                    filePath = args[i + 1];
                    if (!filePath.StartsWith("./"))
                    {
                        filePath = "./" + filePath;
                    }
                }
            }

            return true;
        }

        static void Main(string[] args)
        {
            if (!validateInputArguments(args))
            {
                Console.WriteLine("Invalid input!");
                return;
            }

            runCalculations();
        }

        public static void runCalculations()
        {
            List<BigInteger> results = new List<BigInteger>();
            Thread[] threads = new Thread[threads_count];

            Stopwatch watch = new Stopwatch();
            watch.Start();

            for (int i = 0; i < threads_count; i++)
            {
                ECalculator erunnable = new ECalculator(results, i, threads_count, precision, precision, quietMode, filePath);
                Thread t = new Thread(new ThreadStart(erunnable.run));
                threads[i] = t;
                t.Start();
            }

            BigInteger sum = new BigInteger(0);

            for (int i = 0; i < threads_count; i++)
            {
                threads[i].Join();
                sum = BigInteger.Add(sum, results[i]);
            }

            BigInteger degree = BigInteger.Pow(new BigInteger(10), precision);
            string devider = BigInteger.Divide(sum, degree).ToString();
            string remainder = BigInteger.Remainder(sum, degree).ToString();
            while (remainder.Length < precision)
            {
                remainder = "0" + remainder;
            }
            string neper = string.Format("{0}.{1}", devider, remainder);
            watch.Stop();

            Console.WriteLine();
            Console.WriteLine("Final Result: " + neper);
            Console.WriteLine("Elapsed Time : " + (watch.ElapsedMilliseconds) + " milliseconds");


            if (!string.IsNullOrEmpty(filePath))
            {
                writeToFile("Final Result:" + neper);
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
