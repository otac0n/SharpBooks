// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace SharpBooks.Scheduling
{
    using System;
    using System.Collections.Generic;

    public sealed class WeekendAdjustSchedule : ScheduleBase
    {
        public WeekendAdjustSchedule(ISchedule baseSchedule, WeekdayAdjustmentDirection saturdayAdjustment, WeekdayAdjustmentDirection sundayAdjustment)
        {
            this.BaseSchedule = baseSchedule ?? throw new ArgumentNullException(nameof(baseSchedule));
            this.SaturdayAdjustment = saturdayAdjustment;
            this.SundayAdjustment = sundayAdjustment;
        }

        public enum WeekdayAdjustmentDirection
        {
            NoChange,
            NextWeekday,
            PreviousWeekday,
        }

        private ISchedule BaseSchedule
        {
            get;
            set;
        }

        private WeekdayAdjustmentDirection SaturdayAdjustment
        {
            get;
            set;
        }

        private WeekdayAdjustmentDirection SundayAdjustment
        {
            get;
            set;
        }

        public override IEnumerable<DateTime> YieldAllInstances()
        {
            foreach (var d in this.BaseSchedule.YieldAllInstances())
            {
                if (d.DayOfWeek == DayOfWeek.Saturday)
                {
                    if (this.SaturdayAdjustment == WeekdayAdjustmentDirection.PreviousWeekday)
                    {
                        yield return d.AddDays(-1);
                    }
                    else if (this.SaturdayAdjustment == WeekdayAdjustmentDirection.NextWeekday)
                    {
                        yield return d.AddDays(2);
                    }
                    else
                    {
                        yield return d;
                    }
                }
                else if (d.DayOfWeek == DayOfWeek.Sunday)
                {
                    if (this.SundayAdjustment == WeekdayAdjustmentDirection.PreviousWeekday)
                    {
                        yield return d.AddDays(-2);
                    }
                    else if (this.SundayAdjustment == WeekdayAdjustmentDirection.NextWeekday)
                    {
                        yield return d.AddDays(1);
                    }
                    else
                    {
                        yield return d;
                    }
                }
                else
                {
                    yield return d;
                }
            }
        }
    }
}
