//-----------------------------------------------------------------------
// <copyright file="PriceQuoteRemovedEventArgs.cs" company="(none)">
//  Copyright © 2010 John Gietzen. All rights reserved.
// </copyright>
// <author>John Gietzen</author>
//-----------------------------------------------------------------------

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
