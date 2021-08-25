// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace GnuCashIntegration.Scheduling
{
    using System;

    public class WeekRecurrence : RecurrenceBase
    {
        private int occurence = 0;

        public WeekRecurrence(DateTime startDate, int multiplier)
            : base(startDate, multiplier)
        {
        }

        public override DateTime GetNextOccurence()
        {
            return this.StartDate.AddDays(7 * this.Multiplier * this.occurence++);
        }

        public override void Reset()
        {
            this.occurence = 0;
        }
    }
}
