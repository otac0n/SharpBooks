//-----------------------------------------------------------------------
// <copyright file="IWidget.cs" company="(none)">
//  Copyright © 2010 John Gietzen. All rights reserved.
// </copyright>
// <author>John Gietzen</author>
//-----------------------------------------------------------------------

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
