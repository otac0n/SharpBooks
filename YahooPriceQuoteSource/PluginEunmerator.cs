//-----------------------------------------------------------------------
// <copyright file="PluginEunmerator.cs" company="Microsoft">
//  Copyright (c) 2010 Microsoft
// </copyright>
// <author>otac0n</author>
//-----------------------------------------------------------------------

namespace YahooPriceQuoteSource
{
    using System;
    using SharpBooks.Plugins;
    using System.Collections.Generic;

    public class PluginEunmerator : IPluginEnumerator
    {
        public IEnumerable<IPluginFactory> EnumerateFactories()
        {
            yield return new PriceQuoteSourceFactory<YahooPriceQuoteSource>();
        }
    }
}
