using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ECalculator
{
    public class ECalculator
    {
        private List<BigInteger> results;
        private int thread_number;
        private int threads_count;
        private int precision;
        private bool quietMode;
        private int iterations;
        private string outputPath;

        public ECalculator(List<BigInteger> results, int numberOfThreads, int threads_count, int itterations, int precision, bool quietMode, string outputPath)
        {
            this.results = results;
            this.thread_number = numberOfThreads;
            this.threads_count = threads_count;
            this.precision = precision;
            this.quietMode = quietMode;
            this.iterations = itterations;
            this.outputPath = outputPath;
        }

        public void run()
        {
            //Stopwatch watch = new Stopwatch();
            //watch.Start();

            BigInteger result = new BigInteger(0);
            BigInteger number;

            try
            {
                for (int i = thread_number; i < iterations; i += threads_count)
                {
                    number = new BigInteger(((3 * i) * (3 * i) + 1));
                    BigInteger st = BigInteger.Pow(new BigInteger(10), precision);
                    number = BigInteger.Multiply(number, st);
                    number = BigInteger.Divide(number, factorial(3 * i));
                    if (number.ToString() == "0")
                    {
                        break;
                    }
                    result = BigInteger.Add(result, number);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }

            if (!quietMode)
            {
                BigInteger degree = BigInteger.Pow(new BigInteger(10), precision);
                string devider = BigInteger.Divide(result, degree).ToString();
                string remainder = BigInteger.Remainder(result, degree).ToString();
                while (remainder.Length < precision)
                {
                    remainder = "0" + remainder;
                }
                string neper = string.Format("{0}.{1}", devider, remainder);
                Console.WriteLine("Thread number: " + thread_number + "\nThread execution result: " + neper + "\n");

                if (!string.IsNullOrEmpty(outputPath))
                {
                    Program.writeToFile("Thread number: " + thread_number + "\nThread execution result: " + neper + "\n");
                }
            }



            results.Add(result);

            //watch.Stop();
            //Console.WriteLine("Thread " + thread_number + " : " + watch.ElapsedMilliseconds );
        }

        public static BigInteger factorial(int number)
        {
            BigInteger factorial = new BigInteger(1);

            for (int i = 1; i <= number; i++)
            {
                factorial = BigInteger.Multiply(factorial, new BigInteger(i));
            }

            return factorial;
        }
    }


}
