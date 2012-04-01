//-----------------------------------------------------------------------
// <copyright file="AccountSelectedEventArgs.cs" company="(none)">
//  Copyright © 2012 John Gietzen. All rights reserved.
// </copyright>
// <author>John Gietzen</author>
//-----------------------------------------------------------------------

namespace SharpBooks.Plugins
{
    using System;

    public class AccountSelectedEventArgs : EventArgs
    {
        public Guid AccountId { get; set; }
    }
}
