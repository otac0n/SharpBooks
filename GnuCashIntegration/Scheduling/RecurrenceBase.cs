// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace GnuCashIntegration.Scheduling
{
    using System;

    public abstract class RecurrenceBase
    {
        public RecurrenceBase(DateTime startDate, int multiplier)
        {
            this.StartDate = startDate.Date;
            this.Multiplier = multiplier;
        }

        protected int Multiplier { get; }

        protected DateTime StartDate { get; }

        public abstract DateTime GetNextOccurence();

        public abstract void Reset();
    }
}
