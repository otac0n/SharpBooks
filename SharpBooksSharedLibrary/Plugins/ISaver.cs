//-----------------------------------------------------------------------
// <copyright file="ISaver.cs" company="(none)">
//  Copyright © 2010 John Gietzen. All rights reserved.
// </copyright>
// <author>John Gietzen</author>
//-----------------------------------------------------------------------

namespace SharpBooks.Plugins
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public interface ISaver
    {
        void AddSecurity(SecurityData security);

        void RemoveSecurity(Guid securityId);

        void AddAccount(AccountData account);

        void RemoveAccount(Guid accountId);

        void AddTransaction(TransactionData transaction);

        void RemoveTransaction(Guid transactionId);

        void AddPriceQuote(PriceQuoteData priceQuote);

        void RemovePriceQuote(Guid priceQuoteId);
    }
}
