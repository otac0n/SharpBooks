// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace SharpBooks
{
    using System;

    public static class FormsHelpers
    {
        public static T Clamp<T>(this T value, T min, T max)
            where T : IComparable<T>
        {
            return
                value.CompareTo(min) < 0 ? min :
                value.CompareTo(max) > 0 ? max :
                value;
        }
    }
}
