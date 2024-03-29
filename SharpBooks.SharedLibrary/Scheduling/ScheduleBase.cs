// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace SharpBooks.Scheduling
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public abstract class ScheduleBase : ISchedule
    {
        /// <inheritdoc/>
        public virtual DateTime? GetInstance(int index)
        {
            if (index < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            return this.YieldAllInstances().Skip(index).Select(d => (DateTime?)d).FirstOrDefault();
        }

        /// <inheritdoc/>
        public abstract IEnumerable<DateTime> YieldAllInstances();
    }
}
