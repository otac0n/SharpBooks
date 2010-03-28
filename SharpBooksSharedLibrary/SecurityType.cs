//-----------------------------------------------------------------------
// <copyright file="SecurityType.cs" company="(none)">
//  Copyright © 2010 John Gietzen
// </copyright>
// <author>John Gietzen</author>
//-----------------------------------------------------------------------

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
