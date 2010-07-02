using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpBooks.Scheduling
{
    public sealed class DistinctSchedule : ScheduleBase
    {
        private ISchedule BaseSchedule
        {
            get;
            set;
        }

        public DistinctSchedule(ISchedule baseSchedule)
        {
            if (baseSchedule == null)
            {
                throw new ArgumentNullException("baseSchedule");
            }

            this.BaseSchedule = baseSchedule;
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
