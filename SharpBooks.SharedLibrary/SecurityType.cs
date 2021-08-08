// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace SharpBooks
{
    /// <summary>
    /// Defines the possible types of securities.
    /// </summary>
    public enum SecurityType
    {
        /// <summary>
        /// Indicates that a security is used as a medium of exchange.
        /// </summary>
        Currency,

        /// <summary>
        /// Indicates that a security entitles the holder to a share in the ownership of a company.
        /// </summary>
        Stock,

        /// <summary>
        /// Indicates that a security is a collective investment of different types of securities.
        /// </summary>
        Fund,
    }
}
