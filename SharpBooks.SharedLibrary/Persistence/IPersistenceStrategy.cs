// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace SharpBooks.Persistence
{
    using System;

    public interface IPersistenceStrategy : ILoader, IDisposable
    {
        Uri Open(Uri recentUri);

        void Save(Book book);

        Uri SaveAs(Uri recentUri);

        void SetDestination(Uri destination);
    }
}
