// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace SharpBooks.Events
{
    using System;

    public class SecurityAddedEventArgs : EventArgs
    {
        public SecurityAddedEventArgs(Security security)
        {
            this.Security = security ?? throw new ArgumentNullException(nameof(security));
        }

        public Security Security { get; }
    }
}
