//-----------------------------------------------------------------------
// <copyright file="PriceQuoteSourceFactory.cs" company="(none)">
//  Copyright © 2010 John Gietzen. All rights reserved.
// </copyright>
// <author>John Gietzen</author>
//-----------------------------------------------------------------------

namespace SharpBooks.Plugins
{
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
