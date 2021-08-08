// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace SharpBooks.Plugins
{
    using System.Collections.Generic;

    /// <summary>
    /// Enumerates some or all of the plugins available in the assembly.
    /// </summary>
    /// <remarks>
    /// The application will search for instances of this interface in
    /// order to enumerate the plugins in the plugin assembly.
    /// </remarks>
    public interface IPluginEnumerator
    {
        /// <summary>
        /// Enumerates some or all of the plugins available in the assembly.
        /// </summary>
        /// <returns>An enumerable list of factories that can create plugins.</returns>
        IEnumerable<IPluginFactory> EnumerateFactories();
    }
}
