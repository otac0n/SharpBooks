// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace SharpBooks.Events
{
    using System;

    public class PriceQuoteRemovedEventArgs : EventArgs
    {
        private readonly PriceQuote priceQuote;

        public PriceQuoteRemovedEventArgs(PriceQuote priceQuote)
        {
            if (priceQuote == null)
            {
                throw new ArgumentNullException(nameof(priceQuote));
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
