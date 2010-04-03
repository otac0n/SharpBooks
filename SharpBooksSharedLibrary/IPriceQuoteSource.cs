//-----------------------------------------------------------------------
// <copyright file="IPriceQuoteSource.cs" company="(none)">
//  Copyright © 2010 John Gietzen. All rights reserved.
// </copyright>
// <author>John Gietzen</author>
//-----------------------------------------------------------------------

namespace SharpBooks
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using SharpBooks.Plugins;

    /// <summary>
    /// Describes the interface for retrieving external price quotes for securities.
    /// </summary>
    public interface IPriceQuoteSource : IPlugin
    {
        PriceQuote GetPriceQuote(Security security, Security currency);
    }
}
