// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace GnuCashIntegration.Scheduling
{
    using System;

    public abstract class RecurrenceBase
    {
        protected DateTime startDate
        {
            get;
            private set;
        }

        protected int multiplier
        {
            get;
            private set;
        }

        public RecurrenceBase(DateTime startDate, int multiplier)
        {
            this.startDate = startDate.Date;
            this.multiplier = multiplier;
        }

        public abstract void Reset();
        public abstract DateTime GetNextOccurence();
    }
}
