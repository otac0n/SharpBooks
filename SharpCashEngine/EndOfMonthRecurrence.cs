using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpCash
{
    public class EndOfMonthRecurrence : MonthRecurrence
    {
        private int occurence = 0;

        private DateTime startMonth
        {
            get;
            set;
        }

        public EndOfMonthRecurrence(DateTime startDate, int multiplier)
            : base(startDate, multiplier)
        {
            this.startMonth = new DateTime(this.startDate.Year, this.startDate.Month, 1);
        }

        public override void Reset()
        {
            this.occurence = 0;
        }

        public override DateTime GetNextOccurence()
        {
            var month = this.startMonth.AddMonths(this.occurence++);
            var day = DateTime.DaysInMonth(month.Year, month.Month);

            return new DateTime(month.Year, month.Month, day);
        }
    }
}
