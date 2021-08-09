// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace SharpBooks.Plugins
{
    using System.Drawing;

    public interface IReport : IPlugin
    {
        string Configure(IReadOnlyBook book, string currentSettings);

        void Render(IReadOnlyBook book, Graphics g);

        void SetConfiguration(IReadOnlyBook book, string settings);
    }
}
