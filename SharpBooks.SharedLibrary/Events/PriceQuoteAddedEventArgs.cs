// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace SharpBooks.Events
{
    using System;

    public class PriceQuoteAddedEventArgs : EventArgs
    {
        public PriceQuoteAddedEventArgs(PriceQuote priceQuote)
        {
            this.PriceQuote = priceQuote ?? throw new ArgumentNullException(nameof(priceQuote));
        }

        public PriceQuote PriceQuote { get; }
    }
}
