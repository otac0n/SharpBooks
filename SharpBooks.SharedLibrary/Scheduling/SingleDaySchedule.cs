using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpBooks.Scheduling
{
    public sealed class SingleDaySchedule : ISchedule
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

        public IEnumerable<DateTime> YieldAllInstances()
        {
            yield return this.DateTime;
        }

        public DateTime? GetInstance(int index)
        {
            if (index < 0)
            {
                throw new ArgumentOutOfRangeException("index");
            }

            return this.YieldAllInstances().Skip(index).Select(d => (DateTime?)d).FirstOrDefault();
        }
    }
}
