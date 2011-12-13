//-----------------------------------------------------------------------
// <copyright file="PriceQuoteAddedEventArgs.cs" company="(none)">
//  Copyright © 2010 John Gietzen. All rights reserved.
// </copyright>
// <author>John Gietzen</author>
//-----------------------------------------------------------------------

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
