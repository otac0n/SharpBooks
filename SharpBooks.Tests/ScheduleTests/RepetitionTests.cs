// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace SharpBooks.Tests.ScheduleTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using NUnit.Framework;
    using SharpBooks.Scheduling;

    [TestFixture]
    public class RepetitionTests
    {
        [Datapoints]
        private int[] intDatapoints = new[] { 0, 1, -1, 3, -3, 5, -5, 7, -7, 10, -10, 100, -100, 1000, -1000 };

        [Test]
        public void Constructor_WhenBaseScheduleIsNull_ThrowsException()
        {
            Assert.That(() => new RepetitionSchedule(null, DateUnit.Days, 1), Throws.InstanceOf<ArgumentNullException>());
        }

        [Theory]
        public void Constructor_WhenIncrementIsLessThanOrEqualToZero_ThrowsException(int index)
        {
            Assume.That(index, Is.LessThanOrEqualTo(0));

            Assert.That(() => new RepetitionSchedule(new SingleDaySchedule(DateTime.MinValue), DateUnit.Days, index), Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void GetInstance_WhenCalledOnNestedRepetitions_ReturnsExpectedValues()
        {
            var rep = new RepetitionSchedule(
                new RepetitionSchedule(
                    new SingleDaySchedule(DateTime.MinValue),
                    DateUnit.Hours,
                    12),
                DateUnit.Days,
                1);

            var expected = new List<DateTime>()
                {
                    DateTime.MinValue.AddDays(1 * 0).AddHours(12 * 0),

                    DateTime.MinValue.AddDays(1 * 0).AddHours(12 * 1),

                    DateTime.MinValue.AddDays(1 * 0).AddHours(12 * 2),
                    DateTime.MinValue.AddDays(1 * 1).AddHours(12 * 0),

                    DateTime.MinValue.AddDays(1 * 0).AddHours(12 * 3),
                    DateTime.MinValue.AddDays(1 * 1).AddHours(12 * 1),

                    DateTime.MinValue.AddDays(1 * 0).AddHours(12 * 4),
                    DateTime.MinValue.AddDays(1 * 1).AddHours(12 * 2),
                    DateTime.MinValue.AddDays(1 * 2).AddHours(12 * 0),

                    DateTime.MinValue.AddDays(1 * 0).AddHours(12 * 5),
                    DateTime.MinValue.AddDays(1 * 1).AddHours(12 * 3),
                    DateTime.MinValue.AddDays(1 * 2).AddHours(12 * 1),
                };

            var mismatches = from i in Enumerable.Range(0, expected.Count)
                             where expected[i] != rep.GetInstance(i)
                             select i;

            Assert.That(mismatches.Any(), Is.False);
        }

        [Theory]
        public void GetInstance_WhenCalledWithNegativeIndex_ThrowsException(int index)
        {
            Assume.That(index, Is.LessThan(0));
            var rep = new RepetitionSchedule(
                new SingleDaySchedule(DateTime.MinValue),
                DateUnit.Days,
                1);

            Assert.That(() => rep.GetInstance(index), Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [Theory]
        public void GetInstance_WhenCalledWithPositiveOrZeroIndex_DoesNotReturnNull(int index)
        {
            Assume.That(index, Is.GreaterThan(0));
            var rep = new RepetitionSchedule(
                new SingleDaySchedule(DateTime.MinValue),
                DateUnit.Days,
                1);

            var instance = rep.GetInstance(index);

            Assert.That(instance, Is.Not.Null);
        }

        [Test]
        public void GetInstance_WhenCalledWithZeroIndex_MatchesFirstYieldedInstance()
        {
            var rep = new RepetitionSchedule(
                new SingleDaySchedule(DateTime.MinValue),
                DateUnit.Days,
                1);

            var first = rep.YieldAllInstances().First();
            var zero = rep.GetInstance(0);

            Assert.That(zero, Is.EqualTo(first));
        }

        [Test]
        public void YieldAllInstances_WhenCalledOnNestedRepetitions_ReturnsDatesInOrder()
        {
            var rep = new RepetitionSchedule(
                new RepetitionSchedule(
                    new SingleDaySchedule(DateTime.MinValue),
                    DateUnit.Hours,
                    1),
                DateUnit.Days,
                1);

            var previousDate = DateTime.MinValue;
            foreach (var d in rep.YieldAllInstances().Take(1000))
            {
                Assert.That(d, Is.GreaterThanOrEqualTo(previousDate));
                previousDate = d;
            }
        }
    }
}
