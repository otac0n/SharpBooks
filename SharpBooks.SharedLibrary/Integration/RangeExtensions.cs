// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace SharpBooks.Integration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class RangeExtensions
    {
        public static bool Contains<T>(this IRange<T> range, T value)
            where T : IComparable<T>
        {
            if (range.IsEmpty())
            {
                return false;
            }

            var start = range.Start.CompareTo(value);
            var end = range.End.CompareTo(value);

            if ((range.StartInclusive && start == 0) ||
                (range.EndInclusive && end == 0))
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

        public static bool Contains<T>(this IRange<T> range, IRange<T> other)
            where T : IComparable<T>
        {
            if (other.IsEmpty())
            {
                return true;
            }

            var intersection = other.IntersectWith(range);

            return !intersection.IsEmpty() &&
                    intersection == other;
        }

        public static bool Contains<T>(this IEnumerable<IRange<T>> set, T value)
            where T : IComparable<T>
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

        public static IList<IRange<T>> DifferenceWith<T>(this IRange<T> range, IRange<T> other)
            where T : IComparable<T>
        {
            if (range.IsEmpty())
            {
                return null;
            }

            var intersection = range.IntersectWith(other);

            if (intersection.IsEmpty())
            {
                return new[] { range };
            }
            else if (intersection == range)
            {
                return null;
            }

            var ranges = new List<IRange<T>>();

            var startToStart = range.Start.CompareTo(intersection.Start);

            if (startToStart != 0 ||
                (range.StartInclusive && !intersection.StartInclusive))
            {
                ranges.Add(range.Clone(
                        range.Start,
                        range.StartInclusive,
                        intersection.Start,
                        !intersection.StartInclusive));
            }

            var endToEnd = range.End.CompareTo(intersection.End);

            if (endToEnd != 0 ||
                (range.EndInclusive && !intersection.EndInclusive))
            {
                ranges.Add(range.Clone(
                        intersection.End,
                        !intersection.EndInclusive,
                        range.End,
                        range.EndInclusive));
            }

            return ranges;
        }

        public static IList<IRange<T>> DifferenceWith<T>(this IEnumerable<IRange<T>> set, IRange<T> range)
            where T : IComparable<T>
        {
            return set.DifferenceWith(new[] { range });
        }

        public static IList<IRange<T>> DifferenceWith<T>(this IRange<T> range, IEnumerable<IRange<T>> set)
            where T : IComparable<T>
        {
            return set.DifferenceWith(new[] { range });
        }

        public static IList<IRange<T>> DifferenceWith<T>(this IEnumerable<IRange<T>> setA, IEnumerable<IRange<T>> setB)
            where T : IComparable<T>
        {
            if (setA.IsEmpty())
            {
                return null;
            }
            else if (setB.IsEmpty())
            {
                return setA.ToList();
            }

            List<IRange<T>> results = null;

            foreach (var rangeB in setB)
            {
                results = new List<IRange<T>>();

                foreach (var rangeA in setA)
                {
                    var diff = rangeA.DifferenceWith(rangeB);
                    if (diff != null)
                    {
                        results.AddRange(diff);
                    }
                }

                if (results.Count == 0)
                {
                    return null;
                }

                setA = results;
            }

            return results;
        }

        public static IRange<T> IntersectWith<T>(this IRange<T> range, IRange<T> other)
            where T : IComparable<T>
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

        public static bool IsEmpty<T>(this IRange<T> range)
            where T : IComparable<T>
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

        public static bool IsEmpty<T>(this IEnumerable<IRange<T>> set)
            where T : IComparable<T>
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

        public static IList<IRange<T>> Simplify<T>(this IEnumerable<IRange<T>> set)
            where T : IComparable<T>
        {
            var list = (from r in set
                        where !r.IsEmpty()
                        orderby r.Start descending
                        select r).ToList();

            if (list.Count == 0)
            {
                return null;
            }

            Func<IRange<T>> dequeue = () =>
            {
                var a = list[list.Count - 1];
                list.RemoveAt(list.Count - 1);
                return a;
            };

            var results = new Stack<IRange<T>>();
            results.Push(dequeue());

            while (list.Count > 0)
            {
                var rangeA = results.Pop();
                var rangeB = dequeue();

                foreach (var result in rangeA.UnionWith(rangeB))
                {
                    results.Push(result);
                }
            }

            return results.ToArray();
        }

        public static IList<IRange<T>> UnionWith<T>(this IRange<T> range, IRange<T> other)
            where T : IComparable<T>
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
                    endInclusive),
            };
        }

        public static IList<IRange<T>> UnionWith<T>(this IEnumerable<IRange<T>> set, IRange<T> range)
            where T : IComparable<T>
        {
            return set.UnionWith(new[] { range });
        }

        public static IList<IRange<T>> UnionWith<T>(this IRange<T> range, IEnumerable<IRange<T>> set)
            where T : IComparable<T>
        {
            return set.UnionWith(new[] { range });
        }

        public static IList<IRange<T>> UnionWith<T>(this IEnumerable<IRange<T>> setA, IEnumerable<IRange<T>> setB)
            where T : IComparable<T>
        {
            setA = setA ?? new IRange<T>[0];
            setB = setB ?? new IRange<T>[0];

            return setA.Concat(setB).Simplify();
        }
    }
}
