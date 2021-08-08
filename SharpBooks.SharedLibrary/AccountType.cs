// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace SharpBooks
{
    /// <summary>
    /// Indicates the type of an account.
    /// </summary>
    public enum AccountType
    {
        /// <summary>
        /// Indicates an account that may have transaction splits assigned to it.
        /// </summary>
        Balance,

        /// <summary>
        /// Indicates an account that may not have transaction splits assigned to it.
        /// </summary>
        Grouping
    }
}
