//-----------------------------------------------------------------------
// <copyright file="EventProxy.cs" company="(none)">
//  Copyright © 2010 John Gietzen. All rights reserved.
// </copyright>
// <author>John Gietzen</author>
//-----------------------------------------------------------------------

namespace SharpBooks.Plugins
{
    using System;

    public sealed class EventProxy
    {
        private readonly EventHandler<AccountSelectedEventArgs> accountSelected;

        public EventProxy(EventHandler<AccountSelectedEventArgs> accountSelected)
        {
            this.accountSelected = accountSelected;
        }

        public void RaiseAccountSelected(object sender, AccountSelectedEventArgs args)
        {
            if (this.accountSelected != null)
            {
                this.accountSelected(sender, args);
            }
        }
    }
}
