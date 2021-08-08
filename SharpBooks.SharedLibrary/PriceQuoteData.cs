// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace SharpBooks
{
    using System;

    /// <summary>
    /// Holds a read-only, persistable copy of a price quote.
    /// </summary>
    public class PriceQuoteData
    {
        public PriceQuoteData(PriceQuote priceQuote)
        {
            if (priceQuote == null)
            {
                throw new ArgumentNullException(nameof(priceQuote));
            }

            this.PriceQuoteId = priceQuote.PriceQuoteId;
            this.DateTime = priceQuote.DateTime;
            this.SecuritySecurityId = priceQuote.Security.SecurityId;
            this.Quantity = priceQuote.Quantity;
            this.CurrencySecurityId = priceQuote.Currency.SecurityId;
            this.Price = priceQuote.Price;
            this.Source = priceQuote.Source;
        }

        public Guid CurrencySecurityId { get; }

        public DateTime DateTime { get; }

        public long Price { get; }

        public Guid PriceQuoteId { get; }

        public long Quantity { get; }

        public Guid SecuritySecurityId { get; }

        public string Source { get; }
    }
}
