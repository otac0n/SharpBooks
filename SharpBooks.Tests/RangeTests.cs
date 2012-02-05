//-----------------------------------------------------------------------
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

        private class NumberRange : IRange<int>
        {
            public int Start { get; set; }

            public bool StartInclusive { get; set; }

            public int End { get; set; }

            public bool EndInclusive { get; set; }
        }
    }
}
