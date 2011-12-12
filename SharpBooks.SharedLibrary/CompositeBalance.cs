//-----------------------------------------------------------------------
// <copyright file="CompositeBalance.cs" company="(none)">
//  Copyright © 2010 John Gietzen. All rights reserved.
// </copyright>
// <author>John Gietzen</author>
//-----------------------------------------------------------------------

namespace SharpBooks
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public sealed class CompositeBalance
    {
        private readonly ICollection<Balance> balances;

        public CompositeBalance(IEnumerable<Balance> balances)
        {
            this.balances = CombineBalances(balances).AsReadOnly();
        }

        public CompositeBalance(CompositeBalance balance, IEnumerable<CompositeBalance> balances)
        {
            this.balances = CombineBalances(balances.Concat(new[] { balance })).AsReadOnly();
        }

        public ICollection<Balance> Balances
        {
            get
            {
                return this.balances;
            }
        }

        public override string ToString()
        {
            if (this.balances.Count > 0)
            {
                return string.Join(", ", this.balances.Select(b => b.ToString()));
            }
            else
            {
                return "0";
            }
        }

        private static List<Balance> CombineBalances(IEnumerable<CompositeBalance> balances)
        {
            if (balances == null)
            {
                return new List<Balance>();
            }
            else
            {
                return CombineBalances(balances.SelectMany(s => s.Balances));
            }
        }

        private static List<Balance> CombineBalances(IEnumerable<Balance> balances)
        {
            if (balances == null)
            {
                return new List<Balance>();
            }
            else
            {
                return (from b in balances
                        group b by b.Security into g
                        let amount = g.Sum(c => c.Amount)
                        let exact = g.All(c => c.IsExact)
                        where amount != 0 || !exact
                        select new Balance(
                            g.Key,
                            amount,
                            exact)).ToList();
            }
        }
    }
}
