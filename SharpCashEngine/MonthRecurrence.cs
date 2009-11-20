using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpCash
{
    public abstract class MonthRecurrence : RecurrenceBase
    {
        private int originalInstance = 0;
        private int instance = 0;
        private DateTime startMonth;

        public MonthRecurrence(DateTime startDate, DateTime endDate, int multiplier, DateTime? lastOccurence)
            : base(startDate, endDate, multiplier)
        {
            var month = new DateTime(startDate.Year, startDate.Month, 1);
            var first = GetOccurenceInMonth(month);
            if (first.HasValue && first < startDate)
            {
                month = month.AddMonths(1);
            }
            this.startMonth = month;

            base.LoadToLastOccurence(lastOccurence);
        }

        protected override void SaveState()
        {
            this.originalInstance = this.instance;
        }

        public override void Reset()
        {
            this.instance = this.originalInstance;
        }

        public override DateTime? GetNextOccurence()
        {
            var next = this.GetOccurenceInMonth(this.startMonth.AddMonths(this.instance * this.multiplier));
            if (next > this.endDate)
            {
                return null;
            }
            else
            {
                this.instance++;
                return next;
            }
        }

        public abstract DateTime? GetOccurenceInMonth(DateTime monthOf);
    }
}
