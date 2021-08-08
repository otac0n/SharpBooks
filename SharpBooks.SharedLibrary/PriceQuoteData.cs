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

        public Guid CurrencySecurityId
        {
            get;
            private set;
        }

        public DateTime DateTime
        {
            get;
            private set;
        }

        public long Price
        {
            get;
            private set;
        }

        public Guid PriceQuoteId
        {
            get;
            private set;
        }

        public long Quantity
        {
            get;
            private set;
        }

        public Guid SecuritySecurityId
        {
            get;
            private set;
        }

        public string Source
        {
            get;
            private set;
        }
    }
}
