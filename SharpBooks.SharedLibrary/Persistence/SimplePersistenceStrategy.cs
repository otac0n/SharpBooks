namespace SharpBooks.Persistence
{
    using System;

    public abstract class SimplePersistenceStrategy : IPersistenceStrategy
    {
        private Uri currentUri;
        private Book book;

        protected abstract Book Load(Uri uri);

        protected abstract void Save(Book book, Uri uri);

        public abstract Uri Open(Uri recentUri);

        public abstract Uri SaveAs(Uri recentUri);

        public void SetDestination(Uri destination)
        {
            this.currentUri = destination;
        }

        public void Save()
        {
            this.Save(this.book, this.currentUri);
        }

        public Book Load()
        {
            this.book = this.Load(this.currentUri);
            return this.book;
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
