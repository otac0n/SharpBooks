//-----------------------------------------------------------------------
// <copyright file="IPluginFactory.cs" company="(none)">
//  Copyright © 2010 John Gietzen. All rights reserved.
// </copyright>
// <author>John Gietzen</author>
//-----------------------------------------------------------------------

namespace SharpBooks.Plugins
{
    /// <summary>
    /// Describes the base interface for all plugin factories.
    /// </summary>
    /// <remarks>
    /// Each plugin factory must have a name, but may have creation
    /// methods with varing signatures.  Because of this, only the
    /// name can be read from a generic plugin factory.
    /// </remarks>
    public interface IPluginFactory
    {
        /// <summary>
        /// Gets the name of this plugin factory.
        /// </summary>
        string Name
        {
            get;
        }
    }
}
