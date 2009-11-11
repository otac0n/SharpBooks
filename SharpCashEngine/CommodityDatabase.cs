using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpCash
{
    public class CommodityDatabase
    {
        private List<Commodity> commodities;

        internal CommodityDatabase(IEnumerable<Commodity> commodities)
        {
            this.commodities = new List<Commodity>();
            if (commodities != null)
            {
                this.commodities.AddRange(commodities);
            }
        }

        public ICollection<Commodity> Commodities
        {
            get
            {
                return this.commodities.AsReadOnly();
            }
        }

        public Commodity FindCommodity(string space, string id)
        {
            return (from c in this.commodities
                    where c.Space == space
                    where c.Id == id
                    select c).SingleOrDefault();
        }
    }
}
