// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace SharpBooks
{
    using System;

    public sealed class SavePoint : IDisposable
    {
        internal SavePoint(Book book)
        {
            this.Book = book;
        }

        public Book Book { get; private set; }

        /// <inheritdoc/>
        public void Dispose()
        {
            if (this.Book == null)
            {
                return;
            }

            this.Book.RemoveSavePoint(this);
            this.Book = null;
        }
    }
}
