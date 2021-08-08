// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace SharpBooks.Plugins
{
    public interface IWidgetFactory : IPluginFactory
    {
        string Configure(ReadOnlyBook book, string currentSettings);

        IWidget CreateInstance(ReadOnlyBook book, string settings);
    }
}
