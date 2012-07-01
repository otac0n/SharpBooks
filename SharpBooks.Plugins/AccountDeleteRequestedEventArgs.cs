﻿//-----------------------------------------------------------------------
// <copyright file="AccountDeleteRequestedEventArgs.cs" company="(none)">
//  Copyright © 2012 John Gietzen. All rights reserved.
// </copyright>
// <author>John Gietzen</author>
//-----------------------------------------------------------------------

namespace SharpBooks.Plugins
{
    using System;

    public class AccountDeleteRequestedEventArgs : EventArgs
    {
        public Guid AccountId { get; set; }
    }
}