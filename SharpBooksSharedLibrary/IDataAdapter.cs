//-----------------------------------------------------------------------
// <copyright file="IDataAdapter.cs" company="(none)">
//  Copyright (c) 2010 John Gietzen
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
        void AddAccount(AccountData account);

        void RemoveAccount(Guid accountId);

        void AddTransaction(TransactionData transaction);

        void RemoveTransaction(Guid transactionId);
    }
}
