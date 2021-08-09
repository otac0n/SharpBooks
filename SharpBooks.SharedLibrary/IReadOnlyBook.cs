// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace SharpBooks
{
    using System;
    using System.Collections.Generic;
    using SharpBooks.Events;

    /// <summary>
    /// Encapsulates the read-only functionality of a <see cref="Book"/>.
    /// </summary>
    public interface IReadOnlyBook
    {
        /// <summary>
        /// Raised when an <see cref="Account"/> is added to the book.
        /// </summary>
        event EventHandler<AccountAddedEventArgs> AccountAdded;

        /// <summary>
        /// Raised when an <see cref="Account"/> is removed from the book.
        /// </summary>
        event EventHandler<AccountRemovedEventArgs> AccountRemoved;

        /// <summary>
        /// Raised when a <see cref="PriceQuote"/> is added to the book.
        /// </summary>
        event EventHandler<PriceQuoteAddedEventArgs> PriceQuoteAdded;

        /// <summary>
        /// Raised when a <see cref="PriceQuote"/> is removed from the book.
        /// </summary>
        event EventHandler<PriceQuoteRemovedEventArgs> PriceQuoteRemoved;

        /// <summary>
        /// Raised when a <see cref="Security"/> is added to the book.
        /// </summary>
        event EventHandler<SecurityAddedEventArgs> SecurityAdded;

        /// <summary>
        /// Raised when a <see cref="Security"/> is removed from the book.
        /// </summary>
        event EventHandler<SecurityRemovedEventArgs> SecurityRemoved;

        /// <summary>
        /// Raised when a <see cref="Transaction"/> is added to the book.
        /// </summary>
        event EventHandler<TransactionAddedEventArgs> TransactionAdded;

        /// <summary>
        /// Raised when a <see cref="Transaction"/> is removed from the book.
        /// </summary>
        event EventHandler<TransactionRemovedEventArgs> TransactionRemoved;

        /// <summary>
        /// Gets a collection containing the <see cref="Account">Accounts</see> in the book.
        /// </summary>
        ICollection<Account> Accounts { get; }

        /// <summary>
        /// Gets a collection containing the <see cref="PriceQuote">PriceQuotes</see> in the book.
        /// </summary>
        ICollection<PriceQuote> PriceQuotes { get; }

        /// <summary>
        /// Gets a collection containing the <see cref="Account">Accounts</see> in the book with no parents.
        /// </summary>
        ICollection<Account> RootAccounts { get; }

        /// <summary>
        /// Gets a collection containing the <see cref="Security">Securities</see> in the book.
        /// </summary>
        ICollection<Security> Securities { get; }

        /// <summary>
        /// Gets a collection containing the settings in the book.
        /// </summary>
        ReadOnlyDictionary<string, string> Settings { get; }

        /// <summary>
        /// Gets a collection containing the <see cref="Transaction">Transactions</see> in the book.
        /// </summary>
        ICollection<Transaction> Transactions { get; }

        /// <summary>
        /// Returns a read-only view of this book.
        /// </summary>
        /// <returns>A read-only wrapper for this book.</returns>
        IReadOnlyBook AsReadOnly();

        /// <summary>
        /// Creates a <see cref="SavePoint"/> which can be used to replay changes to the book against a data adapter.
        /// </summary>
        /// <returns>The requested <see cref="SavePoint"/>.</returns>
        SavePoint CreateSavePoint();

        /// <summary>
        /// Gets the balance for the specified <see cref="Account"/>.
        /// </summary>
        /// <param name="account">The <see cref="Account"/> for which the balance will be calculated.</param>
        /// <returns>The balance of the specified <see cref="Account"/>.</returns>
        CompositeBalance GetAccountBalance(Account account);

        /// <summary>
        /// Gets a collection containing all of the <see cref="Split">Splits</see> in the specified <see cref="Account"/>.
        /// </summary>
        /// <param name="account">The <see cref="Account"/> for which the <see cref="Split">Splits</see> will be collected.</param>
        /// <returns>A collection containing the requested <see cref="Split">Splits</see>.</returns>
        ICollection<Split> GetAccountSplits(Account account);

        /// <summary>
        /// Gets the balance for the specified <see cref="Account"/> and all child accounts.
        /// </summary>
        /// <param name="account">The <see cref="Account"/> for which the balance will be calculated.</param>
        /// <returns>The balance of the specified <see cref="Account"/> and all child accounts.</returns>
        CompositeBalance GetAccountTotalBalance(Account account);

        /// <summary>
        /// Replays the changes since the <see cref="SavePoint"/> to the specified <see cref="ISaver">data adapter</see>.
        /// </summary>
        /// <param name="dataAdapter">The data adapter that will accept the replayed changes.</param>
        /// <param name="savePoint">The <see cref="SavePoint"/> for which all changes after will be replayed.</param>
        void Replay(ISaver dataAdapter, SavePoint savePoint = null);
    }
}
