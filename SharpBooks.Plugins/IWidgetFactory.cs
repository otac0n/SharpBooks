//-----------------------------------------------------------------------
// <copyright file="IWidgetFactory.cs" company="(none)">
//  Copyright © 2010 John Gietzen. All rights reserved.
// </copyright>
// <author>John Gietzen</author>
//-----------------------------------------------------------------------

namespace SharpBooks.Plugins
{
    public interface IWidgetFactory : IPluginFactory
    {
        string Configure(ReadOnlyBook book, string currentSettings);

        IWidget CreateInstance(ReadOnlyBook book, string settings);
    }
}
