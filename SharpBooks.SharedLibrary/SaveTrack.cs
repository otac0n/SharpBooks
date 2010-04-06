//-----------------------------------------------------------------------
// <copyright file="SaveTrack.cs" company="(none)">
//  Copyright © 2010 John Gietzen. All rights reserved.
// </copyright>
// <author>John Gietzen</author>
//-----------------------------------------------------------------------

namespace SharpBooks
{
    using System;
    using System.Collections.Generic;

    internal sealed class SaveTrack : ISaver
    {
        /// <summary>
        /// Holds the list of actions performed on the <see cref="SharpBooks.SaveTrack"/>.
        /// </summary>
        private readonly List<Action> actions = new List<Action>();

        /// <summary>
        /// Describes a type of action performed.
        /// </summary>
        private enum ActionType
        {
            /// <summary>
            /// Corresponds to a call to AddSecurity.
            /// </summary>
            AddSecurity,

            /// <summary>
            /// Corresponds to a call to RemoveSecurity.
            /// </summary>
            RemoveSecurity,

            /// <summary>
            /// Corresponds to a call to AddPriceQuote.
            /// </summary>
            AddPriceQuote,

            /// <summary>
            /// Corresponds to a call to RemovePriceQuote.
            /// </summary>
            RemovePriceQuote,

            /// <summary>
            /// Corresponds to a call to AddAccount.
            /// </summary>
            AddAccount,

            /// <summary>
            /// Corresponds to a call to RemoveAccount.
            /// </summary>
            RemoveAccount,

            /// <summary>
            /// Corresponds to a call to AddTransaction.
            /// </summary>
            AddTransaction,

            /// <summary>
            /// Corresponds to a call to RemoveTransaction.
            /// </summary>
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

        public void AddSecurity(SecurityData security)
        {
            this.actions.Add(new Action
            {
                ActionType = ActionType.AddSecurity,
                Item = security,
            });
        }

        public void RemoveSecurity(Guid securityId)
        {
            this.actions.Add(new Action
            {
                ActionType = ActionType.RemoveSecurity,
                Item = securityId,
            });
        }

        public void AddPriceQuote(PriceQuoteData priceQuote)
        {
            this.actions.Add(new Action
            {
                ActionType = ActionType.AddPriceQuote,
                Item = priceQuote,
            });
        }

        public void RemovePriceQuote(Guid priceQuoteId)
        {
            this.actions.Add(new Action
            {
                ActionType = ActionType.RemovePriceQuote,
                Item = priceQuoteId,
            });
        }

        public void AddAccount(AccountData account)
        {
            this.actions.Add(new Action
            {
                ActionType = ActionType.AddAccount,
                Item = account,
            });
        }

        public void RemoveAccount(Guid accountId)
        {
            this.actions.Add(new Action
            {
                ActionType = ActionType.RemoveAccount,
                Item = accountId,
            });
        }

        public void AddTransaction(TransactionData transaction)
        {
            this.actions.Add(new Action
            {
                ActionType = ActionType.AddTransaction,
                Item = transaction,
            });
        }

        public void RemoveTransaction(Guid transactionId)
        {
            this.actions.Add(new Action
            {
                ActionType = ActionType.RemoveTransaction,
                Item = transactionId,
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
