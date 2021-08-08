// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace SharpBooks.Tests.Fakes
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    internal class MockSaver : ISaver
    {
        private readonly List<OrderedGuid> accountAdditions = new List<OrderedGuid>();
        private readonly List<OrderedGuid> accountRemovals = new List<OrderedGuid>();
        private readonly List<OrderedGuid> priceQuoteAdditions = new List<OrderedGuid>();
        private readonly List<OrderedGuid> priceQuoteRemovals = new List<OrderedGuid>();
        private readonly List<OrderedGuid> securityAdditions = new List<OrderedGuid>();

        private readonly List<OrderedGuid> securityRemovals = new List<OrderedGuid>();
        private readonly List<OrderedGuid> transactionAdditions = new List<OrderedGuid>();

        private readonly List<OrderedGuid> transactionRemovals = new List<OrderedGuid>();

        private long orderIndex;

        public ReadOnlyCollection<OrderedGuid> AccountAdditions
        {
            get
            {
                return this.accountAdditions.AsReadOnly();
            }
        }

        public ReadOnlyCollection<OrderedGuid> AccountRemovals
        {
            get
            {
                return this.accountRemovals.AsReadOnly();
            }
        }

        public ReadOnlyCollection<OrderedGuid> PriceQuoteAdditions
        {
            get
            {
                return this.priceQuoteAdditions.AsReadOnly();
            }
        }

        public ReadOnlyCollection<OrderedGuid> PriceQuoteRemovals
        {
            get
            {
                return this.priceQuoteRemovals.AsReadOnly();
            }
        }

        public ReadOnlyCollection<OrderedGuid> SecurityAdditions
        {
            get
            {
                return this.securityAdditions.AsReadOnly();
            }
        }

        public ReadOnlyCollection<OrderedGuid> SecurityRemovals
        {
            get
            {
                return this.securityRemovals.AsReadOnly();
            }
        }

        public ReadOnlyCollection<OrderedGuid> TransactionAdditions
        {
            get
            {
                return this.transactionAdditions.AsReadOnly();
            }
        }

        public ReadOnlyCollection<OrderedGuid> TransactionRemovals
        {
            get
            {
                return this.transactionRemovals.AsReadOnly();
            }
        }

        public void AddAccount(AccountData account)
        {
            if (account == null)
            {
                throw new ArgumentNullException("account");
            }

            this.accountAdditions.Add(new OrderedGuid
            {
                Guid = account.AccountId,
                Order = this.orderIndex++,
            });
        }

        public void AddPriceQuote(PriceQuoteData priceQuote)
        {
            if (priceQuote == null)
            {
                throw new ArgumentNullException("priceQuote");
            }

            this.priceQuoteAdditions.Add(new OrderedGuid
            {
                Guid = priceQuote.PriceQuoteId,
                Order = this.orderIndex++,
            });
        }

        public void AddSecurity(SecurityData security)
        {
            if (security == null)
            {
                throw new ArgumentNullException("security");
            }

            this.securityAdditions.Add(new OrderedGuid
            {
                Guid = security.SecurityId,
                Order = this.orderIndex++,
            });
        }

        public void AddTransaction(TransactionData transaction)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException("transaction");
            }

            this.transactionAdditions.Add(new OrderedGuid
            {
                Guid = transaction.TransactionId,
                Order = this.orderIndex++,
            });
        }

        public void RemoveAccount(Guid accountId)
        {
            this.accountRemovals.Add(new OrderedGuid
            {
                Guid = accountId,
                Order = this.orderIndex++,
            });
        }

        public void RemovePriceQuote(Guid priceQuoteId)
        {
            this.priceQuoteRemovals.Add(new OrderedGuid
            {
                Guid = priceQuoteId,
                Order = this.orderIndex++,
            });
        }

        public void RemoveSecurity(Guid securityId)
        {
            this.securityRemovals.Add(new OrderedGuid
            {
                Guid = securityId,
                Order = this.orderIndex++,
            });
        }

        public void RemoveSetting(string key)
        {
        }

        public void RemoveTransaction(Guid transactionId)
        {
            this.transactionRemovals.Add(new OrderedGuid
            {
                Guid = transactionId,
                Order = this.orderIndex++,
            });
        }

        public void SetSetting(string key, string value)
        {
        }

        public class OrderedGuid
        {
            public Guid Guid
            {
                get;
                set;
            }

            public long Order
            {
                get;
                set;
            }
        }
    }
}
