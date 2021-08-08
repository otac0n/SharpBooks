﻿// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace SharpBooks.Persistence
{
    using System;

    public abstract class SimplePersistenceStrategy : IPersistenceStrategy
    {
        private Uri currentUri;

        protected abstract Book Load(Uri uri);

        protected abstract void Save(Book book, Uri uri);

        /// <inheritdoc/>
        public abstract Uri Open(Uri recentUri);

        /// <inheritdoc/>
        public abstract Uri SaveAs(Uri recentUri);

        /// <inheritdoc/>
        public void SetDestination(Uri destination)
        {
            this.currentUri = destination;
        }

        /// <inheritdoc/>
        public void Save(Book book)
        {
            this.Save(book, this.currentUri);
        }

        /// <inheritdoc/>
        public Book Load()
        {
            return this.Load(this.currentUri);
        }

        ~SimplePersistenceStrategy()
        {
            this.Dispose(false);
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
        }
    }
}
