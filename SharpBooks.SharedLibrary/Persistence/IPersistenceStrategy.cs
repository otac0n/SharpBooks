namespace SharpBooks.Persistence
{
    using System;

    public interface IPersistenceStrategy : ILoader, IDisposable
    {
        Uri Open(Uri recentUri);
        Uri SaveAs(Uri recentUri);

        void SetDestination(Uri destination);
        void Save();
    }
}
