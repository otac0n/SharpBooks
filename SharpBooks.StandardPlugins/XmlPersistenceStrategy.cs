namespace SharpBooks.StandardPlugins
{
    using System;
    using System.Linq;
    using System.Xml.Linq;
    using System.Collections.Generic;

    public class XmlPersistenceStrategy : FilePersistenceStrategy
    {
        protected override Book Load(Uri uri)
        {
            var book = new Book();

            var doc = XDocument.Load(uri.ToString());

            var securities = new Dictionary<Guid, Security>();
            var accounts = new Dictionary<Guid, Account>();

            foreach (var s in doc.Element("Book").Element("Securities").Elements("Security"))
            {
                var securityId = (Guid)s.Attribute("id");
                var securityType = (SecurityType)Enum.Parse(typeof(SecurityType), (string)s.Attribute("type"));
                var name = (string)s.Attribute("name");
                var symbol = (string)s.Attribute("symbol");
                var signFormat = (string)s.Attribute("signFormat");
                var fractionTraded = (int)s.Attribute("fractionTraded");

                var security = new Security(securityId, securityType, name, symbol, signFormat, fractionTraded);
                securities.Add(security.SecurityId, security);
                book.AddSecurity(security);
            }

            foreach (var a in doc.Element("Book").Element("Accounts").Elements("Account"))
            {
                var accountId = (Guid)a.Attribute("id");
                var securityId = (Guid)a.Attribute("securityId");
                var security = securities[securityId];
                Account parentAccount = null;
                var parentAttr = a.Attribute("parentAccountId");
                if (parentAttr != null)
                {
                    var parentAccountId = (Guid)parentAttr;
                    parentAccount = accounts[parentAccountId];
                }
                var name = (string)a.Attribute("name");
                var smallestFraction = (int)a.Attribute("smallestFraction");

                var account = new Account(accountId, security, parentAccount, name, smallestFraction);
                accounts.Add(account.AccountId, account);
                book.AddAccount(account);
            }

            foreach (var t in doc.Element("Book").Element("Transactions").Elements("Transaction"))
            {
                var transactionId = (Guid)t.Attribute("id");
                var securityId = (Guid)t.Attribute("securityId");
                var security = securities[securityId];

                var transaction = new Transaction(transactionId, security);
                using (var tlock = transaction.Lock())
                {
                    foreach (var s in t.Elements("Split"))
                    {
                        var split = transaction.AddSplit(tlock);

                        var accountId = (Guid)s.Attribute("accountId");
                        var account = accounts[accountId];
                        split.SetAccount(account, tlock);

                        var amount = (long)s.Attribute("amount");
                        split.SetAmount(amount, tlock);

                        var transactionAmount = (long)s.Attribute("transactionAmount");
                        split.SetTransactionAmount(transactionAmount, tlock);

                        var dateCleared = (DateTime?)s.Attribute("dateCleared");
                        split.SetDateCleared(dateCleared, tlock);

                        var reconciled = (bool)s.Attribute("reconciled");
                        split.SetIsReconciled(reconciled, tlock);
                    }
                }

                book.AddTransaction(transaction);
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
                            new XAttribute("id", a.AccountId),
                            new XAttribute("securityId", a.Security.SecurityId),
                            a.ParentAccount == null ? null : new XAttribute("parentAccountId", a.ParentAccount.AccountId),
                            new XAttribute("name", a.Name),
                            new XAttribute("smallestFraction", a.SmallestFraction)
                        )
                    ),
                    new XElement("Transactions",
                        from t in book.Transactions
                        select new XElement("Transaction",
                            new XAttribute("id", t.TransactionId),
                            new XAttribute("securityId", t.BaseSecurity.SecurityId),
                            new XAttribute("date", t.Date),
                            from s in t.Splits
                            select new XElement("Split",
                                new XAttribute("accountId", s.Account.AccountId),
                                new XAttribute("amount", s.Amount),
                                new XAttribute("transactionAmount", s.TransactionAmount),
                                new XAttribute("dateCleared", s.DateCleared),
                                new XAttribute("reconciled", s.IsReconciled)
                            )
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
