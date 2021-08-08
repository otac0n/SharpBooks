// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace SharpBooks.Scheduling
{
    using System;
    using System.Collections.Generic;

    public class RepetitionSchedule : ScheduleBase
    {
        private static readonly Dictionary<DateUnit, Func<DateTime, int, DateTime>> Lookups = new Dictionary<DateUnit, Func<DateTime, int, DateTime>>()
        {
            { DateUnit.Seconds, (dt, inc) => dt.AddSeconds(inc) },
            { DateUnit.Minutes, (dt, inc) => dt.AddMinutes(inc) },
            { DateUnit.Hours,   (dt, inc) => dt.AddHours(inc) },
            { DateUnit.Days,    (dt, inc) => dt.AddDays(inc) },
            { DateUnit.Months,  (dt, inc) => dt.AddMonths(inc) },
            { DateUnit.Years,   (dt, inc) => dt.AddYears(inc) },
        };

        public RepetitionSchedule(ISchedule baseSchedule, DateUnit unit, int increment)
        {
            if (baseSchedule == null)
            {
                throw new ArgumentNullException(nameof(baseSchedule));
            }

            if (increment <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(increment));
            }

            this.BaseSchedule = baseSchedule;
            this.Unit = unit;
            this.Increment = increment;
        }

        private ISchedule BaseSchedule
        {
            get;
            set;
        }

        private int Increment
        {
            get;
            set;
        }

        private DateUnit Unit
        {
            get;
            set;
        }

        public override IEnumerable<DateTime> YieldAllInstances()
        {
            var lookup = Lookups[this.Unit];

            var enumerator = this.BaseSchedule.YieldAllInstances().GetEnumerator();

            try
            {
                if (!enumerator.MoveNext())
                {
                    yield break;
                }

                var items = new List<ScheduleIndex>()
                {
                    new ScheduleIndex
                    {
                        BaseDate = enumerator.Current,
                        CurrentOffset = 0,
                        CurrentDate = enumerator.Current,
                    }
                };

                var finished = !enumerator.MoveNext();

                while (true)
                {
                    if (!finished)
                    {
                        if (items[items.Count - 1].CurrentOffset != 0)
                        {
                            items.Add(
                                new ScheduleIndex
                                {
                                    BaseDate = enumerator.Current,
                                    CurrentOffset = 0,
                                    CurrentDate = enumerator.Current,
                                });

                            finished = !enumerator.MoveNext();
                        }
                    }

                    var minIndex = 0;
                    var minDate = items[minIndex].CurrentDate;

                    for (var i = 1; i < items.Count; i++)
                    {
                        if (items[i].CurrentDate < minDate)
                        {
                            minIndex = i;
                            minDate = items[minIndex].CurrentDate;
                        }
                    }

                    yield return minDate;

                    var hit = items[minIndex];
                    hit.CurrentOffset++;
                    hit.CurrentDate = lookup(hit.BaseDate, this.Increment * hit.CurrentOffset);
                }
            }
            finally
            {
                enumerator.Dispose();
            }
        }

        private class ScheduleIndex
        {
            public DateTime BaseDate { get; set; }

            public DateTime CurrentDate { get; set; }

            public int CurrentOffset { get; set; }
        }
    }
}
