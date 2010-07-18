//-----------------------------------------------------------------------
// <copyright file="InstanceLimitSchedule.cs" company="(none)">
//  Copyright © 2010 John Gietzen. All rights reserved.
// </copyright>
// <author>John Gietzen</author>
//-----------------------------------------------------------------------

namespace SharpBooks.Scheduling
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public sealed class InstanceLimitSchedule : ScheduleBase
    {
        public InstanceLimitSchedule(ISchedule baseSchedule, int count)
        {
            if (baseSchedule == null)
            {
                throw new ArgumentNullException("baseSchedule");
            }

            if (count < 0)
            {
                throw new ArgumentOutOfRangeException("count");
            }

            this.BaseSchedule = baseSchedule;
            this.Count = count;
        }

        private ISchedule BaseSchedule
        {
            get;
            set;
        }

        private int Count
        {
            get;
            set;
        }

        public override IEnumerable<DateTime> YieldAllInstances()
        {
            return this.BaseSchedule.YieldAllInstances().Take(this.Count);
        }
    }
}
