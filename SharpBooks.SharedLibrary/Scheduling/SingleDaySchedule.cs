//-----------------------------------------------------------------------
// <copyright file="SingleDaySchedule.cs" company="(none)">
//  Copyright © 2010 John Gietzen. All rights reserved.
// </copyright>
// <author>John Gietzen</author>
//-----------------------------------------------------------------------

namespace SharpBooks.Scheduling
{
    using System;
    using System.Collections.Generic;

    public sealed class SingleDaySchedule : ScheduleBase
    {
        public SingleDaySchedule(DateTime dateTime)
        {
            this.DateTime = dateTime;
        }

        private DateTime DateTime
        {
            get;
            set;
        }

        public override IEnumerable<DateTime> YieldAllInstances()
        {
            yield return this.DateTime;
        }
    }
}
