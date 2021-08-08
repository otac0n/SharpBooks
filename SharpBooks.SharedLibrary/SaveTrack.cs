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

    /// <summary>
    /// Holds a list of changes made to a book from a save point.
    /// </summary>
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
            /// Corresponds to a call to SetSetting.
            /// </summary>
            SetSetting,

            /// <summary>
            /// Corresponds to a call to RemoveSetting.
            /// </summary>
            RemoveSetting,

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

        /// <summary>
        /// Adds an account to the <see cref="SaveTrack"/>.
        /// </summary>
        /// <param name="account">The account to add.</param>
        public void AddAccount(AccountData account)
        {
            this.actions.Add(new Action
            {
                ActionType = ActionType.AddAccount,
                Item = account,
            });
        }

        /// <summary>
        /// Adds a price quote to the <see cref="SaveTrack"/>.
        /// </summary>
        /// <param name="priceQuote">The price quote to add.</param>
        public void AddPriceQuote(PriceQuoteData priceQuote)
        {
            this.actions.Add(new Action
            {
                ActionType = ActionType.AddPriceQuote,
                Item = priceQuote,
            });
        }

        /// <summary>
        /// Adds a security to the <see cref="SaveTrack"/>.
        /// </summary>
        /// <param name="security">The security to add.</param>
        public void AddSecurity(SecurityData security)
        {
            this.actions.Add(new Action
            {
                ActionType = ActionType.AddSecurity,
                Item = security,
            });
        }

        /// <summary>
        /// Adds a transaction to the <see cref="SaveTrack"/>.
        /// </summary>
        /// <param name="transaction">The transaction to add.</param>
        public void AddTransaction(TransactionData transaction)
        {
            this.actions.Add(new Action
            {
                ActionType = ActionType.AddTransaction,
                Item = transaction,
            });
        }

        /// <summary>
        /// Removes an account from the <see cref="SaveTrack"/>.
        /// </summary>
        /// <param name="accountId">The Account Id of the account to remove.</param>
        public void RemoveAccount(Guid accountId)
        {
            this.actions.Add(new Action
            {
                ActionType = ActionType.RemoveAccount,
                Item = accountId,
            });
        }

        /// <summary>
        /// Removes a price quote from the <see cref="SaveTrack"/>.
        /// </summary>
        /// <param name="priceQuoteId">The Price Quote Id of the price quote to remove.</param>
        public void RemovePriceQuote(Guid priceQuoteId)
        {
            this.actions.Add(new Action
            {
                ActionType = ActionType.RemovePriceQuote,
                Item = priceQuoteId,
            });
        }

        /// <summary>
        /// Removes a security from the <see cref="SaveTrack"/>.
        /// </summary>
        /// <param name="securityId">The Security Id of the security to remove.</param>
        public void RemoveSecurity(Guid securityId)
        {
            this.actions.Add(new Action
            {
                ActionType = ActionType.RemoveSecurity,
                Item = securityId,
            });
        }

        /// <summary>
        /// Removes a setting from the <see cref="SaveTrack"/>.
        /// </summary>
        /// <param name="key">The key of the setting.</param>
        public void RemoveSetting(string key)
        {
            this.actions.Add(new Action
            {
                ActionType = ActionType.RemoveSetting,
                Item = key,
            });
        }

        /// <summary>
        /// Removes a transaction from the <see cref="SaveTrack"/>.
        /// </summary>
        /// <param name="transactionId">The Transaction Id of the transaction to remove.</param>
        public void RemoveTransaction(Guid transactionId)
        {
            this.actions.Add(new Action
            {
                ActionType = ActionType.RemoveTransaction,
                Item = transactionId,
            });
        }

        /// <summary>
        /// Replays the actions in this save track into an <see cref="ISaver"/>.
        /// </summary>
        /// <param name="dataAdapter">The adapter into which to replay the actions.</param>
        public void Replay(ISaver dataAdapter)
        {
            foreach (var action in this.actions)
            {
                switch (action.ActionType)
                {
                    case ActionType.SetSetting:
                        var pair = (KeyValuePair<string, string>)action.Item;
                        dataAdapter.SetSetting(pair.Key, pair.Value);
                        break;

                    case ActionType.RemoveSetting:
                        dataAdapter.RemoveSetting((string)action.Item);
                        break;

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

        /// <summary>
        /// Sets the value of a setting.
        /// </summary>
        /// <param name="key">The key of the setting.</param>
        /// <param name="value">The new value of the setting.</param>
        public void SetSetting(string key, string value)
        {
            this.actions.Add(new Action
            {
                ActionType = ActionType.SetSetting,
                Item = new KeyValuePair<string, string>(key, value),
            });
        }

        /// <summary>
        /// Holds data on a singe, atom action performed against a save track.
        /// </summary>
        private class Action
        {
            /// <summary>
            /// Gets or sets the type of action performed.
            /// </summary>
            public ActionType ActionType
            {
                get;
                set;
            }

            /// <summary>
            /// Gets or sets the key or value of the item affected.
            /// </summary>
            public object Item
            {
                get;
                set;
            }
        }
    }
}
