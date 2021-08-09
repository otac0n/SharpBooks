// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace SharpBooks.StandardPlugins
{
    using System.Collections.Generic;
    using SharpBooks.Plugins;

    /// <summary>
    /// Enumerates all of the plugins for the assembly.
    /// </summary>
    public class PluginEnumerator : IPluginEnumerator
    {
        /// <summary>
        /// Enumerates all of the plugins for the assembly.
        /// </summary>
        /// <returns>An enumerable list of factories that can create plugins.</returns>
        public IEnumerable<IPluginFactory> EnumerateFactories()
        {
            yield return new FavoriteAccountsWidgetFactory();
            yield return new RecentExpensesWidgetFactory();
            yield return new PluginFactory<XmlPersistenceStrategy>("XML File");
        }
    }
}
