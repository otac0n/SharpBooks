using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpBooks.Plugins;

namespace SharpBooks.StandardPlugins
{
    public class PluginEnumerator : IPluginEnumerator
    {
        public IEnumerable<IPluginFactory> EnumerateFactories()
        {
            yield return new FavoriteAccountsWidgetFactory();
        }
    }
}
