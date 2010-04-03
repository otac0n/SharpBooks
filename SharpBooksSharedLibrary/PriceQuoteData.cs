//-----------------------------------------------------------------------
// <copyright file="PriceQuoteData.cs" company="(none)">
//  Copyright © 2010 John Gietzen
// </copyright>
// <author>John Gietzen</author>
//-----------------------------------------------------------------------

namespace SharpBooks
{
    using System;

    public class PriceQuoteData
    {
        public PriceQuoteData(PriceQuote priceQuote)
        {
            this.PriceQuoteId = priceQuote.PriceQuoteId;
            this.DateTime = priceQuote.DateTime;
            this.SecuritySecurityId = priceQuote.Security.SecurityId;
            this.Quantity = priceQuote.Quantity;
            this.CurrencySecurityId = priceQuote.Currency.SecurityId;
            this.Price = priceQuote.Price;
            this.Source = priceQuote.Source;
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

        public Guid SecuritySecurityId
        {
            get;
            private set;
        }

        public long Quantity
        {
            get;
            private set;
        }

        public Guid CurrencySecurityId
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
