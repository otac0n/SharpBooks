// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace SharpBooks.Scheduling
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public sealed class WeekdayFilter : ScheduleBase
    {
        public WeekdayFilter(ISchedule baseSchedule, IEnumerable<DayOfWeek> allowedWeekdays)
        {
            if (baseSchedule == null)
            {
                throw new ArgumentNullException(nameof(baseSchedule));
            }

            var weekdays = (from DayOfWeek wk in Enum.GetValues(typeof(DayOfWeek))
                            select wk).ToDictionary(w => w, w => false);

            foreach (var wk in allowedWeekdays)
            {
                weekdays[wk] = true;
            }

            this.BaseSchedule = baseSchedule;
            this.Weekdays = weekdays;
        }

        private ISchedule BaseSchedule
        {
            get;
            set;
        }

        private Dictionary<DayOfWeek, bool> Weekdays
        {
            get;
            set;
        }

        public override IEnumerable<DateTime> YieldAllInstances()
        {
            foreach (var d in this.BaseSchedule.YieldAllInstances())
            {
                if (this.Weekdays[d.DayOfWeek])
                {
                    yield return d;
                }
            }
        }
    }
}
