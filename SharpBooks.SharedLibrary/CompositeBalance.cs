// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace SharpBooks
{
    using System.Collections.Generic;
    using System.Linq;

    public sealed class CompositeBalance
    {
        private readonly IList<Balance> balances;

        public CompositeBalance()
        {
            this.balances = new Balance[0];
        }

        public CompositeBalance(IEnumerable<Balance> balances)
        {
            this.balances = CombineBalances(balances).AsReadOnly();
        }

        public CompositeBalance(CompositeBalance balance, IEnumerable<CompositeBalance> balances)
        {
            this.balances = CombineBalances(balances.Concat(new[] { balance })).AsReadOnly();
        }

        private CompositeBalance(System.Collections.ObjectModel.ReadOnlyCollection<Balance> balances)
        {
            this.balances = balances;
        }

        public ICollection<Balance> Balances
        {
            get
            {
                return this.balances;
            }
        }

        /// <summary>
        /// Adds a single currency <see cref="Balance"/> to the current <see cref="CompositeBalance"/> and returns the new composite balance.
        /// </summary>
        /// <param name="balance">The balance to combine with the current composite balance.</param>
        /// <returns>The new composite balance.</returns>
        public CompositeBalance CombineWith(Balance balance)
        {
            return this.CombineWith(balance.Security, balance.Amount, balance.IsExact);
        }

        /// <summary>
        /// Adds an amount to the current <see cref="CompositeBalance"/> and returns the new composite balance.
        /// </summary>
        /// <param name="security">The security of the amount.</param>
        /// <param name="amount">The amount to add.</param>
        /// <returns>The new composite balance.</returns>
        /// <remarks>
        /// If any of the component balances can be reused, they are.
        /// </remarks>
        public CompositeBalance CombineWith(Security security, long amount, bool isExact)
        {
            // If the new amount will not change the balances in any way, return the original balance.
            if (amount == 0 && isExact)
            {
                return this;
            }

            // Create a new storage location, since we will be changing balances.
            var newBalances = new List<Balance>(this.balances.Count + 1);

            // Go through the list of current balances.
            var combined = false;
            foreach (var bal in this.balances)
            {
                // Reuse the existing balance, if possible.
                if (bal.Security != security)
                {
                    newBalances.Add(bal);
                }
                else
                {
                    combined = true;

                    // If it must be combined, calculate the new amount and propagate the inexact flag.
                    var newAmount = bal.Amount + amount;
                    var newIsExact = bal.IsExact && isExact;

                    // If the result is either not exact, or not zero, add it.
                    if (newAmount != 0 || !newIsExact)
                    {
                        newBalances.Add(new Balance(security, newAmount, newIsExact));
                    }
                }
            }

            // If it was not combined with an existing balance, create a new one for it.
            if (!combined)
            {
                newBalances.Add(new Balance(security, amount, isExact));
            }

            return new CompositeBalance(newBalances.AsReadOnly());
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
