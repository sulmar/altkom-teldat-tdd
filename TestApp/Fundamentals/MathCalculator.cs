using System;
using System.Collections.Generic;
using System.Text;

namespace TestApp
{
    public class MathCalculator
    {
        public int Add(int a, int b)
        {
            return a + b;
        }

        public int Max(int a, int b)
        {
            return (a > b) ? a : b;
        }

        public IEnumerable<int> GetPrimeNumbers(int limit)
        {
            for (int i = 0; i < limit; i++)
            {
                if (IsPrime(i)) yield return i;
            }
        }

        public static bool IsPrime(int n)
        {
            if (n == 2)
            {
                return true;
            }
            if (n < 2 || n % 2 == 0)
            {
                return false;
            }
            for (int i = 3; i <= Math.Sqrt(n); i += 2)
            {
                if (n % i == 0)
                {
                    return false;
                }
            }
            return true;
        }

    }
}
