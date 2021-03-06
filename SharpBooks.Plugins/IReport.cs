﻿//-----------------------------------------------------------------------
// <copyright file="IReport.cs" company="(none)">
//  Copyright © 2010 John Gietzen. All rights reserved.
// </copyright>
// <author>John Gietzen</author>
//-----------------------------------------------------------------------

namespace SharpBooks.Plugins
{
    using System;
    using System.Drawing;

    public interface IReport : IDisposable
    {
        void Render(ReadOnlyBook book, Graphics g);
    }
}
