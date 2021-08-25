// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace SharpBooks
{
    public static class FinancialMath
    {
        public static long GCD(long dividendA, long dividendB)
        {
            while (dividendB != 0)
            {
                var remainder = dividendA % dividendB;
                dividendA = dividendB;
                dividendB = remainder;
            }

            return dividendA;
        }

        public static int GCD(int dividendA, int dividendB)
        {
            while (dividendB != 0)
            {
                var remainder = dividendA % dividendB;
                dividendA = dividendB;
                dividendB = remainder;
            }

            return dividendA;
        }
    }
}
