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
            if (range.Start.CompareTo(range.End) > 0)
            {
                return false;
            }

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

        public static bool Contains<T>(this IRange<T> range, IRange<T> other, bool startInclusive = true, bool endInclusive = false) where T : IComparable<T>
        {
            if (range.Start.CompareTo(range.End) > 0 ||
                other.Start.CompareTo(other.End) > 0)
            {
                return false;
            }

            var startToStart = range.Start.CompareTo(other.Start);
            var endToEnd = range.End.CompareTo(other.End);

            if (startToStart > 0 || // The other range start before this range.
                endToEnd < 0)       // The other range ends after this range.
            {
                return false;
            }

            var startToEnd = range.Start.CompareTo(other.End);
            var endToStart = range.End.CompareTo(other.Start);

            if (startToStart < 0)
            {
                // The other range starts after this one does.

                if (endToStart == 0 && !endInclusive)
                {
                    // The other range ends when this one ends, but the end is not inclusive.
                    return false;
                }
            }

            if (endToEnd > 0)
            {
                // The other range ends before this one does.

                if (startToEnd == 0 && !startInclusive)
                {
                    // The other range starts when this one ends, but the start is not inclusive.
                    return false;
                }
            }

            if (startToEnd == 0 &&
                !startInclusive &&
                !endInclusive)
            {
                // Both ranges include exactly one point, but the point is not inclusive on either end.
                return false;
            }

            return true;
        }
    }
}
