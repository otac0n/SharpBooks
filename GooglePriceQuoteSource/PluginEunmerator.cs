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

    /// <summary>
    /// Enumerates all of the plugins for the assembly.
    /// </summary>
    public class PluginEunmerator : IPluginEnumerator
    {
        /// <summary>
        /// Enumerates all of the plugins for the assembly.
        /// </summary>
        /// <returns>An enumerable list of factories that can create plugins.</returns>
        public IEnumerable<IPluginFactory> EnumerateFactories()
        {
            yield return new PriceQuoteSourceFactory<GoogleCurrencyPriceQuoteSource>();
        }
    }
}
