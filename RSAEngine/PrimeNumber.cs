/*
 Written by Shlomo Sahar, 2015
 */
using System;
using System.Numerics;

namespace RSAEngine
{
    /*
     * This class created for - generate and check prime numbers
     * */
    public class PrimeNumber
    {
        private BigInteger iterationTested = 0;
        private BigInteger numberTested = 0;
        private BigInteger number = 2;

        private BigInteger s = 10;

        private bool found;

        private bool running;

        public BigInteger GetPrimeNumber()
        {
            return number;
        }

        public BigInteger GetTestedCount()
        {
            return numberTested;
        }

        public BigInteger GetTestedIterations()
        {
            return iterationTested;
        }

        public bool GetFoundPrime()
        {
            return found;
        }

        public void StopEngine()
        {
            running = false;
        }

        public void TestRabinMiller()
        {
            found = false;
            running = true;

            if (number == 2)
            {
                found = true;
            }

            if (number % 2 != 0)
            {
                found = RabinMillerTest(number, s);
            }

            running = false;
        }

        public void TestNaive()
        {
            found = false;
            running = true;

            if (number == 2)
            {
                found = true;
            }

            if (number % 2 != 0)
            {
                found = NaiveTest(number);
            }

            running = false;
        }

        private bool NaiveTest(BigInteger r)
        {
            if (r < 3)
            {
                return true;
            }

            iterationTested = 0;

            BigInteger g = Sqrt(r);

            BigInteger j = 3;

            while (j <= g)
            {
                iterationTested++;

                if (number % j == 0)
                {
                    break;
                }

                j = j + 2;
            }

            if (j > g)
            {
                return true;
            }
            return false;
        }


        /*
         * this function checks if the number is prime by using rabin-miller test. for more info see: https://en.wikipedia.org/wiki/Miller%E2%80%93Rabin_primality_test
         * */
        private bool RabinMillerTest(BigInteger r, BigInteger s)
        {
            iterationTested = 0;

            //
            // Find D and K so equality is correct: d*2^k = r - 1
            //

            BigInteger d = r - 1;
            BigInteger k = 0;

            while (d % 2 == 0)
            {
                d = d / 2;
                k = k + 1;
            }

            for (BigInteger j = 1; j <= s; j++)
            {
                iterationTested++;

                BigInteger a = Generator.Random(2, (r - 1));
                BigInteger x = BigInteger.ModPow(a, d, r);

                if (x != 1)
                {
                    for (BigInteger i = 0; i < (k - 1); i++)
                    {
                        if (x == number - 1)
                        {
                            break;
                        }

                        x = BigInteger.ModPow(x, 2, number);
                    }

                    if (x != number - 1)
                    {
                        return false;
                    }
                }

                if (running == false)
                {
                    return false;
                }
            }

            return true;
        }

        /*
         * sqrt for bigInteger
         * */
        private BigInteger Sqrt(BigInteger n)
        {
            if (n == 0)
            {
                return 0;
            }

            if (n > 0)
            {
                int bitLength = Convert.ToInt32(Math.Ceiling(BigInteger.Log(n, 2)));
                BigInteger root = BigInteger.One << (bitLength / 2);

                while (!IsSqrt(n, root))
                {
                    root += n / root;
                    root /= 2;
                }

                return root;
            }

            throw new ArithmeticException("NaN");
        }

        private bool IsSqrt(BigInteger n, BigInteger root)
        {
            BigInteger lowerBound = root * root;
            BigInteger upperBound = (root + 1) * (root + 1);

            return ((n >= lowerBound) && (n < upperBound));
        }


        public void SetNumber(BigInteger num)
        {
            number = num;
        }

        public void SetRabinMiller(BigInteger sNew)
        {
            if (sNew < 2)
            {
                sNew = 2;
            }

            s = sNew;
        }

        public void RabinMiller()
        {
            running = true;
            found = false;

            // No negative primes
            if (number < 1)
            {
                number = 1;
            }

            // Two is prime
            if (number <= 2)
            {
                return;
            }

            // Other even numbers arent primes
            if (number % 2 == 0)
            {
                number = number + 1;
            }

            // First five is prime
            if (number == 5)
            {
                running = false;
                found = true;

                return;
            }

            numberTested = 0;

            while (running)
            {
                if (RabinMillerTest(number, s))
                {
                    found = true;
                    running = false;

                    return;
                }

                // Skip number 5
                if (number % 10 == 3)
                {
                    number = number + 4;
                }
                else
                {
                    number = number + 2;
                }

                numberTested = numberTested + 1;
            }
        }

        public void Naive()
        {
            running = true;
            found = false;

            numberTested = 0;

            // No negative primes
            if (number < 1)
            {
                number = 1;
            }

            // Two is prime
            if (number <= 2)
            {
                return;
            }

            // Other even numbers arent primes
            if (number % 2 == 0)
            {
                number = number + 1;
            }

            // First five is prime
            if (number == 5)
            {
                running = false;
                found = true;

                return;
            }

            while (running)
            {
                if (NaiveTest(number))
                {
                    found = true;
                    running = false;

                    return;
                }

                // Skip number 5
                if (number % 10 == 3)
                {
                    number = number + 4;
                }
                else
                {
                    number = number + 2;
                }

                numberTested = numberTested + 1;
            }
        }
    }
}