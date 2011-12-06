namespace SharpBooks.StandardPlugins
{
    using System;
    using System.Linq;
    using System.Xml.Linq;

    public class XmlPersistenceStrategy : FilePersistenceStrategy
    {
        protected override Book Load(Uri uri)
        {
            var book = new Book();

            var doc = XDocument.Load(uri.ToString());

            var securities = from s in doc.Element("Book").Element("Securities").Elements("Security")
                             let securityId = (Guid)s.Attribute("id")
                             let securityType = (SecurityType)Enum.Parse(typeof(SecurityType), (string)s.Attribute("type"))
                             let name = (string)s.Attribute("name")
                             let symbol = (string)s.Attribute("symbol")
                             let signFormat = (string)s.Attribute("signFormat")
                             let fractionTraded = (int)s.Attribute("fractionTraded")
                             select new Security(securityId, securityType, name, symbol, signFormat, fractionTraded);

            foreach (var security in securities)
            {
                book.AddSecurity(security);
            }

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
                            new XAttribute("id", s.SecurityId),
                            new XAttribute("type", s.SecurityType),
                            new XAttribute("name", s.Name),
                            new XAttribute("symbol", s.Symbol),
                            new XAttribute("signFormat", s.SignFormat),
                            new XAttribute("fractionTraded", s.FractionTraded)
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
