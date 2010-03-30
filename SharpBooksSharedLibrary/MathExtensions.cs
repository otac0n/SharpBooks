namespace My
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

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
