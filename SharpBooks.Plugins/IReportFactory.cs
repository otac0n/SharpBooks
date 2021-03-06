﻿//-----------------------------------------------------------------------
// <copyright file="IReportFactory.cs" company="(none)">
//  Copyright © 2010 John Gietzen. All rights reserved.
// </copyright>
// <author>John Gietzen</author>
//-----------------------------------------------------------------------

namespace SharpBooks.Plugins
{
    public interface IReportFactory : IPluginFactory
    {
        IReport CreateInstance(ReadOnlyBook book, string settings);

        string Configure(ReadOnlyBook book, string currentSettings);
    }
}
