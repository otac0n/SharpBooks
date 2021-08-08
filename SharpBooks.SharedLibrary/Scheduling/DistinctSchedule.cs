// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace SharpBooks.Scheduling
{
    using System;
    using System.Collections.Generic;

    public sealed class DistinctSchedule : ScheduleBase
    {
        public DistinctSchedule(ISchedule baseSchedule)
        {
            if (baseSchedule == null)
            {
                throw new ArgumentNullException("baseSchedule");
            }

            this.BaseSchedule = baseSchedule;
        }

        private ISchedule BaseSchedule
        {
            get;
            set;
        }

        public override IEnumerable<DateTime> YieldAllInstances()
        {
            var previousDate = (DateTime?)null;

            foreach (var d in this.BaseSchedule.YieldAllInstances())
            {
                if (d == previousDate)
                {
                    continue;
                }

                yield return d;
                previousDate = d;
            }
        }
    }
}
