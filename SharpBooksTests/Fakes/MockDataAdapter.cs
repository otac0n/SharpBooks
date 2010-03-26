//-----------------------------------------------------------------------
// <copyright file="MockDataAdapter.cs" company="(none)">
//  Copyright (c) 2010 John Gietzen
// </copyright>
// <author>John Gietzen</author>
//-----------------------------------------------------------------------

using System.Collections.ObjectModel;

namespace SharpBooks.Tests
{
    using System;
    using System.Collections.Generic;

    internal class MockDataAdapter : IDataAdapter
    {
        private readonly List<OrderedSecurityId> securityAdditions = new List<OrderedSecurityId>();

        private readonly List<OrderedSecurityId> securityRemovals = new List<OrderedSecurityId>();

        private readonly List<OrderedGuid> accountAdditions = new List<OrderedGuid>();

        private readonly List<OrderedGuid> accountRemovals = new List<OrderedGuid>();

        private readonly List<OrderedGuid> transactionAdditions = new List<OrderedGuid>();

        private readonly List<OrderedGuid> transactionRemovals = new List<OrderedGuid>();

        private long orderIndex;

        public ReadOnlyCollection<OrderedSecurityId> SecurityAdditions
        {
            get
            {
                return this.securityAdditions.AsReadOnly();
            }
        }

        public ReadOnlyCollection<OrderedSecurityId> SecurityRemovals
        {
            get
            {
                return this.securityRemovals.AsReadOnly();
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
            this.securityAdditions.Add(new OrderedSecurityId
            {
                Symbol = security.Symbol,
                SecurityType = security.SecurityType,
                Order = this.orderIndex++,
            });
        }

        public void RemoveSecurity(SecurityType securityType, string symbol)
        {
            this.securityRemovals.Add(new OrderedSecurityId
            {
                Symbol = symbol,
                SecurityType = securityType,
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
    }
}
