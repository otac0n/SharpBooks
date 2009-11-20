using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpCash
{
    public class EndOfMonthRecurrence : MonthRecurrence
    {
        public EndOfMonthRecurrence(DateTime startDate, DateTime endDate, int multiplier, DateTime? lastOccurence)
            : base(startDate, endDate, multiplier, lastOccurence)
        {
        }

        public override DateTime? GetOccurenceInMonth(DateTime monthOf)
        {
            var day = DateTime.DaysInMonth(monthOf.Year, monthOf.Month);

            return new DateTime(monthOf.Year, monthOf.Month, day);
        }
    }
}
