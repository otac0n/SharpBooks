// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace GnuCashIntegration.Scheduling
{
    using System;

    public class MonthRecurrence : RecurrenceBase
    {
        private int occurence = 0;

        public MonthRecurrence(DateTime startDate, int multiplier)
            : base(startDate, multiplier)
        {
        }

        public override DateTime GetNextOccurence()
        {
            return this.StartDate.AddMonths(this.Multiplier * this.occurence++);
        }

        public override void Reset()
        {
            this.occurence = 0;
        }
    }
}
