using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpCash
{
    public class MonthRecurrence : MonthRecurrence
    {
        public MonthRecurrence(DateTime startDate, int multiplier)
            : base(startDate, multiplier)
        {
        }

        public override DateTime? GetOccurenceInMonth(DateTime monthOf)
        {
            var day = Math.Min(DateTime.DaysInMonth(monthOf.Year, monthOf.Month), this.startDate.Day);

            return new DateTime(monthOf.Year, monthOf.Month, day);
        }
    }
}
