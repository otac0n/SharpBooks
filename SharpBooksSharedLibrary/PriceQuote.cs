//-----------------------------------------------------------------------
// <copyright file="Price.cs" company="Microsoft">
//  Copyright © 2010 Microsoft
// </copyright>
// <author>otac0n</author>
//-----------------------------------------------------------------------

namespace SharpBooks
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Holds the exchange rate of a security for a currency.
    /// </summary>
    public class PriceQuote
    {
        public PriceQuote(Guid priceQuoteId, DateTime dateTime, Security security, long quantity, Security currency, long price, string source)
        {
            if (priceQuoteId == Guid.Empty)
            {
                throw new ArgumentOutOfRangeException("priceQuoteId");
            }

            if (security == null)
            {
                throw new ArgumentNullException("security");
            }

            if (currency == null)
            {
                throw new ArgumentNullException("currency");
            }

            if (string.IsNullOrEmpty(source))
            {
                throw new ArgumentNullException("source");
            }

            if (quantity <= 0)
            {
                throw new ArgumentOutOfRangeException("quantity");
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
                throw new ArgumentOutOfRangeException("price");
            }

            this.PriceQuoteId = priceQuoteId;
            this.DateTime = dateTime;
            this.Security = security;
            this.Quantity = quantity;
            this.Currency = currency;
            this.Price = price;
            this.Source = source;
        }

        public Guid PriceQuoteId
        {
            get;
            private set;
        }

        public DateTime DateTime
        {
            get;
            private set;
        }

        public Security Security
        {
            get;
            private set;
        }

        public long Quantity
        {
            get;
            private set;
        }

        public Security Currency
        {
            get;
            private set;
        }

        public long Price
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
