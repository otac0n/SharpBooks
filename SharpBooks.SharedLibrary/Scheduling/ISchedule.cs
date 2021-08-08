//-----------------------------------------------------------------------
// <copyright file="ISchedule.cs" company="(none)">
//  Copyright © 2010 John Gietzen. All rights reserved.
// </copyright>
// <author>John Gietzen</author>
//-----------------------------------------------------------------------

namespace SharpBooks.Scheduling
{
    using System;
    using System.Collections.Generic;

    public interface ISchedule
    {
        DateTime? GetInstance(int index);

        IEnumerable<DateTime> YieldAllInstances();
    }
}
