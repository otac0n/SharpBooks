// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace SharpBooks.Plugins
{
    using System.Windows.Forms;

    public interface IWidget : IPlugin
    {
        Control Create(IReadOnlyBook book, EventProxy events);

        void Refresh(IReadOnlyBook book, EventProxy events);
    }
}
