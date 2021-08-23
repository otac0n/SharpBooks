// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace SharpBooks.Plugins
{
    /// <summary>
    /// Implements a generic plugin factory.
    /// </summary>
    /// <typeparam name="T">The type of plugins that will be instantiated.</typeparam>
    public class PluginFactory<T> : IPluginFactory<T>
        where T : IPlugin, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PluginFactory{T}"/> class.
        /// </summary>
        /// <param name="name">The name of the plugin being instantiated.</param>
        public PluginFactory(string name)
        {
            this.Name = name;
        }

        /// <inheritdoc/>
        public string Name { get; }

        /// <inheritdoc/>
        public T CreateInstance()
        {
            return new T();
        }
    }
}
