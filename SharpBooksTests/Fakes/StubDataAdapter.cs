//-----------------------------------------------------------------------
// <copyright file="StubDataAdapter.cs" company="(none)">
//  Copyright © 2010 John Gietzen
// </copyright>
// <author>John Gietzen</author>
//-----------------------------------------------------------------------

namespace SharpBooks.Tests
{
    using System;
    using SharpBooks.Plugins;

    public class StubDataAdapter : IDataAdapter
    {
        public string Name
        {
            get
            {
                return "Stub Data Adapter";
            }
        }

        public void AddSecurity(SecurityData security)
        {
        }

        public void RemoveSecurity(Guid securityId)
        {
        }

        public void AddAccount(AccountData account)
        {
        }

        public void RemoveAccount(Guid accountId)
        {
        }

        public void AddTransaction(TransactionData transaction)
        {
        }

        public void RemoveTransaction(Guid transactionId)
        {
        }
    }
}
