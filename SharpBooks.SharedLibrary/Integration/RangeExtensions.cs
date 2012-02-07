﻿//-----------------------------------------------------------------------
// <copyright file="RangeExtensions.cs" company="(none)">
//  Copyright © 2011 John Gietzen. All rights reserved.
// </copyright>
// <author>John Gietzen</author>
//-----------------------------------------------------------------------

namespace SharpBooks.Integration
{
    using System;
    using System.Collections.Generic;

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

        public static bool IsEmpty<T>(this IEnumerable<IRange<T>> set) where T : IComparable<T>
        {
            if (set == null)
            {
                return true;
            }

            foreach (var range in set)
            {
                if (!range.IsEmpty())
                {
                    return false;
                }
            }

            return true;
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

        public static bool Contains<T>(this IEnumerable<IRange<T>> set, T value) where T : IComparable<T>
        {
            foreach (var range in set)
            {
                if (range.Contains(value))
                {
                    return true;
                }
            }

            return false;
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

        public static IEnumerable<IRange<T>> UnionWith<T>(this IRange<T> range, IRange<T> other) where T : IComparable<T>
        {
            var rangeEmpty = range.IsEmpty();
            var otherEmpty = other.IsEmpty();

            if (rangeEmpty && otherEmpty)
            {
                return null;
            }
            else if (rangeEmpty)
            {
                return new[] { other };
            }
            else if (otherEmpty)
            {
                return new[] { range };
            }

            int startToStart, endToEnd, startToEnd;
            T start, end;
            bool startInclusive, endInclusive;

            startToStart = range.Start.CompareTo(other.Start);
            if (startToStart > 0)
            {
                start = range.Start;
                startInclusive = range.StartInclusive;
            }
            else if (startToStart < 0)
            {
                start = other.Start;
                startInclusive = other.StartInclusive;
            }
            else
            {
                start = range.Start;
                startInclusive = range.StartInclusive && other.StartInclusive;
            }

            endToEnd = range.End.CompareTo(other.End);
            if (endToEnd < 0)
            {
                end = range.End;
                endInclusive = range.EndInclusive;
            }
            else if (endToEnd > 0)
            {
                end = other.End;
                endInclusive = other.EndInclusive;
            }
            else
            {
                end = range.End;
                endInclusive = range.EndInclusive && other.EndInclusive;
            }

            startToEnd = start.CompareTo(end);
            if (startToEnd > 0)
            {
                return new[] { range, other };
            }
            else if (startToEnd == 0 && !(startInclusive || endInclusive))
            {
                return new[] { range, other };
            }

            bool startMatchesRange = false,
                 startMatchesOther = false,
                 endMatchesRange = false,
                 endMatchesOther = false;

            if (startToStart < 0)
            {
                start = range.Start;
                startInclusive = range.StartInclusive;
                startMatchesRange = true;
            }
            else if (startToStart > 0)
            {
                start = other.Start;
                startInclusive = other.StartInclusive;
                startMatchesOther = true;
            }
            else
            {
                start = range.Start;
                startInclusive = range.StartInclusive || other.StartInclusive;
                startMatchesRange = startInclusive == range.StartInclusive;
                startMatchesOther = startInclusive == other.StartInclusive;
            }

            if (endToEnd > 0)
            {
                end = range.End;
                endInclusive = range.EndInclusive;
                endMatchesRange = true;
            }
            else if (endToEnd < 0)
            {
                end = other.End;
                endInclusive = other.EndInclusive;
                endMatchesOther = true;
            }
            else
            {
                end = range.End;
                endInclusive = range.EndInclusive || other.EndInclusive;
                endMatchesRange = endInclusive == range.EndInclusive;
                endMatchesOther = endInclusive == other.EndInclusive;
            }

            if (startMatchesRange && endMatchesRange)
            {
                return new[] { range };
            }
            else if (startMatchesOther && endMatchesOther)
            {
                return new[] { other };
            }

            return new[]
            {
                range.Clone(
                    start,
                    startInclusive,
                    end,
                    endInclusive)
            };
        }
    }
}
