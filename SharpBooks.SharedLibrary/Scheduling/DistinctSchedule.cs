using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpBooks.Scheduling
{
    public sealed class DistinctSchedule : ISchedule
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

        public IEnumerable<DateTime> YieldAllInstances()
        {
            var previousDate = (DateTime?)null;

            foreach (var d in this.BaseSchedule.YieldAllInstances())
            {
                if (d != previousDate)
                {
                    yield return d;
                }

                previousDate = d;
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
