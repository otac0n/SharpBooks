//-----------------------------------------------------------------------
// <copyright file="IDataAdapter.cs" company="Microsoft">
//  Copyright (c) 2010 Microsoft
// </copyright>
// <author>otac0n</author>
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
