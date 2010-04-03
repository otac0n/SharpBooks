//-----------------------------------------------------------------------
// <copyright file="IPriceQuoteSourceFactory.cs" company="(none)">
//  Copyright © 2010 John Gietzen. All rights reserved.
// </copyright>
// <author>John Gietzen</author>
//-----------------------------------------------------------------------

namespace SharpBooks.Plugins
{
    using System;

    public interface IPriceQuoteSourceFactory : IPluginFactory
    {
        IPriceQuoteSource CreateInstance();
    }
}
