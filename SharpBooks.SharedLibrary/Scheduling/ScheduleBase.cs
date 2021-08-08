//-----------------------------------------------------------------------
// <copyright file="ScheduleBase.cs" company="(none)">
//  Copyright © 2010 John Gietzen. All rights reserved.
// </copyright>
// <author>John Gietzen</author>
//-----------------------------------------------------------------------

namespace SharpBooks.Scheduling
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public abstract class ScheduleBase : ISchedule
    {
        public virtual DateTime? GetInstance(int index)
        {
            if (index < 0)
            {
                throw new ArgumentOutOfRangeException("index");
            }

            return this.YieldAllInstances().Skip(index).Select(d => (DateTime?)d).FirstOrDefault();
        }

        public abstract IEnumerable<DateTime> YieldAllInstances();
    }
}
