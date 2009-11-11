using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpCash
{
    public class PriceDatabase
    {
        private List<Price> prices;

        internal PriceDatabase(IEnumerable<Price> prices)
        {
            this.prices = new List<Price>();
            if (prices != null)
            {
                this.prices.AddRange(prices);
            }
        }

        public ICollection<Price> Prices
        {
            get
            {
                return this.prices.AsReadOnly();
            }
        }

        public decimal? GetPrice(Commodity commodity, Commodity currency)
        {
            if (commodity == currency)
            {
                return 1;
            }

            var price = GetPrice(commodity, currency, DateTime.Today);

            if (price == null)
            {
                return null;
            }
            else
            {
                return price.Value;
            }
        }

        private Price GetPrice(Commodity commodity, Commodity currency, DateTime date)
        {
            return (from p in this.prices
                     where p.Commodity == commodity
                     where p.Currency == currency
                     orderby Math.Abs((date - p.Time).TotalDays)
                     select p).FirstOrDefault();
        }
    }
}
