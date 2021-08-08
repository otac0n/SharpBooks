// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace SharpBooks.Scheduling
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public sealed class InstanceLimitSchedule : ScheduleBase
    {
        public InstanceLimitSchedule(ISchedule baseSchedule, int count)
        {
            if (baseSchedule == null)
            {
                throw new ArgumentNullException("baseSchedule");
            }

            if (count < 0)
            {
                throw new ArgumentOutOfRangeException("count");
            }

            this.BaseSchedule = baseSchedule;
            this.Count = count;
        }

        private ISchedule BaseSchedule
        {
            get;
            set;
        }

        private int Count
        {
            get;
            set;
        }

        public override IEnumerable<DateTime> YieldAllInstances()
        {
            return this.BaseSchedule.YieldAllInstances().Take(this.Count);
        }
    }
}
