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

        public override void Reset()
        {
            this.occurence = 0;
        }

        public override DateTime GetNextOccurence()
        {
            return this.startDate.AddMonths(this.multiplier * (this.occurence++));
        }
    }
}
