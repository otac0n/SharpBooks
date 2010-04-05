//-----------------------------------------------------------------------
// <copyright file="SavePoint.cs" company="(none)">
//  Copyright © 2010 John Gietzen. All rights reserved.
// </copyright>
// <author>John Gietzen</author>
//-----------------------------------------------------------------------

namespace SharpBooks
{
    using System;

    public sealed class SavePoint : IDisposable
    {
        internal SavePoint(Book book)
        {
            this.Book = book;
        }

        public Book Book
        {
            get;
            private set;
        }

        public void Dispose()
        {
            if (this.Book == null)
            {
                return;
            }

            this.Book.RemoveSavePoint(this);
            this.Book = null;
        }
    }
}
