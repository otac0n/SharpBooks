//-----------------------------------------------------------------------
// <copyright file="RangeExtensions.cs" company="(none)">
//  Copyright © 2011 John Gietzen. All rights reserved.
// </copyright>
// <author>John Gietzen</author>
//-----------------------------------------------------------------------

namespace SharpBooks.Integration
{
    using System;

    public static class RangeExtensions
    {
        public static bool Contains<T>(this IRange<T> range, T value, bool startInclusive = true, bool endInclusive = false) where T : IComparable<T>
        {
            var start = range.Start.CompareTo(value);
            var end = range.End.CompareTo(value);

            if (startInclusive && start == 0 ||
                endInclusive && end == 0)
            {
                return true;
            }

            if (start >= 0 ||
                end <= 0)
            {
                return false;
            }

            return true;
        }
    }
}
