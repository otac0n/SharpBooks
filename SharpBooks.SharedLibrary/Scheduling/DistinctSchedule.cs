//-----------------------------------------------------------------------
// <copyright file="DistinctSchedule.cs" company="(none)">
//  Copyright © 2010 John Gietzen. All rights reserved.
// </copyright>
// <author>John Gietzen</author>
//-----------------------------------------------------------------------

namespace SharpBooks.Scheduling
{
    using System;
    using System.Collections.Generic;

    public sealed class DistinctSchedule : ScheduleBase
    {
        public DistinctSchedule(ISchedule baseSchedule)
        {
            if (baseSchedule == null)
            {
                throw new ArgumentNullException("baseSchedule");
            }

            this.BaseSchedule = baseSchedule;
        }

        private ISchedule BaseSchedule
        {
            get;
            set;
        }

        public override IEnumerable<DateTime> YieldAllInstances()
        {
            var previousDate = (DateTime?)null;

            foreach (var d in this.BaseSchedule.YieldAllInstances())
            {
                if (d == previousDate)
                {
                    continue;
                }

                yield return d;
                previousDate = d;
            }
        }
    }
}
