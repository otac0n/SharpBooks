// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace GnuCashIntegration.Scheduling
{
    using System;

    public class EndOfMonthRecurrence : MonthRecurrence
    {
        private int occurence = 0;

        public EndOfMonthRecurrence(DateTime startDate, int multiplier)
            : base(startDate, multiplier)
        {
            this.StartMonth = new DateTime(this.StartDate.Year, this.StartDate.Month, 1);
        }

        private DateTime StartMonth { get; }

        public override DateTime GetNextOccurence()
        {
            var month = this.StartMonth.AddMonths(this.occurence++);
            var day = DateTime.DaysInMonth(month.Year, month.Month);

            return new DateTime(month.Year, month.Month, day);
        }

        public override void Reset()
        {
            this.occurence = 0;
        }
    }
}
