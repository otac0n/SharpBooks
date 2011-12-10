//-----------------------------------------------------------------------
// <copyright file="AccountType.cs" company="(none)">
//  Copyright © 2010 John Gietzen. All rights reserved.
// </copyright>
// <author>John Gietzen</author>
//-----------------------------------------------------------------------

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
