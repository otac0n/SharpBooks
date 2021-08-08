// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace SharpBooks.Scheduling
{
    using System;
    using System.Collections.Generic;

    public sealed class DistinctSchedule : ScheduleBase
    {
        public DistinctSchedule(ISchedule baseSchedule)
        {
            this.BaseSchedule = baseSchedule ?? throw new ArgumentNullException(nameof(baseSchedule));
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
