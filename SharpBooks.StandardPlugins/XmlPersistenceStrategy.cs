namespace SharpBooks.StandardPlugins
{
    using System;
    using System.Linq;
    using System.Xml.Linq;

    public class XmlPersistenceStrategy : FilePersistenceStrategy
    {
        protected override Book Load(Uri uri)
        {
            var path = uri.LocalPath;

            var book = new Book();

            return book;
        }

        protected override void Save(Book book, Uri uri)
        {
            var path = uri.LocalPath;

            var root =
                new XElement("Book",
                    new XElement("Securities",
                        from s in book.Securities
                        select new XElement("Security",
                            new XAttribute("id", s.SecurityId)
                        )
                    ),
                    new XElement("Accounts",
                        from a in book.Accounts
                        select new XElement("Account",
                            new XAttribute("id", a.AccountId)
                        )
                    ),
                    new XElement("Transactions",
                        from t in book.Transactions
                        select new XElement("Transaction",
                            new XAttribute("id", t.TransactionId)
                        )
                    ),
                    new XElement("PriceQuotes",
                        from p in book.PriceQuotes
                        select new XElement("PriceQuote",
                            new XAttribute("id", p.PriceQuoteId)
                        )
                    )
                );

            var doc = new XDocument(root);

            doc.Save(path, SaveOptions.DisableFormatting);
        }
    }
}
