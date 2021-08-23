// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace GnuCashIntegration
{
    using System;
    using System.Collections.Generic;
    using SharpBooks;
    using SharpBooks.Plugins;

    /// <summary>
    /// Provides functionality to import data from GNUCash.
    /// </summary>
    public sealed class GnuCashDataSource : IExternalSecuritySource, IExternalAccountSource, IExternalTransactionSource, IExternalSplitSource
    {
        /// <inheritdoc/>
        public void Dispose()
        {
        }

        /// <inheritdoc/>
        public IEnumerable<AccountData> GetAccountData()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public IEnumerable<SecurityData> GetSecurityData()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public IEnumerable<SplitData> GetSplitData()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public IEnumerable<TransactionData> GetTransactionData()
        {
            throw new NotImplementedException();
        }
    }
}
