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

            var range = new NumberRange { Start = start, End = end };

            var result = range.Contains(value, startInclusive: startInclusive, endInclusive: endInclusive);

            Assert.That(result, Is.True);
        }

        [Theory]
        public void Contains_WithInclusiveStart_ReturnsTrue(int start, int end, bool endInclusive)
        {
            Assume.That(start <= end);

            var range = new NumberRange { Start = start, End = end };

            var result = range.Contains(start, startInclusive: true, endInclusive: endInclusive);

            Assert.That(result, Is.True);
        }

        [Theory]
        public void Contains_WithInclusiveEnd_ReturnsTrue(int start, int end, bool startInclusive)
        {
            Assume.That(start <= end);

            var range = new NumberRange { Start = start, End = end };

            var result = range.Contains(end, startInclusive: startInclusive, endInclusive: true);

            Assert.That(result, Is.True);
        }

        [Theory]
        public void Contains_WithExclusiveStart_ReturnsFalse(int start, int end, bool endInclusive)
        {
            Assume.That(start < end);

            var range = new NumberRange { Start = start, End = end };

            var result = range.Contains(start, startInclusive: false, endInclusive: endInclusive);

            Assert.That(result, Is.False);
        }

        [Theory]
        public void Contains_WithExclusiveEnd_ReturnsFalse(int start, int end, bool startInclusive)
        {
            Assume.That(start < end);

            var range = new NumberRange { Start = start, End = end };

            var result = range.Contains(end, startInclusive: startInclusive, endInclusive: false);

            Assert.That(result, Is.False);
        }

        [Theory]
        public void Contains_WithInvalidRange_ReturnsFalse(int start, int end, bool startInclusive, bool endInclusive, int value)
        {
            Assume.That(start > end);

            var range = new NumberRange { Start = start, End = end };

            var result = range.Contains(value, startInclusive: startInclusive, endInclusive: endInclusive);

            Assert.That(result, Is.False);
        }

        [Theory]
        public void Contains_WithZeroLengthAndExclusiveStartAndEnd_ReturnsFalse(int startAndEnd, int value)
        {
            var range = new NumberRange { Start = startAndEnd, End = startAndEnd };

            var result = range.Contains(value, startInclusive: false, endInclusive: false);

            Assert.That(result, Is.False);
        }

        [TestCase(0, 0, 0, 0, true, false, true)]
        [TestCase(0, 0, 0, 0, true, true, true)]
        [TestCase(0, 0, 0, 0, false, false, false)]
        [TestCase(0, 0, 0, 0, false, true, true)]
        [TestCase(0, 0, 0, 1, true, false, false)]
        [TestCase(0, 0, 0, 1, true, true, false)]
        [TestCase(0, 0, 0, 1, false, false, false)]
        [TestCase(0, 0, 0, 1, false, true, false)]
        [TestCase(0, 0, 1, 0, true, false, false)]
        [TestCase(0, 0, 1, 0, true, true, false)]
        [TestCase(0, 0, 1, 0, false, false, false)]
        [TestCase(0, 0, 1, 0, false, true, false)]
        [TestCase(0, 0, 1, 1, true, false, false)]
        [TestCase(0, 0, 1, 1, true, true, false)]
        [TestCase(0, 0, 1, 1, false, false, false)]
        [TestCase(0, 0, 1, 1, false, true, false)]
        [TestCase(0, 0, 1, 2, true, false, false)]
        [TestCase(0, 0, 1, 2, true, true, false)]
        [TestCase(0, 0, 1, 2, false, false, false)]
        [TestCase(0, 0, 1, 2, false, true, false)]
        [TestCase(0, 0, 2, 1, true, false, false)]
        [TestCase(0, 0, 2, 1, true, true, false)]
        [TestCase(0, 0, 2, 1, false, false, false)]
        [TestCase(0, 0, 2, 1, false, true, false)]
        [TestCase(0, 1, 0, 0, true, false, true)]
        [TestCase(0, 1, 0, 0, true, true, true)]
        [TestCase(0, 1, 0, 0, false, false, false)]
        [TestCase(0, 1, 0, 0, false, true, false)]
        [TestCase(0, 1, 0, 1, true, false, true)]
        [TestCase(0, 1, 0, 1, true, true, true)]
        [TestCase(0, 1, 0, 1, false, false, true)]
        [TestCase(0, 1, 0, 1, false, true, true)]
        [TestCase(0, 1, 0, 2, true, false, false)]
        [TestCase(0, 1, 0, 2, true, true, false)]
        [TestCase(0, 1, 0, 2, false, false, false)]
        [TestCase(0, 1, 0, 2, false, true, false)]
        [TestCase(0, 1, 1, 0, true, false, false)]
        [TestCase(0, 1, 1, 0, true, true, false)]
        [TestCase(0, 1, 1, 0, false, false, false)]
        [TestCase(0, 1, 1, 0, false, true, false)]
        [TestCase(0, 1, 1, 1, true, false, false)]
        [TestCase(0, 1, 1, 1, true, true, true)]
        [TestCase(0, 1, 1, 1, false, false, false)]
        [TestCase(0, 1, 1, 1, false, true, true)]
        [TestCase(0, 1, 1, 2, true, false, false)]
        [TestCase(0, 1, 1, 2, true, true, false)]
        [TestCase(0, 1, 1, 2, false, false, false)]
        [TestCase(0, 1, 1, 2, false, true, false)]
        [TestCase(0, 1, 2, 0, true, false, false)]
        [TestCase(0, 1, 2, 0, true, true, false)]
        [TestCase(0, 1, 2, 0, false, false, false)]
        [TestCase(0, 1, 2, 0, false, true, false)]
        [TestCase(0, 1, 2, 1, true, false, false)]
        [TestCase(0, 1, 2, 1, true, true, false)]
        [TestCase(0, 1, 2, 1, false, false, false)]
        [TestCase(0, 1, 2, 1, false, true, false)]
        [TestCase(0, 1, 2, 2, true, false, false)]
        [TestCase(0, 1, 2, 2, true, true, false)]
        [TestCase(0, 1, 2, 2, false, false, false)]
        [TestCase(0, 1, 2, 2, false, true, false)]
        [TestCase(0, 1, 2, 3, true, false, false)]
        [TestCase(0, 1, 2, 3, true, true, false)]
        [TestCase(0, 1, 2, 3, false, false, false)]
        [TestCase(0, 1, 2, 3, false, true, false)]
        [TestCase(0, 1, 3, 2, true, false, false)]
        [TestCase(0, 1, 3, 2, true, true, false)]
        [TestCase(0, 1, 3, 2, false, false, false)]
        [TestCase(0, 1, 3, 2, false, true, false)]
        [TestCase(0, 2, 0, 1, true, false, true)]
        [TestCase(0, 2, 0, 1, true, true, true)]
        [TestCase(0, 2, 0, 1, false, false, true)]
        [TestCase(0, 2, 0, 1, false, true, true)]
        [TestCase(0, 2, 1, 0, true, false, false)]
        [TestCase(0, 2, 1, 0, true, true, false)]
        [TestCase(0, 2, 1, 0, false, false, false)]
        [TestCase(0, 2, 1, 0, false, true, false)]
        [TestCase(0, 2, 1, 1, true, false, true)]
        [TestCase(0, 2, 1, 1, true, true, true)]
        [TestCase(0, 2, 1, 1, false, false, true)]
        [TestCase(0, 2, 1, 1, false, true, true)]
        [TestCase(0, 2, 1, 2, true, false, true)]
        [TestCase(0, 2, 1, 2, true, true, true)]
        [TestCase(0, 2, 1, 2, false, false, true)]
        [TestCase(0, 2, 1, 2, false, true, true)]
        [TestCase(0, 2, 1, 3, true, false, false)]
        [TestCase(0, 2, 1, 3, true, true, false)]
        [TestCase(0, 2, 1, 3, false, false, false)]
        [TestCase(0, 2, 1, 3, false, true, false)]
        [TestCase(0, 2, 2, 1, true, false, false)]
        [TestCase(0, 2, 2, 1, true, true, false)]
        [TestCase(0, 2, 2, 1, false, false, false)]
        [TestCase(0, 2, 2, 1, false, true, false)]
        [TestCase(0, 2, 3, 1, true, false, false)]
        [TestCase(0, 2, 3, 1, true, true, false)]
        [TestCase(0, 2, 3, 1, false, false, false)]
        [TestCase(0, 2, 3, 1, false, true, false)]
        [TestCase(0, 3, 1, 2, true, false, true)]
        [TestCase(0, 3, 1, 2, true, true, true)]
        [TestCase(0, 3, 1, 2, false, false, true)]
        [TestCase(0, 3, 1, 2, false, true, true)]
        [TestCase(0, 3, 2, 1, true, false, false)]
        [TestCase(0, 3, 2, 1, true, true, false)]
        [TestCase(0, 3, 2, 1, false, false, false)]
        [TestCase(0, 3, 2, 1, false, true, false)]
        [TestCase(1, 0, 0, 0, true, false, false)]
        [TestCase(1, 0, 0, 0, true, true, false)]
        [TestCase(1, 0, 0, 0, false, false, false)]
        [TestCase(1, 0, 0, 0, false, true, false)]
        [TestCase(1, 0, 0, 1, true, false, false)]
        [TestCase(1, 0, 0, 1, true, true, false)]
        [TestCase(1, 0, 0, 1, false, false, false)]
        [TestCase(1, 0, 0, 1, false, true, false)]
        [TestCase(1, 0, 0, 2, true, false, false)]
        [TestCase(1, 0, 0, 2, true, true, false)]
        [TestCase(1, 0, 0, 2, false, false, false)]
        [TestCase(1, 0, 0, 2, false, true, false)]
        [TestCase(1, 0, 1, 0, true, false, false)]
        [TestCase(1, 0, 1, 0, true, true, false)]
        [TestCase(1, 0, 1, 0, false, false, false)]
        [TestCase(1, 0, 1, 0, false, true, false)]
        [TestCase(1, 0, 1, 1, true, false, false)]
        [TestCase(1, 0, 1, 1, true, true, false)]
        [TestCase(1, 0, 1, 1, false, false, false)]
        [TestCase(1, 0, 1, 1, false, true, false)]
        [TestCase(1, 0, 1, 2, true, false, false)]
        [TestCase(1, 0, 1, 2, true, true, false)]
        [TestCase(1, 0, 1, 2, false, false, false)]
        [TestCase(1, 0, 1, 2, false, true, false)]
        [TestCase(1, 0, 2, 0, true, false, false)]
        [TestCase(1, 0, 2, 0, true, true, false)]
        [TestCase(1, 0, 2, 0, false, false, false)]
        [TestCase(1, 0, 2, 0, false, true, false)]
        [TestCase(1, 0, 2, 1, true, false, false)]
        [TestCase(1, 0, 2, 1, true, true, false)]
        [TestCase(1, 0, 2, 1, false, false, false)]
        [TestCase(1, 0, 2, 1, false, true, false)]
        [TestCase(1, 0, 2, 2, true, false, false)]
        [TestCase(1, 0, 2, 2, true, true, false)]
        [TestCase(1, 0, 2, 2, false, false, false)]
        [TestCase(1, 0, 2, 2, false, true, false)]
        [TestCase(1, 0, 2, 3, true, false, false)]
        [TestCase(1, 0, 2, 3, true, true, false)]
        [TestCase(1, 0, 2, 3, false, false, false)]
        [TestCase(1, 0, 2, 3, false, true, false)]
        [TestCase(1, 0, 3, 2, true, false, false)]
        [TestCase(1, 0, 3, 2, true, true, false)]
        [TestCase(1, 0, 3, 2, false, false, false)]
        [TestCase(1, 0, 3, 2, false, true, false)]
        [TestCase(1, 1, 0, 0, true, false, false)]
        [TestCase(1, 1, 0, 0, true, true, false)]
        [TestCase(1, 1, 0, 0, false, false, false)]
        [TestCase(1, 1, 0, 0, false, true, false)]
        [TestCase(1, 1, 0, 1, true, false, false)]
        [TestCase(1, 1, 0, 1, true, true, false)]
        [TestCase(1, 1, 0, 1, false, false, false)]
        [TestCase(1, 1, 0, 1, false, true, false)]
        [TestCase(1, 1, 0, 2, true, false, false)]
        [TestCase(1, 1, 0, 2, true, true, false)]
        [TestCase(1, 1, 0, 2, false, false, false)]
        [TestCase(1, 1, 0, 2, false, true, false)]
        [TestCase(1, 1, 1, 0, true, false, false)]
        [TestCase(1, 1, 1, 0, true, true, false)]
        [TestCase(1, 1, 1, 0, false, false, false)]
        [TestCase(1, 1, 1, 0, false, true, false)]
        [TestCase(1, 1, 2, 0, true, false, false)]
        [TestCase(1, 1, 2, 0, true, true, false)]
        [TestCase(1, 1, 2, 0, false, false, false)]
        [TestCase(1, 1, 2, 0, false, true, false)]
        [TestCase(1, 2, 0, 0, true, false, false)]
        [TestCase(1, 2, 0, 0, true, true, false)]
        [TestCase(1, 2, 0, 0, false, false, false)]
        [TestCase(1, 2, 0, 0, false, true, false)]
        [TestCase(1, 2, 0, 1, true, false, false)]
        [TestCase(1, 2, 0, 1, true, true, false)]
        [TestCase(1, 2, 0, 1, false, false, false)]
        [TestCase(1, 2, 0, 1, false, true, false)]
        [TestCase(1, 2, 0, 2, true, false, false)]
        [TestCase(1, 2, 0, 2, true, true, false)]
        [TestCase(1, 2, 0, 2, false, false, false)]
        [TestCase(1, 2, 0, 2, false, true, false)]
        [TestCase(1, 2, 0, 3, true, false, false)]
        [TestCase(1, 2, 0, 3, true, true, false)]
        [TestCase(1, 2, 0, 3, false, false, false)]
        [TestCase(1, 2, 0, 3, false, true, false)]
        [TestCase(1, 2, 1, 0, true, false, false)]
        [TestCase(1, 2, 1, 0, true, true, false)]
        [TestCase(1, 2, 1, 0, false, false, false)]
        [TestCase(1, 2, 1, 0, false, true, false)]
        [TestCase(1, 2, 2, 0, true, false, false)]
        [TestCase(1, 2, 2, 0, true, true, false)]
        [TestCase(1, 2, 2, 0, false, false, false)]
        [TestCase(1, 2, 2, 0, false, true, false)]
        [TestCase(1, 2, 3, 0, true, false, false)]
        [TestCase(1, 2, 3, 0, true, true, false)]
        [TestCase(1, 2, 3, 0, false, false, false)]
        [TestCase(1, 2, 3, 0, false, true, false)]
        [TestCase(1, 3, 0, 2, true, false, false)]
        [TestCase(1, 3, 0, 2, true, true, false)]
        [TestCase(1, 3, 0, 2, false, false, false)]
        [TestCase(1, 3, 0, 2, false, true, false)]
        [TestCase(1, 3, 2, 0, true, false, false)]
        [TestCase(1, 3, 2, 0, true, true, false)]
        [TestCase(1, 3, 2, 0, false, false, false)]
        [TestCase(1, 3, 2, 0, false, true, false)]
        [TestCase(2, 0, 0, 1, true, false, false)]
        [TestCase(2, 0, 0, 1, true, true, false)]
        [TestCase(2, 0, 0, 1, false, false, false)]
        [TestCase(2, 0, 0, 1, false, true, false)]
        [TestCase(2, 0, 1, 0, true, false, false)]
        [TestCase(2, 0, 1, 0, true, true, false)]
        [TestCase(2, 0, 1, 0, false, false, false)]
        [TestCase(2, 0, 1, 0, false, true, false)]
        [TestCase(2, 0, 1, 1, true, false, false)]
        [TestCase(2, 0, 1, 1, true, true, false)]
        [TestCase(2, 0, 1, 1, false, false, false)]
        [TestCase(2, 0, 1, 1, false, true, false)]
        [TestCase(2, 0, 1, 2, true, false, false)]
        [TestCase(2, 0, 1, 2, true, true, false)]
        [TestCase(2, 0, 1, 2, false, false, false)]
        [TestCase(2, 0, 1, 2, false, true, false)]
        [TestCase(2, 0, 1, 3, true, false, false)]
        [TestCase(2, 0, 1, 3, true, true, false)]
        [TestCase(2, 0, 1, 3, false, false, false)]
        [TestCase(2, 0, 1, 3, false, true, false)]
        [TestCase(2, 0, 2, 1, true, false, false)]
        [TestCase(2, 0, 2, 1, true, true, false)]
        [TestCase(2, 0, 2, 1, false, false, false)]
        [TestCase(2, 0, 2, 1, false, true, false)]
        [TestCase(2, 0, 3, 1, true, false, false)]
        [TestCase(2, 0, 3, 1, true, true, false)]
        [TestCase(2, 0, 3, 1, false, false, false)]
        [TestCase(2, 0, 3, 1, false, true, false)]
        [TestCase(2, 1, 0, 0, true, false, false)]
        [TestCase(2, 1, 0, 0, true, true, false)]
        [TestCase(2, 1, 0, 0, false, false, false)]
        [TestCase(2, 1, 0, 0, false, true, false)]
        [TestCase(2, 1, 0, 1, true, false, false)]
        [TestCase(2, 1, 0, 1, true, true, false)]
        [TestCase(2, 1, 0, 1, false, false, false)]
        [TestCase(2, 1, 0, 1, false, true, false)]
        [TestCase(2, 1, 0, 2, true, false, false)]
        [TestCase(2, 1, 0, 2, true, true, false)]
        [TestCase(2, 1, 0, 2, false, false, false)]
        [TestCase(2, 1, 0, 2, false, true, false)]
        [TestCase(2, 1, 0, 3, true, false, false)]
        [TestCase(2, 1, 0, 3, true, true, false)]
        [TestCase(2, 1, 0, 3, false, false, false)]
        [TestCase(2, 1, 0, 3, false, true, false)]
        [TestCase(2, 1, 1, 0, true, false, false)]
        [TestCase(2, 1, 1, 0, true, true, false)]
        [TestCase(2, 1, 1, 0, false, false, false)]
        [TestCase(2, 1, 1, 0, false, true, false)]
        [TestCase(2, 1, 2, 0, true, false, false)]
        [TestCase(2, 1, 2, 0, true, true, false)]
        [TestCase(2, 1, 2, 0, false, false, false)]
        [TestCase(2, 1, 2, 0, false, true, false)]
        [TestCase(2, 1, 3, 0, true, false, false)]
        [TestCase(2, 1, 3, 0, true, true, false)]
        [TestCase(2, 1, 3, 0, false, false, false)]
        [TestCase(2, 1, 3, 0, false, true, false)]
        [TestCase(2, 2, 0, 1, true, false, false)]
        [TestCase(2, 2, 0, 1, true, true, false)]
        [TestCase(2, 2, 0, 1, false, false, false)]
        [TestCase(2, 2, 0, 1, false, true, false)]
        [TestCase(2, 2, 1, 0, true, false, false)]
        [TestCase(2, 2, 1, 0, true, true, false)]
        [TestCase(2, 2, 1, 0, false, false, false)]
        [TestCase(2, 2, 1, 0, false, true, false)]
        [TestCase(2, 3, 0, 1, true, false, false)]
        [TestCase(2, 3, 0, 1, true, true, false)]
        [TestCase(2, 3, 0, 1, false, false, false)]
        [TestCase(2, 3, 0, 1, false, true, false)]
        [TestCase(2, 3, 1, 0, true, false, false)]
        [TestCase(2, 3, 1, 0, true, true, false)]
        [TestCase(2, 3, 1, 0, false, false, false)]
        [TestCase(2, 3, 1, 0, false, true, false)]
        [TestCase(3, 0, 1, 2, true, false, false)]
        [TestCase(3, 0, 1, 2, true, true, false)]
        [TestCase(3, 0, 1, 2, false, false, false)]
        [TestCase(3, 0, 1, 2, false, true, false)]
        [TestCase(3, 0, 2, 1, true, false, false)]
        [TestCase(3, 0, 2, 1, true, true, false)]
        [TestCase(3, 0, 2, 1, false, false, false)]
        [TestCase(3, 0, 2, 1, false, true, false)]
        [TestCase(3, 1, 0, 2, true, false, false)]
        [TestCase(3, 1, 0, 2, true, true, false)]
        [TestCase(3, 1, 0, 2, false, false, false)]
        [TestCase(3, 1, 0, 2, false, true, false)]
        [TestCase(3, 1, 2, 0, true, false, false)]
        [TestCase(3, 1, 2, 0, true, true, false)]
        [TestCase(3, 1, 2, 0, false, false, false)]
        [TestCase(3, 1, 2, 0, false, true, false)]
        [TestCase(3, 2, 0, 1, true, false, false)]
        [TestCase(3, 2, 0, 1, true, true, false)]
        [TestCase(3, 2, 0, 1, false, false, false)]
        [TestCase(3, 2, 0, 1, false, true, false)]
        [TestCase(3, 2, 1, 0, true, false, false)]
        [TestCase(3, 2, 1, 0, true, true, false)]
        [TestCase(3, 2, 1, 0, false, false, false)]
        [TestCase(3, 2, 1, 0, false, true, false)]
        public void Contains_MatchesExpectationForRanges(int aStart, int aEnd, int bStart, int bEnd, bool startInclusive, bool endInclusive, bool expected)
        {
            var rangeA = new NumberRange { Start = aStart, End = aEnd };
            var rangeB = new NumberRange { Start = bStart, End = bEnd };

            var actual = rangeA.Contains(rangeB, startInclusive: startInclusive, endInclusive: endInclusive);

            Assert.That(actual, Is.EqualTo(expected));
        }

        private class NumberRange : IRange<int>
        {
            public int Start { get; set; }

            public int End { get; set; }
        }
    }
}
