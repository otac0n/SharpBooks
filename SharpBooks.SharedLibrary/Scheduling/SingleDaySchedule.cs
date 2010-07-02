namespace SharpBooks.Scheduling
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public sealed class SingleDaySchedule : ScheduleBase
    {
        private DateTime DateTime
        {
            get;
            set;
        }

        public SingleDaySchedule(DateTime dateTime)
        {
            this.DateTime = dateTime;
        }

        public override IEnumerable<DateTime> YieldAllInstances()
        {
            yield return this.DateTime;
        }
    }
}
