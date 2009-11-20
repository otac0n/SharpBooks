using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpCash
{
    public class MonthDayRecurrence : MonthRecurrence
    {
        public MonthDayRecurrence(DateTime startDate, DateTime endDate, int multiplier, DateTime? lastOccurence)
            : base(startDate, endDate, multiplier, lastOccurence)
        {
        }

        public override DateTime? GetOccurenceInMonth(DateTime monthOf)
        {
            var day = Math.Min(DateTime.DaysInMonth(monthOf.Year, monthOf.Month), this.startDate.Day);

            return new DateTime(monthOf.Year, monthOf.Month, day);
        }
    }
}
