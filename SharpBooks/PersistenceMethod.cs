// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace SharpBooks
{
    using System;
    using SharpBooks.Plugins.Persistence;

    public class PersistenceMethod
    {
        private Uri uri;

        public PersistenceMethod(IPersistenceStrategy strategy, Uri uri)
        {
            this.Strategy = strategy;
            this.uri = uri;
        }

        public IPersistenceStrategy Strategy { get; }

        public Uri Uri
        {
            get => this.uri;

            set => this.uri = value;
        }
    }
}
