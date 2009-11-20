using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpCash
{
    public abstract class RecurrenceBase
    {
        protected DateTime startDate
        {
            get;
            private set;
        }

        protected int multiplier
        {
            get;
            private set;
        }

        public RecurrenceBase(DateTime startDate, int multiplier)
        {
            this.startDate = startDate.Date;
            this.multiplier = multiplier;
        }

        public abstract void Reset();
        public abstract DateTime GetNextOccurence();
    }
}
