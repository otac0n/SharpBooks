//-----------------------------------------------------------------------
// <copyright file="SavePoint.cs" company="(none)">
//  Copyright (c) 2010 John Gietzen
// </copyright>
// <author>John Gietzen</author>
//-----------------------------------------------------------------------

namespace SharpBooks
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

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
