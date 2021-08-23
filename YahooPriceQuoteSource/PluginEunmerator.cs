// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace YahooPriceQuoteSource
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
            yield return new PluginFactory<YahooPriceQuoteSource>("Yahoo!® Finance Price Quotes");
        }
    }
}
