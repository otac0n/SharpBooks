using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpCash
{
    public class Price
    {
        private Guid id;
        private Commodity commodity;
        private Commodity currency;
        private DateTime time;
        private string source;
        private string type;
        private decimal value;

        internal Price(Guid id, Commodity commodity, Commodity currency, DateTime time, string source, string type, decimal value)
        {
            this.id = id;
            this.commodity = commodity;
            this.currency = currency;
            this.time = time;
            this.source = source;
            this.type = type;
            this.value = value;
        }

        public Guid Id
        {
            get
            {
                return this.id;
            }
        }

        public Commodity Commodity
        {
            get
            {
                return this.commodity;
            }
        }

        public Commodity Currency
        {
            get
            {
                return this.currency;
            }
        }

        public DateTime Time
        {
            get
            {
                return this.time;
            }
        }

        public string Source
        {
            get
            {
                return this.source;
            }
        }

        public string Type
        {
            get
            {
                return this.type;
            }
        }

        public decimal Value
        {
            get
            {
                return this.value;
            }
        }
    }
}
