// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace SharpBooks.Scheduling
{
    using System;
    using System.Collections.Generic;

    public sealed class DateLimitSchedule : ScheduleBase
    {
        public DateLimitSchedule(ISchedule baseSchedule, DateTime onOrBefore)
        {
            this.BaseSchedule = baseSchedule ?? throw new ArgumentNullException(nameof(baseSchedule));
            this.OnOrBefore = onOrBefore;
        }

        private ISchedule BaseSchedule
        {
            get;
            set;
        }

        private DateTime OnOrBefore
        {
            get;
            set;
        }

        public override IEnumerable<DateTime> YieldAllInstances()
        {
            foreach (var d in this.BaseSchedule.YieldAllInstances())
            {
                if (d > this.OnOrBefore)
                {
                    yield break;
                }
                else
                {
                    yield return d;
                }
            }
        }
    }
}
