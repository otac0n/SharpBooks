//-----------------------------------------------------------------------
// <copyright file="PersistenceStrategyFactory`1.cs" company="(none)">
//  Copyright © 2010 John Gietzen. All rights reserved.
// </copyright>
// <author>John Gietzen</author>
//-----------------------------------------------------------------------

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
