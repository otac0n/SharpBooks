// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace SharpBooks.Events
{
    using System;

    public class SecurityRemovedEventArgs : EventArgs
    {
        private readonly Security security;

        public SecurityRemovedEventArgs(Security security)
        {
            if (security == null)
            {
                throw new ArgumentNullException(nameof(security));
            }

            this.security = security;
        }

        public Security Security
        {
            get
            {
                return this.security;
            }
        }
    }
}
