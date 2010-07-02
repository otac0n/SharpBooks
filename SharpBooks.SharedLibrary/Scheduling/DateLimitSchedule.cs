﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpBooks.Scheduling
{
    public sealed class DateLimitSchedule : ISchedule
    {
        private ISchedule BaseSchedule
        {
            get;
            set;
        }

        private DateTime OnOrBefore
        {
            get;
            set;
        }

        public DateLimitSchedule(ISchedule baseSchedule, DateTime onOrBefore)
        {
            if (baseSchedule == null)
            {
                throw new ArgumentNullException("baseSchedule");
            }

            this.BaseSchedule = baseSchedule;
            this.OnOrBefore = onOrBefore;
        }

        public IEnumerable<DateTime> YieldAllInstances()
        {
            foreach (var d in this.BaseSchedule.YieldAllInstances())
            {
                if (d > this.OnOrBefore)
                {
                    yield break;
                }
                else
                {
                    yield return d;
                }
            }
        }

        public DateTime? GetInstance(int index)
        {
            if (index < 0)
            {
                throw new ArgumentOutOfRangeException("index");
            }

            return this.YieldAllInstances().Skip(index).Select(d => (DateTime?)d).First();
        }
    }
}
