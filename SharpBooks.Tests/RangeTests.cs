﻿//-----------------------------------------------------------------------
// <copyright file="RangeTests.cs" company="(none)">
//  Copyright © 2011 John Gietzen. All rights reserved.
// </copyright>
// <author>John Gietzen</author>
//-----------------------------------------------------------------------

namespace SharpBooks.Tests
{
    using NUnit.Framework;
    using SharpBooks.Integration;

    [TestFixture]
    public class RangeTests
    {
        [Datapoints]
        private int[] intDatapoints = new[] { -3, -2, -1, 0, 1, 2, 3 };

        [Theory]
        public void IsEmpty_WithNullSet_ReturnsTrue(int start, int end, bool startInclusive, bool endInclusive)
        {
            NumberRange rangeA = null;

            var actual = rangeA.IsEmpty();

            Assert.That(actual, Is.True);
        }

        [Theory]
        public void IsEmpty_WithBackwardsSet_ReturnsTrue(int start, int end, bool startInclusive, bool endInclusive)
        {
            Assume.That(start > end);

            var rangeA = new NumberRange { Start = start, StartInclusive = startInclusive, End = end, EndInclusive = endInclusive };

            var actual = rangeA.IsEmpty();

            Assert.That(actual, Is.True);
        }

        [Theory]
        public void IsEmpty_WithSelfExcludedSet_ReturnsTrue(int startAndEnd)
        {
            var rangeA = new NumberRange { Start = startAndEnd, StartInclusive = false, End = startAndEnd, EndInclusive = false };

            var actual = rangeA.IsEmpty();

            Assert.That(actual, Is.True);
        }

        [Theory]
        public void IsEmpty_WithNonSelfExcludedSet_ReturnsFalse(int startAndEnd, bool startInclusive, bool endInclusive)
        {
            Assume.That(startInclusive || endInclusive);

            var rangeA = new NumberRange { Start = startAndEnd, StartInclusive = startInclusive, End = startAndEnd, EndInclusive = endInclusive };

            var actual = rangeA.IsEmpty();

            Assert.That(actual, Is.False);
        }

        [Theory]
        public void IsEmpty_WithInOrderSetSet_ReturnsFalse(int start, int end, bool startInclusive, bool endInclusive)
        {
            Assume.That(start < end);

            var rangeA = new NumberRange { Start = start, StartInclusive = startInclusive, End = end, EndInclusive = endInclusive };

            var actual = rangeA.IsEmpty();

            Assert.That(actual, Is.False);
        }

        [Theory]
        public void Contains_WithIncludedValue_ReturnsTrue(int start, int end, int value, bool startInclusive, bool endInclusive)
        {
            Assume.That(start < value && value < end);

            var range = new NumberRange { Start = start, StartInclusive = startInclusive, End = end, EndInclusive = endInclusive };

            var result = range.Contains(value);

            Assert.That(result, Is.True);
        }

        [Theory]
        public void Contains_WithInclusiveStart_ReturnsTrue(int start, int end, bool endInclusive)
        {
            Assume.That(start <= end);

            var range = new NumberRange { Start = start, StartInclusive = true, End = end, EndInclusive = endInclusive };

            var result = range.Contains(start);

            Assert.That(result, Is.True);
        }

        [Theory]
        public void Contains_WithInclusiveEnd_ReturnsTrue(int start, int end, bool startInclusive)
        {
            Assume.That(start <= end);

            var range = new NumberRange { Start = start, StartInclusive = startInclusive, End = end, EndInclusive = true };

            var result = range.Contains(end);

            Assert.That(result, Is.True);
        }

        [Theory]
        public void Contains_WithExclusiveStart_ReturnsFalse(int start, int end, bool endInclusive)
        {
            Assume.That(start < end);

            var range = new NumberRange { Start = start, StartInclusive = false, End = end, EndInclusive = endInclusive };

            var result = range.Contains(start);

            Assert.That(result, Is.False);
        }

        [Theory]
        public void Contains_WithExclusiveEnd_ReturnsFalse(int start, int end, bool startInclusive)
        {
            Assume.That(start < end);

            var range = new NumberRange { Start = start, StartInclusive = startInclusive, End = end, EndInclusive = false };

            var result = range.Contains(end);

            Assert.That(result, Is.False);
        }

        [Theory]
        public void Contains_WithInvalidRange_ReturnsFalse(int start, int end, bool startInclusive, bool endInclusive, int value)
        {
            Assume.That(start > end);

            var range = new NumberRange { Start = start, StartInclusive = startInclusive, End = end, EndInclusive = endInclusive };

            var result = range.Contains(value);

            Assert.That(result, Is.False);
        }

        [Theory]
        public void Contains_WithZeroLengthAndExclusiveStartAndEnd_ReturnsFalse(int startAndEnd, int value)
        {
            var range = new NumberRange { Start = startAndEnd, StartInclusive = false, End = startAndEnd, EndInclusive = false };

            var result = range.Contains(value);

            Assert.That(result, Is.False);
        }

        [Theory]
        public void Contains_WithNonIntersectingRange_ReturnsFalse(bool aStartInclusive, bool aEndInclusive, bool bStartInclusive, bool bEndInclusive)
        {
            var rangeA = new NumberRange { Start = 0, StartInclusive = aStartInclusive, End = 1, EndInclusive = aEndInclusive };
            var rangeB = new NumberRange { Start = 2, StartInclusive = bStartInclusive, End = 3, EndInclusive = bEndInclusive };

            var result = rangeA.Contains(rangeB);

            Assert.That(result, Is.False);
        }

        [Theory]
        public void Contains_WithInvalidRange_ReturnsFalse(int aStart, bool aStartInclusive, int aEnd, bool aEndInclusive, bool bStartInclusive, bool bEndInclusive)
        {
            var rangeA = new NumberRange { Start = aStart, StartInclusive = aStartInclusive, End = aStart, EndInclusive = aEndInclusive };
            var rangeB = new NumberRange { Start = 1, StartInclusive = bStartInclusive, End = 0, EndInclusive = bEndInclusive };

            var result = rangeA.Contains(rangeB);

            Assert.That(result, Is.False);
        }

        [Theory]
        public void Contains_FromInvalidRange_ReturnsFalse(bool aStartInclusive, bool aEndInclusive, int bStart, bool bStartInclusive, int bEnd, bool bEndInclusive)
        {
            var rangeA = new NumberRange { Start = 1, StartInclusive = aStartInclusive, End = 0, EndInclusive = aEndInclusive };
            var rangeB = new NumberRange { Start = bStart, StartInclusive = bStartInclusive, End = bEnd, EndInclusive = bEndInclusive };

            var result = rangeA.Contains(rangeB);

            Assert.That(result, Is.False);
        }

        [Theory]
        public void Contains_WithFullyContainedRange_ReturnsTrue(bool aStartInclusive, bool aEndInclusive, bool bStartInclusive, bool bEndInclusive)
        {
            var rangeA = new NumberRange { Start = 0, StartInclusive = aStartInclusive, End = 3, EndInclusive = aEndInclusive };
            var rangeB = new NumberRange { Start = 1, StartInclusive = bStartInclusive, End = 2, EndInclusive = bEndInclusive };

            var result = rangeA.Contains(rangeB);

            Assert.That(result, Is.True);
        }

        [Theory]
        public void Contains_WithRangeAdjacentlyIncludedAtStart_ReturnsTrue(bool aEndInclusive, bool bStartInclusive, bool bEndInclusive)
        {
            var rangeA = new NumberRange { Start = 0, StartInclusive = true, End = 3, EndInclusive = aEndInclusive };
            var rangeB = new NumberRange { Start = 0, StartInclusive = bStartInclusive, End = 2, EndInclusive = bEndInclusive };

            var result = rangeA.Contains(rangeB);

            Assert.That(result, Is.True);
        }

        [Theory]
        public void Contains_WithRangeAdjacentlyIncludedAtEnd_ReturnsTrue(bool aStartInclusive, bool bStartInclusive, bool bEndInclusive)
        {
            var rangeA = new NumberRange { Start = 0, StartInclusive = aStartInclusive, End = 3, EndInclusive = true };
            var rangeB = new NumberRange { Start = 1, StartInclusive = bStartInclusive, End = 3, EndInclusive = bEndInclusive };

            var result = rangeA.Contains(rangeB);

            Assert.That(result, Is.True);
        }

        [Theory]
        public void Contains_WithRangeAdjacentlyExcludedAtStart_ReturnsFalse(bool aEndInclusive, bool bEndInclusive)
        {
            var rangeA = new NumberRange { Start = 0, StartInclusive = false, End = 3, EndInclusive = aEndInclusive };
            var rangeB = new NumberRange { Start = 0, StartInclusive = true, End = 2, EndInclusive = bEndInclusive };

            var result = rangeA.Contains(rangeB);

            Assert.That(result, Is.False);
        }

        [Theory]
        public void Contains_WithRangeAdjacentlyExcludedAtEnd_ReturnsTrue(bool aStartInclusive, bool bStartInclusive)
        {
            var rangeA = new NumberRange { Start = 0, StartInclusive = aStartInclusive, End = 3, EndInclusive = false };
            var rangeB = new NumberRange { Start = 1, StartInclusive = bStartInclusive, End = 3, EndInclusive = true };

            var result = rangeA.Contains(rangeB);

            Assert.That(result, Is.False);
        }

        [Theory]
        public void Contains_WithZeroRangeInclusive_ReturnsTrue(bool aStartInclusive, bool aEndInclusive, bool bStartInclusive, bool bEndInclusively)
        {
            Assume.That(aStartInclusive || aEndInclusive);

            var rangeA = new NumberRange { Start = 0, StartInclusive = aStartInclusive, End = 0, EndInclusive = aEndInclusive };
            var rangeB = new NumberRange { Start = 0, StartInclusive = bStartInclusive, End = 0, EndInclusive = bEndInclusively };

            var result = rangeA.Contains(rangeB);

            Assert.That(result, Is.True);
        }

        [Theory]
        public void Contains_WithZeroRangeExclusive_ReturnsFalse(bool bStartInclusive, bool bEndInclusively)
        {
            var rangeA = new NumberRange { Start = 0, StartInclusive = false, End = 0, EndInclusive = false };
            var rangeB = new NumberRange { Start = 0, StartInclusive = bStartInclusive, End = 0, EndInclusive = bEndInclusively };

            var result = rangeA.Contains(rangeB);

            Assert.That(result, Is.False);
        }

        [Theory]
        public void Contains_WithEndAdjacentZeroRangeExcluded_ReturnsFalse(bool aStartInclusive, bool bStartInclusive, bool bEndInclusively)
        {
            var rangeA = new NumberRange { Start = 0, StartInclusive = aStartInclusive, End = 3, EndInclusive = false };
            var rangeB = new NumberRange { Start = 3, StartInclusive = bStartInclusive, End = 3, EndInclusive = bEndInclusively };

            var result = rangeA.Contains(rangeB);

            Assert.That(result, Is.False);
        }

        [Theory]
        public void Contains_WithEndAdjacentZeroRangeIncluded_ReturnsTrue(bool aStartInclusive, bool bStartInclusive, bool bEndInclusively)
        {
            var rangeA = new NumberRange { Start = 0, StartInclusive = aStartInclusive, End = 3, EndInclusive = true };
            var rangeB = new NumberRange { Start = 3, StartInclusive = bStartInclusive, End = 3, EndInclusive = bEndInclusively };

            var result = rangeA.Contains(rangeB);

            Assert.That(result, Is.True);
        }

        [Theory]
        public void Contains_WithStartAdjacentZeroRangeExcluded_ReturnsFalse(bool aEndInclusive, bool bStartInclusive, bool bEndInclusively)
        {
            var rangeA = new NumberRange { Start = 0, StartInclusive = false, End = 3, EndInclusive = aEndInclusive };
            var rangeB = new NumberRange { Start = 0, StartInclusive = bStartInclusive, End = 0, EndInclusive = bEndInclusively };

            var result = rangeA.Contains(rangeB);

            Assert.That(result, Is.False);
        }

        [Theory]
        public void Contains_WithStartAdjacentZeroRangeIncluded_ReturnsTrue(bool aEndInclusive, bool bStartInclusive, bool bEndInclusively)
        {
            var rangeA = new NumberRange { Start = 0, StartInclusive = true, End = 3, EndInclusive = aEndInclusive };
            var rangeB = new NumberRange { Start = 0, StartInclusive = bStartInclusive, End = 0, EndInclusive = bEndInclusively };

            var result = rangeA.Contains(rangeB);

            Assert.That(result, Is.True);
        }

        [Theory]
        public void IntersectWith_WithInvalidRange_ReturnsNull(bool aStartInclusive, bool aEndInclusive, int bStart, bool bStartInclusive, int bEnd, bool bEndInclusive)
        {
            var rangeA = new NumberRange { Start = 1, StartInclusive = aStartInclusive, End = 0, EndInclusive = aEndInclusive };
            var rangeB = new NumberRange { Start = bStart, StartInclusive = bStartInclusive, End = bEnd, EndInclusive = bEndInclusive };

            var actual = rangeA.IntersectWith(rangeB);

            Assert.That(actual, Is.Null);
        }

        [Theory]
        public void IntersectWith_FromInvalidRange_ReturnsNull(int aStart, bool aStartInclusive, int aEnd, bool aEndInclusive, bool bStartInclusive, bool bEndInclusive)
        {
            var rangeA = new NumberRange { Start = aStart, StartInclusive = aStartInclusive, End = aEnd, EndInclusive = aEndInclusive };
            var rangeB = new NumberRange { Start = 1, StartInclusive = bStartInclusive, End = 0, EndInclusive = bEndInclusive };

            var actual = rangeA.IntersectWith(rangeB);

            Assert.That(actual, Is.Null);
        }

        [Theory]
        public void IntersectWith_WithSameRange_ReturnsOriginalReference(bool aStartInclusive, bool aEndInclusive)
        {
            var rangeA = new NumberRange { Start = 0, StartInclusive = aStartInclusive, End = 3, EndInclusive = aEndInclusive };

            var actual = rangeA.IntersectWith(rangeA);

            Assert.That(actual, Is.EqualTo(rangeA));
        }

        [Theory]
        public void IntersectWith_WithWhollyContianedRange_ReturnsContainedRange(bool aStartInclusive, bool aEndInclusive, bool bStartInclusive, bool bEndInclusive)
        {
            var rangeA = new NumberRange { Start = 0, StartInclusive = aStartInclusive, End = 3, EndInclusive = aEndInclusive };
            var rangeB = new NumberRange { Start = 1, StartInclusive = bStartInclusive, End = 2, EndInclusive = bEndInclusive };

            var actual = rangeA.IntersectWith(rangeB);

            Assert.That(actual, Is.EqualTo(rangeB));
        }

        [Theory]
        public void IntersectWith_WhenRangesDoNotIntersect_ReturnsNull(bool aStartInclusive, bool aEndInclusive, bool bStartInclusive, bool bEndInclusive)
        {
            var rangeA = new NumberRange { Start = 0, StartInclusive = aStartInclusive, End = 1, EndInclusive = aEndInclusive };
            var rangeB = new NumberRange { Start = 2, StartInclusive = bStartInclusive, End = 3, EndInclusive = bEndInclusive };

            var actual = rangeA.IntersectWith(rangeB);

            Assert.That(actual, Is.Null);
        }

        [Theory]
        public void IntersectWith_WhenRangesIntersectInAPoint_ReturnsThatPointWithMatchingInclusivity(bool aStartInclusive, bool bEndInclusive)
        {
            var rangeA = new NumberRange { Start = 0, StartInclusive = aStartInclusive, End = 2, EndInclusive = true };
            var rangeB = new NumberRange { Start = 2, StartInclusive = true, End = 3, EndInclusive = bEndInclusive };

            var actual = rangeA.IntersectWith(rangeB);

            Assert.That(actual.Start, Is.EqualTo(rangeB.Start));
            Assert.That(actual.End, Is.EqualTo(rangeA.End));
            Assert.That(actual.StartInclusive || actual.EndInclusive, Is.EqualTo(true));
        }

        [Theory]
        public void IntersectWith_WhenRangesIntersectInAnExclusivePoint_ReturnsNull(bool aStartInclusive, bool aEndInclusive, bool bStartInclusive, bool bEndInclusive)
        {
            Assume.That(!aEndInclusive || !bStartInclusive);

            var rangeA = new NumberRange { Start = 0, StartInclusive = aStartInclusive, End = 2, EndInclusive = aEndInclusive };
            var rangeB = new NumberRange { Start = 2, StartInclusive = bStartInclusive, End = 3, EndInclusive = bEndInclusive };

            var actual = rangeA.IntersectWith(rangeB);

            Assert.That(actual, Is.Null);
        }

        [Theory]
        public void IntersectWith_WhenRangesIntersectInARange_ReturnsThatRangeWithMatchingInclusivity(bool aStartInclusive, bool aEndInclusive, bool bStartInclusive, bool bEndInclusive)
        {
            var rangeA = new NumberRange { Start = 0, StartInclusive = aStartInclusive, End = 2, EndInclusive = aEndInclusive };
            var rangeB = new NumberRange { Start = 1, StartInclusive = bStartInclusive, End = 3, EndInclusive = bEndInclusive };

            var actual = rangeA.IntersectWith(rangeB);

            Assert.That(actual.Start, Is.EqualTo(rangeB.Start));
            Assert.That(actual.StartInclusive, Is.EqualTo(rangeB.StartInclusive));
            Assert.That(actual.End, Is.EqualTo(rangeA.End));
            Assert.That(actual.EndInclusive, Is.EqualTo(rangeA.EndInclusive));
        }

        [Theory]
        public void IntersectWith_WhenRangesIntersectInARangeReversed_ReturnsThatRangeWithMatchingInclusivity(bool aStartInclusive, bool aEndInclusive, bool bStartInclusive, bool bEndInclusive)
        {
            var rangeA = new NumberRange { Start = 0, StartInclusive = aStartInclusive, End = 2, EndInclusive = aEndInclusive };
            var rangeB = new NumberRange { Start = 1, StartInclusive = bStartInclusive, End = 3, EndInclusive = bEndInclusive };

            var actual = rangeB.IntersectWith(rangeA);

            Assert.That(actual.Start, Is.EqualTo(rangeB.Start));
            Assert.That(actual.StartInclusive, Is.EqualTo(rangeB.StartInclusive));
            Assert.That(actual.End, Is.EqualTo(rangeA.End));
            Assert.That(actual.EndInclusive, Is.EqualTo(rangeA.EndInclusive));
        }

        [Theory]
        public void IntersectWith_WhenRangesAreSameSize_ReturnsRangeWithMostRestrictedInclusivity(bool aStartInclusive, bool aEndInclusive, bool bStartInclusive, bool bEndInclusive)
        {
            var rangeA = new NumberRange { Start = 0, StartInclusive = aStartInclusive, End = 3, EndInclusive = aEndInclusive };
            var rangeB = new NumberRange { Start = 0, StartInclusive = bStartInclusive, End = 3, EndInclusive = bEndInclusive };

            var actual = rangeA.IntersectWith(rangeB);

            Assert.That(actual.Start, Is.EqualTo(rangeB.Start));
            Assert.That(actual.StartInclusive, Is.EqualTo(rangeA.StartInclusive && rangeB.StartInclusive));
            Assert.That(actual.End, Is.EqualTo(rangeA.End));
            Assert.That(actual.EndInclusive, Is.EqualTo(rangeA.EndInclusive && rangeB.EndInclusive));
        }

        [Theory]
        public void IntersectWith_WhenRangesAreSameSizeAndThisRangeMatchesMostRestrictive_ReturnsThisRange(bool aStartInclusive, bool aEndInclusive, bool bStartInclusive, bool bEndInclusive)
        {
            Assume.That(bStartInclusive || bEndInclusive);
            Assume.That(bStartInclusive || (!bStartInclusive && !aStartInclusive));
            Assume.That(bEndInclusive || (!bEndInclusive && !aEndInclusive));

            var rangeA = new NumberRange { Start = 0, StartInclusive = aStartInclusive, End = 3, EndInclusive = aEndInclusive };
            var rangeB = new NumberRange { Start = 0, StartInclusive = bStartInclusive, End = 3, EndInclusive = bEndInclusive };

            var actual = rangeA.IntersectWith(rangeB);

            Assert.That(actual, Is.EqualTo(rangeA));
        }

        [Theory]
        public void IntersectWith_WhenRangesAreSameSizeAndOtherRangeMatchesMostRestrictive_ReturnsOtherRange(bool aStartInclusive, bool aEndInclusive, bool bStartInclusive, bool bEndInclusive)
        {
            Assume.That(aStartInclusive || aEndInclusive);
            Assume.That(aStartInclusive || (!aStartInclusive && !bStartInclusive));
            Assume.That(aEndInclusive || (!aEndInclusive && !bEndInclusive));
            Assume.That((!bStartInclusive && aStartInclusive) || (!bEndInclusive && aEndInclusive));

            var rangeA = new NumberRange { Start = 0, StartInclusive = aStartInclusive, End = 3, EndInclusive = aEndInclusive };
            var rangeB = new NumberRange { Start = 0, StartInclusive = bStartInclusive, End = 3, EndInclusive = bEndInclusive };

            var actual = rangeA.IntersectWith(rangeB);

            Assert.That(actual, Is.EqualTo(rangeB));
        }

        [Theory]
        public void IntersectWith_WhenThisRangeIsEmpty_ReturnsNull(int bStart, bool bStartInclusive, int bEnd, bool bEndInclusive)
        {
            var rangeA = new NumberRange { Start = 0, StartInclusive = false, End = 0, EndInclusive = false };
            var rangeB = new NumberRange { Start = bStart, StartInclusive = bStartInclusive, End = bEnd, EndInclusive = bEndInclusive };

            var actual = rangeA.IntersectWith(rangeB);

            Assert.That(actual, Is.Null);
        }

        [Theory]
        public void IntersectWith_WhenOtherRangeIsEmpty_ReturnsNull(int aStart, bool aStartInclusive, int aEnd, bool aEndInclusive)
        {
            var rangeA = new NumberRange { Start = aStart, StartInclusive = aStartInclusive, End = aEnd, EndInclusive = aEndInclusive };
            var rangeB = new NumberRange { Start = 0, StartInclusive = false, End = 0, EndInclusive = false };

            var actual = rangeA.IntersectWith(rangeB);

            Assert.That(actual, Is.Null);
        }

        private class NumberRange : IRange<int>
        {
            public int Start { get; set; }

            public bool StartInclusive { get; set; }

            public int End { get; set; }

            public bool EndInclusive { get; set; }

            public NumberRange Clone(int start, bool startInclusive, int end, bool endInclusive)
            {
                return new NumberRange
                {
                    Start = start,
                    StartInclusive = startInclusive,
                    End = end,
                    EndInclusive = endInclusive,
                };
            }

            IRange<int> IRange<int>.Clone(int start, bool startInclusive, int end, bool endInclusive)
            {
                return this.Clone(start, startInclusive, end, endInclusive);
            }

            public override string ToString()
            {
                return
                    (StartInclusive ? "[" : "(") +
                    Start +
                    "," +
                    End +
                    (EndInclusive ? "]" : ")");
            }
        }
    }
}
