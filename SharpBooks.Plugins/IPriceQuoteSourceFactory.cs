//-----------------------------------------------------------------------
// <copyright file="IPriceQuoteSourceFactory.cs" company="Microsoft">
//  Copyright (c) 2010 Microsoft
// </copyright>
// <author>otac0n</author>
//-----------------------------------------------------------------------

namespace SharpBooks.Plugins
{
    public interface IPriceQuoteSourceFactory : IPluginFactory
    {
        IPriceQuoteSource CreateInstance();
    }
}
