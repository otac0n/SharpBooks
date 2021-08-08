// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace SharpBooks.Events
{
    using System;

    public class PriceQuoteAddedEventArgs : EventArgs
    {
        private readonly PriceQuote priceQuote;

        public PriceQuoteAddedEventArgs(PriceQuote priceQuote)
        {
            if (priceQuote == null)
            {
                throw new ArgumentNullException("priceQuote");
            }

            this.priceQuote = priceQuote;
        }

        public PriceQuote PriceQuote
        {
            get
            {
                return this.priceQuote;
            }
        }
    }
}
