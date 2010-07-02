namespace SharpBooks.Scheduling
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public sealed class InstanceLimitSchedule : ISchedule
    {
        private ISchedule BaseSchedule
        {
            get;
            set;
        }

        private int Count
        {
            get;
            set;
        }

        public InstanceLimitSchedule(ISchedule baseSchedule, int count)
        {
            if (baseSchedule == null)
            {
                throw new ArgumentNullException("baseSchedule");
            }

            if (count < 0)
            {
                throw new ArgumentOutOfRangeException("count");
            }

            this.BaseSchedule = baseSchedule;
            this.Count = count;
        }

        public IEnumerable<DateTime> YieldAllInstances()
        {
            return this.BaseSchedule.YieldAllInstances().Take(this.Count);
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
