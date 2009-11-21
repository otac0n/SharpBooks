using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpCash
{
    public class Schedule
    {
        public string Name { get; private set; }
        public DateTime StartDate { get; private set; }
        public DateTime? EndDate { get; private set; }
        public DateTime? LastOccurence { get; private set; }
        public int? TotalOccurences { get; private set; }
        public int? RemainingOccurences { get; private set; }

        private List<RecurrenceBase> recurrences;

        public Schedule(string name, DateTime startDate, DateTime? endDate, DateTime? lastOccurence, int? totalOccurences, int? remainingOccurences, IEnumerable<RecurrenceBase> recurrences)
        {
            this.Name = name;
            this.StartDate = startDate;
            this.EndDate = endDate;
            this.LastOccurence = lastOccurence;
            this.TotalOccurences = totalOccurences;
            this.RemainingOccurences = remainingOccurences;

            this.recurrences = new List<RecurrenceBase>(recurrences);
        }

        public IEnumerable<DateTime> GetDatesInRange(DateTime startDate, DateTime endDate)
        {
            List<DateTime> dates = new List<DateTime>();
            foreach (var r in this.recurrences)
            {
                dates.AddRange(GetDatesInRange(r, this.StartDate, endDate));
            }

            foreach (var d in dates.Distinct())
            {
                if (d >= startDate && d <= endDate)
                {
                    yield return d;
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
