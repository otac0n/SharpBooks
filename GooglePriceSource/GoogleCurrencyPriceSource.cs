//-----------------------------------------------------------------------
// <copyright file="GooglePriceSource.cs" company="(none)">
//  Copyright © 2010 John Gietzen
// </copyright>
// <author>John Gietzen</author>
//-----------------------------------------------------------------------

namespace GooglePriceSource
{
    using System;
    using System.Diagnostics;
    using System.Net;
    using SharpBooks;
    using SharpBooks.Plugins;

    public class GoogleCurrencyPriceSource : IPriceQuoteSource
    {
        private const string UrlFormat = "http://www.google.com/ig/calculator?hl=en&q=1+{0}+in+{1}";

        public string Name
        {
            get
            {
                return "Google™ Calculator Price Quotes";
            }
        }

        public PriceQuote GetPriceQuote(Security security, Security currecny)
        {
            throw new NotImplementedException();
        }
    }
}
