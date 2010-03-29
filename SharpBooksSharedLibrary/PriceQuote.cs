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
        public PriceQuote(DateTime dateTime, Security security, long quantity, Security currency, long price)
        {
            if (quantity <= 0)
            {
                throw new ArgumentOutOfRangeException("quantity");
            }

            if (price <= 0)
            {
                throw new ArgumentOutOfRangeException("quantity");
            }

            this.DateTime = dateTime;
            this.Security = security;
            this.Quantity = quantity;
            this.Currency = currency;
            this.Price = price;
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
    }
}
