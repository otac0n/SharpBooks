//-----------------------------------------------------------------------
// <copyright file="PluginEunmerator.cs" company="(none)">
//  Copyright © 2010 John Gietzen. All rights reserved.
// </copyright>
// <author>John Gietzen</author>
//-----------------------------------------------------------------------

namespace GooglePriceQuoteSource
{
    using System.Collections.Generic;
    using SharpBooks.Plugins;

    public class PluginEunmerator : IPluginEnumerator
    {
        public IEnumerable<IPluginFactory> EnumerateFactories()
        {
            yield return new PriceQuoteSourceFactory<GoogleCurrencyPriceQuoteSource>();
        }
    }
}
