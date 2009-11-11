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
    }
}
