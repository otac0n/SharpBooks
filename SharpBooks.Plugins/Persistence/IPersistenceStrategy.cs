// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace SharpBooks.Plugins.Persistence
{
    using System;
    using SharpBooks.Plugins;

    public interface IPersistenceStrategy : ILoader, IPlugin
    {
        Uri Open(Uri recentUri);

        void Save(Book book);

        Uri SaveAs(Uri recentUri);

        void SetDestination(Uri destination);
    }
}
