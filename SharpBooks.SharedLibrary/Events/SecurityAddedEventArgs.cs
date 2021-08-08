// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace SharpBooks.Events
{
    using System;

    public class SecurityAddedEventArgs : EventArgs
    {
        public SecurityAddedEventArgs(Security security)
        {
            if (security == null)
            {
                throw new ArgumentNullException(nameof(security));
            }

            this.Security = security;
        }

        public Security Security { get; }
    }
}
