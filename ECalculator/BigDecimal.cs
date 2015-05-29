using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ECalculator
{
    public class BigDecimal
    {
        public BigInteger number;
        private int precision;

        #region Constructors

        public BigDecimal(long number, int precision = 0)
        {
            this.number = new BigInteger(number);
            if (precision != 0)
            {
                this.SetPrecision(precision);
            }
        }

        public BigDecimal(ulong number, int precision = 0)
        {
            this.number = new BigInteger(number);
            if (precision != 0)
            {
                this.SetPrecision(precision);
            }
        }

        public BigDecimal(int number, int precision = 0)
        {
            this.number = new BigInteger(number);
            if (precision != 0)
            {
                this.SetPrecision(precision);
            }
        }

        public BigDecimal(uint number, int precision = 0)
        {
            this.number = new BigInteger(number);
            if (precision != 0)
            {
                this.SetPrecision(precision);
            }
        }

        public BigDecimal(BigInteger number)
        {
            this.number = number;
        }

        #endregion

        public static bool IsNullOrZero(BigDecimal number)
        {
            if (number == null || number.number == null || number.number.IsZero)
            {
                return true;
            }

            return false;
        }

        public void ChangePrecision(int precision)
        {
            this.SetPrecision(this.precision - precision);
            this.precision = precision;
        }

        public override string ToString()
        {
            BigInteger degree = BigInteger.Pow(new BigInteger(10), precision);
            string devider = BigInteger.Divide(this.number, degree).ToString();
            string remainder = BigInteger.Remainder(this.number, degree).ToString();
            StringBuilder sb = new StringBuilder();
            while (remainder.Length + sb.Length < precision)
            {
                sb.Append('0');
            }
            sb.Append(remainder);
            string neper = string.Format("{0}.{1}", devider, sb.ToString());
            return neper;
        }

        private void SetPrecision(int precision)
        {
            this.precision = precision;
            this.number = BigInteger.Multiply(this.number, BigInteger.Pow(new BigInteger(10), this.precision));
        }
    }

    public static class BigDecimalExtentions
    {
        public static void Add(this BigDecimal first, BigDecimal second)
        {
            first.number = BigInteger.Add(first.number, second.number);
        }

        public static void Pow(this BigDecimal number, int exponent)
        {
            number.number = BigInteger.Pow(number.number, exponent);
        }

        public static void Divide(this BigDecimal divident, BigDecimal divisor)
        {
            divident.number = BigInteger.Divide(divident.number, divisor.number);
        }

        public static void Multiply(this BigDecimal left, BigDecimal right)
        {
            left.number = BigInteger.Multiply(left.number, right.number);
        }

        public static void Subtract(this BigDecimal left, BigDecimal right)
        {
            left.number = BigInteger.Subtract(left.number, right.number);
        }
    }
}