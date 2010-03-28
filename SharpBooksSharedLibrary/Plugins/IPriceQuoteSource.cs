//-----------------------------------------------------------------------
// <copyright file="IPriceSource.cs" company="Microsoft">
//  Copyright © 2010 Microsoft
// </copyright>
// <author>otac0n</author>
//-----------------------------------------------------------------------

namespace SharpBooks.Plugins
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Describes the interface for retrieving external price quotes for securities.
    /// </summary>
    public interface IPriceQuoteSource : IPlugin
    {
        PriceQuote GetPriceQuote(Security security, Security currecny);
    }
}
