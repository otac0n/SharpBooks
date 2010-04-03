//-----------------------------------------------------------------------
// <copyright file="MathExtensions.cs" company="(none)">
//  Copyright © 2010 John Gietzen. All rights reserved.
// </copyright>
// <author>John Gietzen</author>
//-----------------------------------------------------------------------

namespace My
{
    public static class Math
    {
        public static long GCD(long a, long b)
        {
            return b == 0 ? a : GCD(b, a % b);
        }

        public static int GCD(int a, int b)
        {
            return b == 0 ? a : GCD(b, a % b);
        }
    }
}
