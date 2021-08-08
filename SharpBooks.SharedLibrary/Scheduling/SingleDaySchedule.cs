// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace SharpBooks.Scheduling
{
    using System;
    using System.Collections.Generic;

    public sealed class SingleDaySchedule : ScheduleBase
    {
        public SingleDaySchedule(DateTime dateTime)
        {
            this.DateTime = dateTime;
        }

        private DateTime DateTime
        {
            get;
            set;
        }

        public override IEnumerable<DateTime> YieldAllInstances()
        {
            yield return this.DateTime;
        }
    }
}
