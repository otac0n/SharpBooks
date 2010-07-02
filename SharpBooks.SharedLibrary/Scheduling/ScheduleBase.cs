using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpBooks.Scheduling
{
    public abstract class ScheduleBase : ISchedule
    {
        public abstract IEnumerable<DateTime> YieldAllInstances();

        public virtual DateTime? GetInstance(int index)
        {
            if (index < 0)
            {
                throw new ArgumentOutOfRangeException("index");
            }

            return this.YieldAllInstances().Skip(index).Select(d => (DateTime?)d).First();
        }
    }
}
