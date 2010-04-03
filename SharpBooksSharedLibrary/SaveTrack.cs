//-----------------------------------------------------------------------
// <copyright file="SaveTrack.cs" company="Microsoft">
//  Copyright (c) 2010 Microsoft
// </copyright>
// <author>otac0n</author>
//-----------------------------------------------------------------------

namespace SharpBooks
{
    using System;
    using System.Collections.Generic;
    using SharpBooks.Plugins;

    internal class SaveTrack
    {
        private readonly List<Action> actions = new List<Action>();

        private enum ActionType
        {
            AddSecurity,
            RemoveSecurity,
            AddPriceQuote,
            RemovePriceQuote,
            AddAccount,
            RemoveAccount,
            AddTransaction,
            RemoveTransaction
        }

        public void Replay(ISaver dataAdapter)
        {
            foreach (var action in this.actions)
            {
                switch (action.ActionType)
                {
                    case ActionType.AddSecurity:
                        dataAdapter.AddSecurity((SecurityData)action.Item);
                        break;
                    case ActionType.RemoveSecurity:
                        dataAdapter.RemoveSecurity((Guid)action.Item);
                        break;
                    case ActionType.AddPriceQuote:
                        dataAdapter.AddPriceQuote((PriceQuoteData)action.Item);
                        break;
                    case ActionType.RemovePriceQuote:
                        dataAdapter.RemovePriceQuote((Guid)action.Item);
                        break;
                    case ActionType.AddAccount:
                        dataAdapter.AddAccount((AccountData)action.Item);
                        break;
                    case ActionType.RemoveAccount:
                        dataAdapter.RemoveAccount((Guid)action.Item);
                        break;
                    case ActionType.AddTransaction:
                        dataAdapter.AddTransaction((TransactionData)action.Item);
                        break;
                    case ActionType.RemoveTransaction:
                        dataAdapter.RemoveTransaction((Guid)action.Item);
                        break;
                }
            }
        }

        public void AddSecurity(Security security)
        {
            this.actions.Add(new Action
            {
                ActionType = ActionType.AddSecurity,
                Item = new SecurityData(security),
            });
        }

        public void RemoveSecurity(Security security)
        {
            this.actions.Add(new Action
            {
                ActionType = ActionType.RemoveSecurity,
                Item = security.SecurityId,
            });
        }

        public void AddPriceQuote(PriceQuote priceQuote)
        {
            this.actions.Add(new Action
            {
                ActionType = ActionType.AddPriceQuote,
                Item = new PriceQuoteData(priceQuote),
            });
        }

        public void RemovePriceQuote(PriceQuote priceQuote)
        {
            this.actions.Add(new Action
            {
                ActionType = ActionType.RemovePriceQuote,
                Item = priceQuote.PriceQuoteId,
            });
        }

        public void AddAccount(Account account)
        {
            this.actions.Add(new Action
            {
                ActionType = ActionType.AddAccount,
                Item = new AccountData(account),
            });
        }

        public void RemoveAccount(Account account)
        {
            this.actions.Add(new Action
            {
                ActionType = ActionType.RemoveAccount,
                Item = account.AccountId,
            });
        }

        public void AddTransaction(Transaction transaction)
        {
            this.actions.Add(new Action
            {
                ActionType = ActionType.AddTransaction,
                Item = new TransactionData(transaction),
            });
        }

        public void RemoveTransaction(Transaction transaction)
        {
            this.actions.Add(new Action
            {
                ActionType = ActionType.RemoveTransaction,
                Item = transaction.TransactionId,
            });
        }

        private class Action
        {
            public ActionType ActionType
            {
                get;
                set;
            }

            public object Item
            {
                get;
                set;
            }
        }
    }
}
