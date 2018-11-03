/*
 Written by Shlomo Sahar, 2015
 */
using System;
using System.Diagnostics;
using System.Globalization;
using System.Numerics;


namespace RSAEngain
{
    static public class Generator
    {
        private static int type;

        private static BigInteger m = BigInteger.Pow(2, 32);
        private static BigInteger a = 69069;
        private static BigInteger b = 0;
        private static BigInteger rn = 1;

        private static Random rnd = new Random();

        public static void Initialize(int t)
        {
            type = t;

            if (type == 0)
            {
                m = BigInteger.Pow(2, 32);
                a = 69069;
                b = 0;
                rn = 1;
            }
            else if (type == 1)
            {
                rnd = new Random();
            }
            else
            {
                rnd = new Random();
            }
        }

        public static void SetLcg(BigInteger mIn, BigInteger aIn, BigInteger bIn, BigInteger rnIn)
        {
            type = 0;

            m = mIn;
            a = aIn;
            b = bIn;
            rn = rnIn;
        }

        public static BigInteger Random(BigInteger a, BigInteger b)
        {
            BigInteger retValue;

            if (type == 0)
            {
                retValue = a + Lcg() % (b - a + 1);
            }
            else if (type == 1)
            {
                BigInteger count = b - a;

                BigInteger digits = 0;

                while ((count / 10) > 0)
                {
                    count = count / 10;

                    digits++;
                }

                string entropy = Entropy();

                string retVal = "";

                while (retVal.Length < digits)
                {
                    retVal = retVal + entropy[rnd.Next(0, entropy.Length)];
                }

                retValue = BigInteger.Parse(retVal);

                // the number might be too big, so do a mod
                retValue = a + (retValue % b);
            }
            else
            {
                BigInteger count = b - a;

                BigInteger digits = 0;

                while ((count / 10) > 0)
                {
                    count = count / 10;

                    digits++;
                }

                string retVal = rnd.Next(1000000000, 2100000000).ToString(CultureInfo.InvariantCulture);

                while (retVal.Length < digits)
                {
                    retVal = retVal + rnd.Next(1000).ToString(CultureInfo.InvariantCulture);
                }

                retValue = BigInteger.Parse(retVal);

                // the number might be too big, so do a mod
                retValue = a + (retValue % b);
            }

            return retValue;
        }

        private static string Entropy()
        {
            string entropy = DateTime.Now.Ticks.ToString(CultureInfo.InvariantCulture);

            PerformanceCounter cpuCounter = new PerformanceCounter
            {
                CategoryName = "Processor",
                CounterName = "% Processor Time",
                InstanceName = "_Total"
            };

            entropy = entropy + cpuCounter.NextValue();

            return entropy;
        }

        private static BigInteger Lcg()
        {
            rn = (a * rn + b) % m;
            return rn;
        }
    }
}
