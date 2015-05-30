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
        private List<BigDecimal> results;
        private int thread_number;
        private int threads_count;
        private int precision;
        private bool quietMode;
        private string outputPath;

        public ECalculator(List<BigDecimal> results, int numberOfThreads, int threads_count, int precision, bool quietMode, string outputPath)
        {
            this.results = results;
            this.thread_number = numberOfThreads;
            this.threads_count = threads_count;
            this.precision = precision;
            this.quietMode = quietMode;
            this.outputPath = outputPath;
        }

        public void run()
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();

            BigDecimal result = new BigDecimal(0, precision);
            BigDecimal number;

            for (int i = thread_number; i < precision; i += threads_count)
            {
                number = new BigDecimal(((9 * i * i) + 1), precision);
                number.Divide(factorial(3 * i));
                if (BigDecimal.IsNullOrZero(number))
                {
                    break;
                }
                result.Add(number);
            }

            watch.Stop();

            if (!quietMode)
            {
                Console.WriteLine("\nThread number: " + thread_number + "\nThread execution result: " + result + "\nThread " + thread_number + " : " + watch.ElapsedMilliseconds);

                if (!string.IsNullOrEmpty(outputPath))
                {
                    Program.writeToFile("Thread number: " + thread_number + "\nThread execution result: " + result + "\nThread " + thread_number + " : " + watch.ElapsedMilliseconds);
                }
            }

            results.Add(result);
        }

        public static BigDecimal factorial(int number)
        {
            BigDecimal factorial = new BigDecimal(1);

            for (int i = 1; i <= number; i++)
            {
                factorial.Multiply(new BigDecimal(i));
            }

            return factorial;
        }
    }


}
