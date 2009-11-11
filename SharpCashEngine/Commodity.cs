using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpCash
{
    public class Commodity
    {
        private string id;
        private string space;
        private string name;
        private string xCode;
        private int fraction;
        private bool getQuotes;
        private string quoteSource;
        private string quoteTimeZone;

        internal Commodity(string id, string space, string name, string xCode, int fraction, bool getQuotes, string quoteSource, string quoteTimeZone)
        {
            this.id = id;
            this.space = space;
            this.name = name;
            this.xCode = xCode;
            this.fraction = fraction;
            this.getQuotes = getQuotes;
            this.quoteSource = quoteSource;
            this.quoteTimeZone = quoteTimeZone;
        }

        public string Id
        {
            get
            {
                return this.id;
            }
        }

        public string Space
        {
            get
            {
                return this.space;
            }
        }

        public string Name
        {
            get
            {
                return this.name;
            }
        }

        public string XCode
        {
            get
            {
                return this.xCode;
            }
        }

        public int Fraction
        {
            get
            {
                return this.fraction;
            }
        }

        public bool GetQuotes
        {
            get
            {
                return this.getQuotes;
            }
        }

        public string QuoteSource
        {
            get
            {
                return this.quoteSource;
            }
        }

        public string QuoteTimeZone
        {
            get
            {
                return this.quoteTimeZone;
            }
        }
    }
}
