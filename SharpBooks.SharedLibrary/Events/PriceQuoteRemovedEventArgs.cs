// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace SharpBooks.Events
{
    using System;

    public class PriceQuoteRemovedEventArgs : EventArgs
    {
        public PriceQuoteRemovedEventArgs(PriceQuote priceQuote)
        {
            if (priceQuote == null)
            {
                throw new ArgumentNullException(nameof(priceQuote));
            }

            this.PriceQuote = priceQuote;
        }

        public PriceQuote PriceQuote { get; }
    }
}
