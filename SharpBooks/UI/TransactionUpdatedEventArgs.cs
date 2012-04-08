//-----------------------------------------------------------------------
// <copyright file="TransactionUpdatedEventArgs.cs" company="(none)">
//  Copyright © 2012 John Gietzen. All rights reserved.
// </copyright>
// <author>John Gietzen</author>
//-----------------------------------------------------------------------

namespace SharpBooks.UI
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class TransactionUpdatedEventArgs : EventArgs
    {
        private readonly Transaction oldTransaction;
        private readonly Transaction newTransaction;

        public TransactionUpdatedEventArgs(Transaction oldTransaction, Transaction newTransaction)
        {
            if (oldTransaction == null)
            {
                throw new ArgumentNullException("oldTransaction");
            }

            if (newTransaction == null)
            {
                throw new ArgumentNullException("newTransaction");
            }

            this.oldTransaction = oldTransaction;
            this.newTransaction = newTransaction;
        }

        public Transaction OldTransaction
        {
            get { return this.oldTransaction; }
        }

        public Transaction NewTransaction
        {
            get { return this.newTransaction; }
        }
    }
}
