/*
 Written by Shlomo Sahar, 2015
 */
using System;
using System.Numerics;


namespace RSAEngain
{
    static public class MathExtended
    {
        public static BigInteger ModularLinearEquationSolver(BigInteger a, BigInteger b, BigInteger n)
        {
            BigInteger x;
            BigInteger y;
            BigInteger d;

            ExtendedEuclid(a, n, out d, out x, out y);

            if (b % d == 0)
            {
                x = (x * (b / d)) % n;

                if (x < 0)
                {
                    return (x + n) % n;
                }
                return x;
            }
            return -1;
        }

        static void ExtendedEuclid(BigInteger a, BigInteger b, out BigInteger d, out BigInteger lastx, out BigInteger lasty)
        {
            BigInteger x = 0;
            BigInteger y = 1;

            lastx = 1;
            lasty = 0;

            while (b != 0)
            {
                BigInteger quotient = a / b;
                BigInteger temp = b;

                b = a % b;
                a = temp;

                temp = x;
                x = lastx - quotient * x;
                lastx = temp;

                temp = y;
                y = lasty - quotient * y;
                lasty = temp;
            }

            d = a;
        }
    }
}