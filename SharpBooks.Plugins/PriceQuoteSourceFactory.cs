// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace SharpBooks.Plugins
{
    public class PriceQuoteSourceFactory<T> : IPriceQuoteSourceFactory
        where T : IPriceQuoteSource, new()
    {
        public PriceQuoteSourceFactory(string name)
        {
            this.Name = name;
        }

        /// <inheritdoc/>
        public string Name
        {
            get;
            private set;
        }

        /// <inheritdoc/>
        public IPriceQuoteSource CreateInstance()
        {
            return new T();
        }
    }
}
