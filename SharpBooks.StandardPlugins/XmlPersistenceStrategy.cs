namespace SharpBooks.StandardPlugins
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;

    public class XmlPersistenceStrategy : FilePersistenceStrategy
    {
        protected override string FileFilter
        {
            get { return "XML Files (*.xml)|*.xml"; }
        }

        protected override Book Load(Uri uri)
        {
            var book = new Book();

            var doc = XDocument.Load(uri.ToString());

            var securities = new Dictionary<Guid, Security>();
            var accounts = new Dictionary<Guid, Account>();

            foreach (var s in doc.Element("Book").Element("Securities").Elements("Security"))
            {
                var f = s.Element("Format");
                var decimalDigits = (int)f.Attribute("decimalDigits");
                var decimalSeparator = (string)f.Attribute("decimalSeparator");
                var groupSeparator = (string)f.Attribute("groupSeparator");
                var groupSizes = ((string)f.Attribute("groupSizes")).Split(',').Select(g => int.Parse(g.Trim()));
                var positiveFormat = (PositiveFormat)Enum.Parse(typeof(PositiveFormat), (string)f.Attribute("positiveFormat"));
                var negativeFormat = (NegativeFormat)Enum.Parse(typeof(NegativeFormat), (string)f.Attribute("negativeFormat"));
                var currencySymbol = (string)f.Attribute("symbol");
                var format = new CurrencyFormat(decimalDigits, decimalSeparator, groupSeparator, groupSizes, currencySymbol, positiveFormat, negativeFormat);

                var securityId = (Guid)s.Attribute("id");
                var securityType = (SecurityType)Enum.Parse(typeof(SecurityType), (string)s.Attribute("type"));
                var name = (string)s.Attribute("name");
                var symbol = (string)s.Attribute("symbol");
                var fractionTraded = (int)s.Attribute("fractionTraded");

                var security = new Security(securityId, securityType, name, symbol, format, fractionTraded);
                securities.Add(security.SecurityId, security);
                book.AddSecurity(security);
            }

            foreach (var a in doc.Element("Book").Element("Accounts").Elements("Account"))
            {
                var accountId = (Guid)a.Attribute("id");
                var accountType = (AccountType)Enum.Parse(typeof(AccountType), (string)a.Attribute("type"));

                Security security = null;
                var securityAttr = a.Attribute("securityId");
                if (securityAttr != null)
                {
                    security = securities[(Guid)securityAttr];
                }

                Account parentAccount = null;
                var parentAttr = a.Attribute("parentAccountId");
                if (parentAttr != null)
                {
                    parentAccount = accounts[(Guid)parentAttr];
                }

                var name = (string)a.Attribute("name");
                var smallestFraction = (int?)a.Attribute("smallestFraction");

                var account = new Account(accountId, accountType, security, parentAccount, name, smallestFraction);
                accounts.Add(account.AccountId, account);
                book.AddAccount(account);
            }

            foreach (var t in doc.Element("Book").Element("Transactions").Elements("Transaction"))
            {
                var transactionId = (Guid)t.Attribute("id");
                var securityId = (Guid)t.Attribute("securityId");
                var security = securities[securityId];
                var date = (DateTime)t.Attribute("date");

                var transaction = new Transaction(transactionId, security);
                transaction.Date = date;

                foreach (var s in t.Elements("Split"))
                {
                    var split = transaction.AddSplit();

                    var accountId = (Guid)s.Attribute("accountId");
                    var account = accounts[accountId];
                    split.Account = account;

                    var splitSecurityId = (Guid?)s.Attribute("securityId");
                    var splitSecurity = securities[splitSecurityId ?? securityId];
                    split.Security = splitSecurity;

                    var amount = (long)s.Attribute("amount");
                    split.Amount = amount;

                    var transactionAmount = (long?)s.Attribute("transactionAmount");
                    split.TransactionAmount = splitSecurity != security ? transactionAmount.Value : transactionAmount ?? amount;

                    var dateCleared = (DateTime?)s.Attribute("dateCleared");
                    split.DateCleared = dateCleared;

                    var reconciled = (bool)s.Attribute("reconciled");
                    split.IsReconciled = reconciled;
                }

                book.AddTransaction(transaction);
            }

            foreach (var p in doc.Element("Book").Element("PriceQuotes").Elements("PriceQuote"))
            {
                var priceQuoteId = (Guid)p.Attribute("id");
                var dateTime = (DateTime)p.Attribute("date");
                var securityId = (Guid)p.Attribute("securityId");
                var security = securities[securityId];
                var quantity = (long)p.Attribute("quantity");
                var currencyId = (Guid)p.Attribute("currencyId");
                var currency = securities[currencyId];
                var price = (long)p.Attribute("price");
                var source = (string)p.Attribute("source");

                var priceQuote = new PriceQuote(priceQuoteId, dateTime, security, quantity, currency, price, source);
                book.AddPriceQuote(priceQuote);
            }

            foreach (var s in doc.Element("Book").Element("Settings").Elements("Setting"))
            {
                var key = (string)s.Attribute("key");
                var value = (string)s.Attribute("value");

                book.SetSetting(key, value);
            }

            return book;
        }

        protected override void Save(Book book, Uri uri)
        {
            var root =
                new XElement("Book",
                    new XElement("Securities",
                        from s in book.Securities
                        select new XElement("Security",
                            new XAttribute("id", s.SecurityId),
                            new XAttribute("type", s.SecurityType),
                            new XAttribute("name", s.Name),
                            new XAttribute("symbol", s.Symbol),
                            new XAttribute("fractionTraded", s.FractionTraded),
                            new XElement("Format",
                                new XAttribute("decimalDigits", s.Format.DecimalDigits),
                                new XAttribute("decimalSeparator", s.Format.DecimalSeparator),
                                new XAttribute("groupSeparator", s.Format.GroupSeparator),
                                new XAttribute("groupSizes", string.Join(",", s.Format.GroupSizes)),
                                new XAttribute("negativeFormat", s.Format.NegativeFormat),
                                new XAttribute("positiveFormat", s.Format.PositiveFormat),
                                new XAttribute("symbol", s.Format.Symbol)
                            )
                        )
                    ),
                    new XElement("Accounts",
                        from a in book.Accounts
                        select new XElement("Account",
                            new XAttribute("id", a.AccountId),
                            new XAttribute("type", a.AccountType),
                            a.Security == null ? null : new XAttribute("securityId", a.Security.SecurityId),
                            a.ParentAccount == null ? null : new XAttribute("parentAccountId", a.ParentAccount.AccountId),
                            new XAttribute("name", a.Name),
                            a.Security == null ? null : new XAttribute("smallestFraction", a.SmallestFraction)
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
                                s.Security != t.BaseSecurity ? new XAttribute("securityId", s.Security.SecurityId) : null,
                                new XAttribute("amount", s.Amount),
                                s.Security != t.BaseSecurity || s.Amount != s.TransactionAmount ? new XAttribute("transactionAmount", s.TransactionAmount) : null,
                                s.DateCleared.HasValue ? new XAttribute("dateCleared", s.DateCleared) : null,
                                new XAttribute("reconciled", s.IsReconciled)
                            )
                        )
                    ),
                    new XElement("PriceQuotes",
                        from p in book.PriceQuotes
                        select new XElement("PriceQuote",
                            new XAttribute("id", p.PriceQuoteId),
                            new XAttribute("date", p.DateTime),
                            new XAttribute("securityId", p.Security.SecurityId),
                            new XAttribute("quantity", p.Quantity),
                            new XAttribute("currencyId", p.Currency.SecurityId),
                            new XAttribute("price", p.Price),
                            new XAttribute("source", p.Source)
                        )
                    ),
                    new XElement("Settings",
                        from s in book.Settings
                        select new XElement("Setting",
                            new XAttribute("key", s.Key),
                            new XAttribute("value", s.Value)
                        )
                    )
                );

            var doc = new XDocument(root);

            doc.Save(uri.LocalPath);
        }
    }
}
