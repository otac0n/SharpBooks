// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace SharpBooks
{
    using System;
    using SharpBooks.Persistence;

    public class PersistenceMethod
    {
        private readonly IPersistenceStrategy strategy;
        private Uri uri;

        public PersistenceMethod(IPersistenceStrategy strategy, Uri uri)
        {
            this.strategy = strategy;
            this.uri = uri;
        }

        public IPersistenceStrategy Strategy
        {
            get { return this.strategy; }
        }

        public Uri Uri
        {
            get { return this.uri; }

            set { this.uri = value; }
        }
    }
}
