// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace SharpBooks.Tests.ScheduleTests
{
    using System;
    using System.Linq;
    using NUnit.Framework;
    using SharpBooks.Scheduling;

    [TestFixture]
    public class SingleDayTests
    {
        [Datapoints]
        private readonly int[] intDatapoints = new[] { 0, 1, -1, 3, -3, 5, -5, 7, -7, 10, -10, 100, -100, 1000, -1000, int.MaxValue, int.MinValue };

        [Theory]
        public void GetInstance_WhenCalledWithNegativeIndex_ThrowsException(int index)
        {
            Assume.That(index, Is.LessThan(0));
            var single = new SingleDaySchedule(DateTime.MinValue);

            Assert.That(() => single.GetInstance(index), Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [Theory]
        public void GetInstance_WhenCalledWithPositiveNonZeroIndex_ReturnsNull(int index)
        {
            Assume.That(index, Is.GreaterThan(0));
            var single = new SingleDaySchedule(DateTime.MinValue);

            var instance = single.GetInstance(index);

            Assert.That(instance, Is.Null);
        }

        [Test]
        public void GetInstance_WhenCalledWithZeroIndex_DoesNotReturnNull()
        {
            var single = new SingleDaySchedule(DateTime.MinValue);

            var instance = single.GetInstance(0);

            Assert.That(instance, Is.Not.Null);
        }

        [Test]
        public void GetInstance_WhenCalledWithZeroIndex_MatchesFirstYieldedInstance()
        {
            var single = new SingleDaySchedule(DateTime.MinValue);

            var first = single.YieldAllInstances().First();
            var zero = single.GetInstance(0);

            Assert.That(zero, Is.EqualTo(first));
        }

        [Test]
        public void YieldAllInstances_WhenCalled_ReturnsASingleInstance()
        {
            var single = new SingleDaySchedule(DateTime.MinValue);

            var count = single.YieldAllInstances().Count();

            Assert.That(count, Is.EqualTo(1));
        }
    }
}
