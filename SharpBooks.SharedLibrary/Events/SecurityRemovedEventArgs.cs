//-----------------------------------------------------------------------
// <copyright file="SecurityRemovedEventArgs.cs" company="(none)">
//  Copyright © 2010 John Gietzen. All rights reserved.
// </copyright>
// <author>John Gietzen</author>
//-----------------------------------------------------------------------

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
                throw new ArgumentNullException("security");
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
