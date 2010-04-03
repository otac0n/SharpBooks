//-----------------------------------------------------------------------
// <copyright file="PluginEunmerator.cs" company="Microsoft">
//  Copyright (c) 2010 Microsoft
// </copyright>
// <author>otac0n</author>
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
