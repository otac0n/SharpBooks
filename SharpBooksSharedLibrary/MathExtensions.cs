namespace SharpBooks
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public static class MathExtensions
    {
        public static long GCD(this long a, long b)
        {
            return b == 0 ? a : b.GCD(a % b);
        }

        public static int GCD(this int a, int b)
        {
            return b == 0 ? a : b.GCD(a % b);
        }
    }
}
