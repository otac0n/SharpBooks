// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace SharpBooks.Plugins
{
    /// <summary>
    /// Describes the interface for retrieving external price quotes for securities.
    /// </summary>
    public interface IPriceQuoteSource : IPlugin
    {
        PriceQuote GetPriceQuote(Security security, Security currency);
    }
}
