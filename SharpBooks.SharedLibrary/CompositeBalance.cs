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
        private readonly Security security;
        private readonly ICollection<Balance> balances;

        public CompositeBalance(Security security, Balance balance, IEnumerable<CompositeBalance> balances)
        {
            if (security == null)
            {
                throw new ArgumentNullException("security");
            }

            if (balance == null)
            {
                throw new ArgumentNullException("balance");
            }

            this.security = security;
            this.balances = CombineBalances(balance, balances).AsReadOnly();
        }

        public Security Security
        {
            get
            {
                return this.security;
            }
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
            return string.Join(", ", this.balances.Select(b => b.ToString()));
        }

        private static List<Balance> CombineBalances(Balance balance, IEnumerable<CompositeBalance> balances)
        {
            if (balance == null)
            {
                throw new ArgumentNullException("balance");
            }

            if (balances == null)
            {
                var bal = new List<Balance>();
                bal.Add(balance);
                return bal;
            }
            else
            {
                var bal = new List<Balance>(balances.SelectMany(b => b.Balances));
                bal.Add(balance);

                return (from b in bal
                        group b by b.Security into g
                        select new Balance(
                            g.Key,
                            g.Sum(c => c.Amount),
                            g.All(c => c.IsExact))).ToList();
            }
        }
    }
}
