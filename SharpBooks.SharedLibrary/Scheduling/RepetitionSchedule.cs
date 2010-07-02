using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpBooks.Scheduling
{
    public class RepetitionSchedule : ISchedule
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

        private ISchedule BaseSchedule
        {
            get;
            set;
        }

        private DateUnit Unit
        {
            get;
            set;
        }

        private int Increment
        {
            get;
            set;
        }

        public RepetitionSchedule(ISchedule baseSchedule, DateUnit unit, int increment)
        {
            if (baseSchedule == null)
            {
                throw new ArgumentNullException("baseSchedule");
            }

            if (increment <= 0)
            {
                throw new ArgumentOutOfRangeException("increment");
            }

            this.BaseSchedule = baseSchedule;
            this.Unit = unit;
            this.Increment = increment;
        }

        private class ScheduleIndex
        {
            public DateTime BaseDate { get; set; }
            public int CurrentOffset { get; set; }
            public DateTime CurrentDate { get; set; }
        }

        public IEnumerable<DateTime> YieldAllInstances()
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

                var atEnd = !enumerator.MoveNext();

                while (true)
                {
                    if (!atEnd)
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

                            atEnd = !enumerator.MoveNext();
                        }
                    }

                    var minIndex = 0;
                    var minDate = items[minIndex].CurrentDate;

                    for (int i = 1; i < items.Count; i++)
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

        public DateTime? GetInstance(int index)
        {
            if (index < 0)
            {
                throw new ArgumentOutOfRangeException("index");
            }

            return this.YieldAllInstances().Skip(index).Select(d => (DateTime?)d).First();
        }
    }
}
