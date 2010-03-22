//-----------------------------------------------------------------------
// <copyright file="Account.cs" company="(none)">
//  Copyright (c) 2010 John Gietzen
// </copyright>
// <author>John Gietzen</author>
//-----------------------------------------------------------------------

namespace SharpBooks
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class Account
    {
        public Commodity Commodity
        {
            get;
            private set;
        }

        public Account ParentAccount
        {
            get;
            set;
        }
    }
}
