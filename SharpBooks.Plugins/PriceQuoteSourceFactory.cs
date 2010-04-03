//-----------------------------------------------------------------------
// <copyright file="PriceQuoteSourceFactory.cs" company="Microsoft">
//  Copyright (c) 2010 Microsoft
// </copyright>
// <author>otac0n</author>
//-----------------------------------------------------------------------

namespace SharpBooks.Plugins
{
    using System;

    public class PriceQuoteSourceFactory<T> : IPriceQuoteSourceFactory where T : IPriceQuoteSource, new()
    {
        public PriceQuoteSourceFactory()
        {
            this.Name = new T().Name;
        }

        public string Name
        {
            get;
            private set;
        }

        public IPriceQuoteSource CreateInstance()
        {
            return new T();
        }
    }
}
