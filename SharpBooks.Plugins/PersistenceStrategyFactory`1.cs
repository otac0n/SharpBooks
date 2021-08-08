// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace SharpBooks.Plugins
{
    using SharpBooks.Persistence;

    public class PersistenceStrategyFactory<T> : IPersistenceStrategyFactory where T : IPersistenceStrategy, new()
    {
        public PersistenceStrategyFactory(string name)
        {
            this.Name = name;
        }

        public string Name
        {
            get;
            private set;
        }

        public IPersistenceStrategy CreateInstance()
        {
            return new T();
        }
    }
}
