// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace SharpBooks.Plugins
{
    /// <summary>
    /// Describes the generic interface for all plugin factories.
    /// </summary>
    /// <typeparam name="T">The type of plugins that can be instantiated.</typeparam>
    public interface IPluginFactory<out T> : IPluginFactory
        where T : IPlugin
    {
        /// <summary>
        /// Instantiates an instance of the specified plugin.
        /// </summary>
        /// <returns>The created plugin instance.</returns>
        T CreateInstance();
    }
}
