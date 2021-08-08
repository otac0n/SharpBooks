// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace SharpBooks
{
    using System;

    /// <summary>
    /// Holds the exchange rate of a security for a currency.
    /// </summary>
    public class PriceQuote
    {
        public PriceQuote(Guid priceQuoteId, DateTime dateTime, Security security, long quantity, Security currency, long price, string source)
        {
            if (priceQuoteId == Guid.Empty)
            {
                throw new ArgumentOutOfRangeException(nameof(priceQuoteId));
            }

            if (dateTime.Kind != DateTimeKind.Utc)
            {
                throw new ArgumentOutOfRangeException(nameof(dateTime));
            }

            if (security == null)
            {
                throw new ArgumentNullException(nameof(security));
            }

            if (currency == null)
            {
                throw new ArgumentNullException(nameof(currency));
            }

            if (string.IsNullOrEmpty(source))
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (quantity <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(quantity));
            }

            if (currency.SecurityType != SecurityType.Currency)
            {
                throw new InvalidOperationException("Could not create a price quote, because the currency parameter was not a valid currency.");
            }

            if (currency == security)
            {
                throw new InvalidOperationException("Could not create a price quote, because the security and currency were the same.");
            }

            if (price <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(price));
            }

            this.PriceQuoteId = priceQuoteId;
            this.DateTime = dateTime;
            this.Security = security;
            this.Quantity = quantity;
            this.Currency = currency;
            this.Price = price;
            this.Source = source;
        }

        public Security Currency { get; }

        public DateTime DateTime { get; }

        public long Price { get; }

        public Guid PriceQuoteId { get; }

        public long Quantity { get; }

        public Security Security { get; }

        public string Source { get; }
    }
}
