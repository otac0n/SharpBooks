//-----------------------------------------------------------------------
// <copyright file="DateLimitSchedule.cs" company="(none)">
//  Copyright © 2010 John Gietzen. All rights reserved.
// </copyright>
// <author>John Gietzen</author>
//-----------------------------------------------------------------------

namespace SharpBooks.Scheduling
{
    using System;
    using System.Collections.Generic;

    public sealed class DateLimitSchedule : ScheduleBase
    {
        public DateLimitSchedule(ISchedule baseSchedule, DateTime onOrBefore)
        {
            if (baseSchedule == null)
            {
                throw new ArgumentNullException("baseSchedule");
            }

            this.BaseSchedule = baseSchedule;
            this.OnOrBefore = onOrBefore;
        }

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

        public override IEnumerable<DateTime> YieldAllInstances()
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
    }
}
