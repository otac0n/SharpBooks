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
        public static bool IsEmpty<T>(this IRange<T> range) where T : IComparable<T>
        {
            if (range == null)
            {
                return true;
            }

            var startToEnd = range.Start.CompareTo(range.End);
            if (startToEnd > 0)
            {
                return true;
            }
            else if (startToEnd == 0 && (!range.StartInclusive || !range.EndInclusive))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool Contains<T>(this IRange<T> range, T value) where T : IComparable<T>
        {
            if (range.IsEmpty())
            {
                return false;
            }

            var start = range.Start.CompareTo(value);
            var end = range.End.CompareTo(value);

            if (range.StartInclusive && start == 0 ||
                range.EndInclusive && end == 0)
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

        public static bool Contains<T>(this IRange<T> range, IRange<T> other) where T : IComparable<T>
        {
            if (other.IsEmpty())
            {
                return true;
            }

            var intersection = other.IntersectWith(range);

            return !intersection.IsEmpty() &&
                    intersection == other;
        }

        public static IRange<T> IntersectWith<T>(this IRange<T> range, IRange<T> other) where T : IComparable<T>
        {
            if (range.IsEmpty() ||
                other.IsEmpty())
            {
                return null;
            }

            T start, end;
            bool startInclusive, endInclusive;

            bool startMatchesRange = false,
                 startMatchesOther = false,
                 endMatchesRange = false,
                 endMatchesOther = false;

            var startToStart = range.Start.CompareTo(other.Start);
            if (startToStart > 0)
            {
                start = range.Start;
                startInclusive = range.StartInclusive;
                startMatchesRange = true;
            }
            else if (startToStart < 0)
            {
                start = other.Start;
                startInclusive = other.StartInclusive;
                startMatchesOther = true;
            }
            else
            {
                start = range.Start;
                startInclusive = range.StartInclusive && other.StartInclusive;
                startMatchesRange = startInclusive == range.StartInclusive;
                startMatchesOther = startInclusive == other.StartInclusive;
            }

            var endToEnd = range.End.CompareTo(other.End);
            if (endToEnd < 0)
            {
                end = range.End;
                endInclusive = range.EndInclusive;
                endMatchesRange = true;
            }
            else if (endToEnd > 0)
            {
                end = other.End;
                endInclusive = other.EndInclusive;
                endMatchesOther = true;
            }
            else
            {
                end = range.End;
                endInclusive = range.EndInclusive && other.EndInclusive;
                endMatchesRange = endInclusive == range.EndInclusive;
                endMatchesOther = endInclusive == other.EndInclusive;
            }

            var startToEnd = start.CompareTo(end);
            if (startToEnd > 0)
            {
                return null;
            }
            else if (startToEnd == 0 && (!startInclusive || !endInclusive))
            {
                return null;
            }

            if (startMatchesRange && endMatchesRange)
            {
                return range;
            }
            else if (startMatchesOther && endMatchesOther)
            {
                return other;
            }

            return range.Clone(
                start,
                startInclusive,
                end,
                endInclusive);
        }
    }
}
