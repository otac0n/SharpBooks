//-----------------------------------------------------------------------
// <copyright file="CommodityType.cs" company="(none)">
//  Copyright (c) 2010 John Gietzen
// </copyright>
// <author>John Gietzen</author>
//-----------------------------------------------------------------------

namespace SharpBooks
{
    /// <summary>
    /// Defines the possible types of commodities.
    /// </summary>
    public enum CommodityType
    {
        /// <summary>
        /// Indicates that a commodity is used as a medium of exchange.
        /// </summary>
        Currency,

        /// <summary>
        /// Indicates that a commodity entitles the holder to a share in the ownership of a company.
        /// </summary>
        Stock,
    }
}
