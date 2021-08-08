// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace SharpBooks.Plugins
{
    using System;
    using System.Windows.Forms;

    public interface IWidget : IDisposable
    {
        Control Create(ReadOnlyBook book, EventProxy events);

        void Refresh(ReadOnlyBook book, EventProxy events);
    }
}
