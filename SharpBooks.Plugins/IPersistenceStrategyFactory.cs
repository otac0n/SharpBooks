﻿//-----------------------------------------------------------------------
// <copyright file="IPriceQuoteSourceFactory.cs" company="(none)">
//  Copyright © 2010 John Gietzen. All rights reserved.
// </copyright>
// <author>John Gietzen</author>
//-----------------------------------------------------------------------

namespace SharpBooks.Plugins
{
    using SharpBooks.Persistence;

    public interface IPersistenceStrategyFactory : IPluginFactory
    {
        IPersistenceStrategy CreateInstance();
    }
}
