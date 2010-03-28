//-----------------------------------------------------------------------
// <copyright file="IDataAdapter.cs" company="(none)">
//  Copyright © 2010 John Gietzen
// </copyright>
// <author>John Gietzen</author>
//-----------------------------------------------------------------------

namespace SharpBooks
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public interface IDataAdapter
    {
        void AddSecurity(SecurityData security);

        void RemoveSecurity(SecurityType securityType, string symbol);

        void AddAccount(AccountData account);

        void RemoveAccount(Guid accountId);

        void AddTransaction(TransactionData transaction);

        void RemoveTransaction(Guid transactionId);
    }
}
