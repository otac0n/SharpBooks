// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace SharpBooks.Plugins
{
    public interface IReportFactory : IPluginFactory
    {
        string Configure(IReadOnlyBook book, string currentSettings);

        IReport CreateInstance(IReadOnlyBook book, string settings);
    }
}
