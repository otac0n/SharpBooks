using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpCash
{
    public class WeekRecurrence : RecurrenceBase
    {
        private int occurence = 0;

        public WeekRecurrence(DateTime startDate, int multiplier)
            : base(startDate, multiplier)
        {
        }

        public override void Reset()
        {
            this.occurence = 0;
        }

        public override DateTime GetNextOccurence()
        {
            return this.startDate.AddDays(7 * this.multiplier * (this.occurence++));
        }
    }
}
