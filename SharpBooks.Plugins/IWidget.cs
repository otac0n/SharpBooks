﻿//-----------------------------------------------------------------------
// <copyright file="IWidget.cs" company="(none)">
//  Copyright © 2010 John Gietzen. All rights reserved.
// </copyright>
// <author>John Gietzen</author>
//-----------------------------------------------------------------------

namespace SharpBooks.Plugins
{
    using System;
    using System.Windows;

    public interface IWidget : IDisposable
    {
        FrameworkElement Create(ReadOnlyBook book, object events);

        void Refresh(ReadOnlyBook book);
    }
}
