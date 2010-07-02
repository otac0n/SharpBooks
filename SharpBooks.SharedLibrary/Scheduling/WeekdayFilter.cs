using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpBooks.Scheduling
{
    public sealed class WeekdayFilter : ISchedule
    {
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

        public WeekdayFilter(ISchedule baseSchedule, IEnumerable<DayOfWeek> allowedWeekdays)
        {
            if (baseSchedule == null)
            {
                throw new ArgumentNullException("baseSchedule");
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

        public IEnumerable<DateTime> YieldAllInstances()
        {
            foreach (var d in this.BaseSchedule.YieldAllInstances())
            {
                if (this.Weekdays[d.DayOfWeek])
                {
                    yield return d;
                }
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
