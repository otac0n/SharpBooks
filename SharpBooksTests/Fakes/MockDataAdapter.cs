//-----------------------------------------------------------------------
// <copyright file="MockDataAdapter.cs" company="(none)">
//  Copyright © 2010 John Gietzen
// </copyright>
// <author>John Gietzen</author>
//-----------------------------------------------------------------------

namespace SharpBooks.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using SharpBooks.Plugins;

    internal class MockDataAdapter : IDataAdapter
    {
        private readonly List<OrderedGuid> securityAdditions = new List<OrderedGuid>();

        private readonly List<OrderedGuid> securityRemovals = new List<OrderedGuid>();

        private readonly List<OrderedGuid> priceQuoteAdditions = new List<OrderedGuid>();

        private readonly List<OrderedGuid> priceQuoteRemovals = new List<OrderedGuid>();

        private readonly List<OrderedGuid> accountAdditions = new List<OrderedGuid>();

        private readonly List<OrderedGuid> accountRemovals = new List<OrderedGuid>();

        private readonly List<OrderedGuid> transactionAdditions = new List<OrderedGuid>();

        private readonly List<OrderedGuid> transactionRemovals = new List<OrderedGuid>();

        private long orderIndex;

        public string Name
        {
            get
            {
                return "Mock Data Adapter";
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

        public void AddSecurity(SecurityData security)
        {
            this.securityAdditions.Add(new OrderedGuid
            {
                Guid = security.SecurityId,
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

        public void AddAccount(AccountData account)
        {
            this.accountAdditions.Add(new OrderedGuid
            {
                Guid = account.AccountId,
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

        public void AddTransaction(TransactionData transaction)
        {
            this.transactionAdditions.Add(new OrderedGuid
            {
                Guid = transaction.TransactionId,
                Order = this.orderIndex++,
            });
        }

        public void RemoveTransaction(Guid transactionId)
        {
            this.transactionRemovals.Add(new OrderedGuid
            {
                Guid = transactionId,
                Order = this.orderIndex++,
            });
        }

        public class OrderedGuid
        {
            public long Order
            {
                get;
                set;
            }

            public Guid Guid
            {
                get;
                set;
            }
        }

        public class OrderedSecurityId
        {
            public long Order
            {
                get;
                set;
            }

            public SecurityType SecurityType
            {
                get;
                set;
            }

            public string Symbol
            {
                get;
                set;
            }
        }

        public class OrderedPriceQuoteId
        {
            public long Order
            {
                get;
                set;
            }

            public DateTime SecurityType
            {
                get;
                set;
            }

            public string Symbol
            {
                get;
                set;
            }
        }
    }
}
