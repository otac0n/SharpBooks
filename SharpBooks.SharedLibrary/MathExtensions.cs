//-----------------------------------------------------------------------
// <copyright file="MathExtensions.cs" company="(none)">
//  Copyright © 2010 John Gietzen. All rights reserved.
// </copyright>
// <author>John Gietzen</author>
//-----------------------------------------------------------------------

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
