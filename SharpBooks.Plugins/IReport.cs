// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace SharpBooks.Plugins
{
    using System;
    using System.Drawing;

    public interface IReport : IDisposable
    {
        void Render(IReadOnlyBook book, Graphics g);
    }
}
