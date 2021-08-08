// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

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
