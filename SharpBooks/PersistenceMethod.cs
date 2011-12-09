﻿//-----------------------------------------------------------------------
// <copyright file="PersistenceMethod.cs" company="(none)">
//  Copyright © 2010 John Gietzen. All rights reserved.
// </copyright>
// <author>John Gietzen</author>
//-----------------------------------------------------------------------

namespace SharpBooks
{
    using System;
    using SharpBooks.Persistence;

    public class PersistenceMethod
    {
        private readonly IPersistenceStrategy strategy;
        private readonly Uri uri;

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
        }
    }
}
