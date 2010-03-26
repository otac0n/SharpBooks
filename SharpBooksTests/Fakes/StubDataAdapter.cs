//-----------------------------------------------------------------------
// <copyright file="StubDataAdapter.cs" company="(none)">
//  Copyright (c) 2010 John Gietzen
// </copyright>
// <author>John Gietzen</author>
//-----------------------------------------------------------------------

namespace SharpBooks.Tests
{
    using System;

    class StubDataAdapter : IDataAdapter
    {
        public void AddSecurity(SecurityData security)
        {
        }

        public void RemoveSecurity(SecurityType securityType, string symbol)
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
