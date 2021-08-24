// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace GnuCashIntegration.Scheduling
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Schedule
    {
        private List<RecurrenceBase> recurrences;

        public Schedule(string name, DateTime startDate, DateTime? endDate, DateTime? lastOccurence, int? totalOccurences, int? remainingOccurences, IEnumerable<RecurrenceBase> recurrences)
        {
            this.Name = name;
            this.StartDate = startDate;
            this.EndDate = endDate;
            this.LastOccurence = lastOccurence;
            this.TotalOccurences = (totalOccurences.HasValue && totalOccurences.Value == 0) ? (int?)null : totalOccurences;
            this.RemainingOccurences = remainingOccurences;

            this.recurrences = new List<RecurrenceBase>(recurrences);
        }

        public DateTime? EndDate { get; private set; }

        public DateTime? LastOccurence { get; private set; }

        public string Name { get; private set; }

        public int? RemainingOccurences { get; private set; }

        public DateTime StartDate { get; private set; }

        public int? TotalOccurences { get; private set; }

        public IEnumerable<KeyValuePair<int, DateTime>> GetDatesInRange(DateTime startDate, DateTime endDate)
        {
            endDate = this.EndDate.HasValue ? (this.EndDate.Value < endDate ? this.EndDate.Value : endDate) : endDate;

            List<DateTime> dates = new List<DateTime>();
            foreach (var r in this.recurrences)
            {
                dates.AddRange(GetDatesInRange(r, this.StartDate, endDate));
            }

            startDate = this.StartDate > startDate ? this.StartDate : startDate;
            startDate = this.LastOccurence.HasValue ? (this.LastOccurence.Value > startDate ? this.LastOccurence.Value : startDate) : startDate;

            dates = dates.Distinct().ToList();

            int i = 1;
            foreach (var d in dates)
            {
                if (d >= startDate && d <= endDate && (!this.LastOccurence.HasValue || d > this.LastOccurence.Value))
                {
                    yield return new KeyValuePair<int, DateTime>(i, d);
                }

                i++;

                if (this.TotalOccurences.HasValue && this.TotalOccurences < i)
                {
                    break;
                }
            }

            yield break;
        }

        private IEnumerable<DateTime> GetDatesInRange(RecurrenceBase recurrence, DateTime startDate, DateTime endDate)
        {
            recurrence.Reset();

            DateTime date;

            do
            {
                date = recurrence.GetNextOccurence();
                if (date >= startDate && date <= endDate)
                {
                    yield return date;
                }
            } while (date <= endDate);

            yield break;
        }
    }
}
