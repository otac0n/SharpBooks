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

        protected DateTime endDate
        {
            get;
            private set;
        }

        protected int multiplier
        {
            get;
            private set;
        }

        public RecurrenceBase(DateTime startDate, DateTime endDate, int multiplier)
        {
            this.startDate = startDate.Date;
            this.endDate = endDate.Date;
            this.multiplier = multiplier;
        }

        protected void LoadToLastOccurence(DateTime? lastOccurence)
        {
            if (lastOccurence.HasValue)
            {
                var last = lastOccurence.Value;

                var next = this.GetNextOccurence();
                while (next < last)
                {
                    next = this.GetNextOccurence();
                }

                if (next != last)
                {
                    throw new ArgumentException("The date passed as the last occurence was not an occurence in the sequence.");
                }
            }

            SaveState();
        }

        protected abstract void SaveState();
        public abstract void Reset();
        public abstract DateTime? GetNextOccurence();
    }
}
