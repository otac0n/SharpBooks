// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace SharpBooks.Plugins
{
    using System;

    public class AccountSelectedEventArgs : EventArgs
    {
        public Guid AccountId { get; set; }
    }
}
