// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace SharpBooks.Persistence
{
    using System;

    public abstract class SimplePersistenceStrategy : IPersistenceStrategy
    {
        private Uri currentUri;

        protected abstract Book Load(Uri uri);

        protected abstract void Save(Book book, Uri uri);

        public abstract Uri Open(Uri recentUri);

        public abstract Uri SaveAs(Uri recentUri);

        public void SetDestination(Uri destination)
        {
            this.currentUri = destination;
        }

        public void Save(Book book)
        {
            this.Save(book, this.currentUri);
        }

        public Book Load()
        {
            return this.Load(this.currentUri);
        }

        ~SimplePersistenceStrategy()
        {
            this.Dispose(false);
        }

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
