﻿//-----------------------------------------------------------------------
// <copyright file="IWidgetFactory.cs" company="(none)">
//  Copyright © 2010 John Gietzen. All rights reserved.
// </copyright>
// <author>John Gietzen</author>
//-----------------------------------------------------------------------

namespace SharpBooks.Plugins
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public interface IWidgetFactory : IPluginFactory
    {
        IWidget CreateInstance(ReadOnlyBook book, string settings);

        string Configure(ReadOnlyBook book, string currentSettings);
    }
}
