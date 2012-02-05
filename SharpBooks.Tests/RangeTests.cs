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
        private int[] intDatapoints = new[] { int.MaxValue, -3, -2, -1, 0, 1, 2, 3, int.MinValue };

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

        private class NumberRange : IRange<int>
        {
            public int Start { get; set; }

            public int End { get; set; }
        }
    }
}
