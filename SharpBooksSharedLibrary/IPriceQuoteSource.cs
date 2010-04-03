//-----------------------------------------------------------------------
// <copyright file="IPriceQuoteSource.cs" company="(none)">
//  Copyright © 2010 John Gietzen. All rights reserved.
// </copyright>
// <author>John Gietzen</author>
//-----------------------------------------------------------------------

namespace SharpBooks
{
    using System;

    /// <summary>
    /// Describes the interface for retrieving external price quotes for securities.
    /// </summary>
    public interface IPriceQuoteSource : IDisposable
    {
        string Name
        {
            get;
        }

        PriceQuote GetPriceQuote(Security security, Security currency);
    }
}
