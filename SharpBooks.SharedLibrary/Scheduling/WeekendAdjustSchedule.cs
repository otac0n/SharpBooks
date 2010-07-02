﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpBooks.Scheduling
{
    public sealed class WeekendAdjustSchedule : ScheduleBase
    {
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

        private WeekdayAdjustmentDirection SundayAdjustment
        {
            get;
            set;
        }

        private WeekdayAdjustmentDirection SaturdayAdjustment
        {
            get;
            set;
        }

        public WeekendAdjustSchedule(ISchedule baseSchedule, WeekdayAdjustmentDirection saturdayAdjustment, WeekdayAdjustmentDirection sundayAdjustment)
        {
            if (baseSchedule == null)
            {
                throw new ArgumentNullException("baseSchedule");
            }

            this.BaseSchedule = baseSchedule;
            this.SaturdayAdjustment = saturdayAdjustment;
            this.SundayAdjustment = sundayAdjustment;
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
