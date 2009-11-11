using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.IO;
using System.IO.Compression;
using System.Xml;
using System.Globalization;

namespace SharpCash
{
    public class Engine
    {
        public static IEnumerable<Book> Load(string filename)
        {
            XDocument doc = null;

            using (FileStream fs = new FileStream(filename, FileMode.Open))
            {
                using (GZipStream gz = new GZipStream(fs, CompressionMode.Decompress))
                {
                    using (XmlTextReader x = new XmlTextReader(gz))
                    {
                        doc = XDocument.Load(x, LoadOptions.None);
                    }
                }
            }

            var books = LoadBooksIn(doc.Root);

            return books.ToList();
        }

        private static IEnumerable<Book> LoadBooksIn(XElement document)
        {
            return from e in document.Elements()
                   where e.Name.LocalName == "book"
                   select LoadBook(e);
        }

        private static Book LoadBook(XElement book)
        {
            var guid = LoadIdIn(book);
            var commodities = LoadCommodityDbIn(book);
            var priceDb = LoadPriceDbIn(book, commodities);
            var accountDb = LoadAccountDbIn(book, commodities);
            var transactionDb = LoadTransactionDbIn(book, accountDb, commodities);
            var templateDb = LoadTemplateDbIn(book, commodities);
            var scheduleDb = LoadScheduleDbIn(book, templateDb);
            return new Book(guid, commodities, priceDb, accountDb, transactionDb, scheduleDb);
        }

        private static ScheduleDatabase LoadScheduleDbIn(XElement book, Template template)
        {
            var schedules = LoadSchedulesIn(book, template);
            return new ScheduleDatabase(template, schedules);
        }

        private static IEnumerable<Schedule> LoadSchedulesIn(XElement book, Template template)
        {
            return from s in book.Elements()
                   where s.Name.LocalName == "schedxaction"
                   select LoadSchedule(s, template);
        }

        private static Schedule LoadSchedule(XElement schedule, Template template)
        {
            int temp = 0;

            var id = LoadIdIn(schedule);
            var name = LoadStringIn(schedule, "name");
            var enabled = LoadStringIn(schedule, "enabled") == "y";
            var autoCreate = LoadStringIn(schedule, "autoCreate") == "y";
            var autoCreateNotify = LoadStringIn(schedule, "autoCreateNotify") == "y";
            var advanceCreateDays = int.Parse(LoadStringIn(schedule, "advanceCreateDays"));
            var advanceRemindDays = int.Parse(LoadStringIn(schedule, "advanceRemindDays"));
            var instanceCount = int.TryParse(LoadStringIn(schedule, "instanceCount"), out temp) ? (int?)temp : null;
            var start = LoadDateTimeIn(schedule, "start", "gdate");
            var numOccurrences = int.TryParse(LoadStringIn(schedule, "num-occur"), out temp) ? (int?)temp : null;
            var remainingOccurrences = int.TryParse(LoadStringIn(schedule, "rem-occur"), out temp) ? (int?)temp : null;

            return new Schedule();//id, name, enabled, autoCreate, advanceCreateDays, autoCreateNotify, advanceRemindDays, start, numOccurrences, remainingOccurrences);
        }

        private static Template LoadTemplateDbIn(XElement book, CommodityDatabase commodities)
        {
            return (from t in book.Elements()
                    where t.Name.LocalName == "template-transactions"
                    select LoadTemplateDb(t, commodities)).SingleOrDefault();
        }

        private static Template LoadTemplateDb(XElement template, CommodityDatabase commodities)
        {
            var accountDb = LoadAccountDbIn(template, commodities);
            var transactionDb = LoadTransactionDbIn(template, accountDb, commodities);
            return new Template(accountDb, transactionDb);
        }

        private static TransactionDatabase LoadTransactionDbIn(XElement book, AccountDatabase accounts, CommodityDatabase commodities)
        {
            var transactions = LoadTransactionsIn(book, accounts, commodities);
            return new TransactionDatabase(transactions);
        }

        private static IEnumerable<Transaction> LoadTransactionsIn(XElement book, AccountDatabase accounts, CommodityDatabase commodities)
        {
            return from t in book.Elements()
                   where t.Name.LocalName == "transaction"
                   select LoadTransaction(t, accounts, commodities);
        }

        private static Transaction LoadTransaction(XElement transaction, AccountDatabase accounts, CommodityDatabase commodities)
        {
            var id = LoadIdIn(transaction);
            var currency = (from c in transaction.Elements()
                            where c.Name.LocalName == "currency"
                            select LoadCommodity(c)).Single();
            currency = commodities.FindCommodity(currency.Space, currency.Id);
            var datePosted = LoadDateTimeIn(transaction, "date-posted");
            var dateEntered = LoadDateTimeIn(transaction, "date-entered");
            var description = LoadStringIn(transaction, "description");
            
            var trans = new Transaction(id, currency, datePosted, dateEntered, description);

            var splits = LoadSplitsIn((from s in transaction.Elements()
                                       where s.Name.LocalName == "splits"
                                       select s).Single(), accounts, trans);
            
            trans.AddSplits(splits);

            return trans;
        }

        private static IEnumerable<Split> LoadSplitsIn(XElement splits, AccountDatabase accounts, Transaction transaction)
        {
            return from s in splits.Elements()
                   where s.Name.LocalName == "split"
                   select LoadSplit(s, accounts, transaction);
        }

        private static Split LoadSplit(XElement split, AccountDatabase accounts, Transaction transaction)
        {
            var id = LoadIdIn(split);
            var account = LoadIdIn(split, "account").Value;
            var reconciledState = (ReconciledState)Enum.Parse(typeof(ReconciledState), LoadStringIn(split, "reconciled-state"));
            var memo = LoadStringIn(split, "memo");
            var value = LoadValue(LoadStringIn(split, "value"));
            var quantity = LoadValue(LoadStringIn(split, "quantity"));
            return new Split(accounts, transaction, id, account, reconciledState, memo, value, quantity);
        }

        private static AccountDatabase LoadAccountDbIn(XElement book, CommodityDatabase commodities)
        {
            var accountDb = new AccountDatabase();
            var accounts = LoadAccountsIn(book, accountDb, commodities);

            foreach (var account in accounts)
            {
                accountDb.AddAccount(account);
            }

            return accountDb;
        }

        private static IEnumerable<Account> LoadAccountsIn(XElement book, AccountDatabase accounts, CommodityDatabase commodities)
        {
            return from a in book.Elements()
                   where a.Name.LocalName == "account"
                   select LoadAccount(a, accounts, commodities);
        }

        private static Account LoadAccount(XElement account, AccountDatabase accounts, CommodityDatabase commodities)
        {
            var name = (from n in account.Elements()
                        where n.Name.LocalName == "name"
                        select n.Value).SingleOrDefault();

            var id = LoadIdIn(account);

            var type = (from t in account.Elements()
                        where t.Name.LocalName == "type"
                        select t.Value).SingleOrDefault();

            var commodity = (from c in account.Elements()
                             where c.Name.LocalName == "commodity"
                             select LoadCommodity(c)).SingleOrDefault();

            var commodityScu = (from c in account.Elements()
                                where c.Name.LocalName == "commodity-scu"
                                select int.Parse(c.Value)).SingleOrDefault();

            var description = (from d in account.Elements()
                               where d.Name.LocalName == "description"
                               select d.Value).SingleOrDefault();

            var parentId = LoadIdIn(account, "parent");

            if (commodity != null)
            {
                commodity = commodities.FindCommodity(commodity.Space, commodity.Id);
            }

            return new Account(accounts, id, name, type, commodity, commodityScu, description, parentId);
        }

        private static CommodityDatabase LoadCommodityDbIn(XElement book)
        {
            var commodities = LoadCommoditiesIn(book);

            return new CommodityDatabase(commodities);
        }

        private static PriceDatabase LoadPriceDbIn(XElement book, CommodityDatabase commodities)
        {
            return (from p in book.Elements()
                    where p.Name.LocalName == "pricedb"
                    select LoadPriceDb(p, commodities)).SingleOrDefault();
        }

        private static PriceDatabase LoadPriceDb(XElement pricedb, CommodityDatabase commodities)
        {
            var prices = LoadPricesIn(pricedb, commodities);

            return new PriceDatabase(prices);
        }

        private static IEnumerable<Price> LoadPricesIn(XElement pricedb, CommodityDatabase commodities)
        {
            return from p in pricedb.Elements()
                   where p.Name.LocalName == "price"
                   select LoadPrice(p, commodities);
        }

        private static Price LoadPrice(XElement price, CommodityDatabase commodities)
        {
            var id = LoadIdIn(price);

            var commodity = (from c in price.Elements()
                             where c.Name.LocalName == "commodity"
                             select LoadCommodity(c)).Single();

            var currency = (from c in price.Elements()
                            where c.Name.LocalName == "currency"
                            select LoadCommodity(c)).Single();

            var time = LoadDateTimeIn(price, "time");

            var source = LoadStringIn(price, "source");

            var type = LoadStringIn(price, "type");

            var value = (from v in price.Elements()
                         where v.Name.LocalName == "value"
                         select LoadValue(v.Value)).Single();

            commodity = commodities.FindCommodity(commodity.Space, commodity.Id);
            currency = commodities.FindCommodity(currency.Space, currency.Id);

            return new Price(id, commodity, currency, time, source, type, value);
        }

        private static string LoadStringIn(XElement item, string name)
        {
            return (from i in item.Elements()
                    where i.Name.LocalName == name
                    select i.Value).SingleOrDefault();
        }

        private static DateTime LoadDateTimeIn(XElement item, string dateTimeName)
        {
            return LoadDateTimeIn(item, dateTimeName, "date");
        }

        private static DateTime LoadDateTimeIn(XElement item, string dateTimeName, string innerName)
        {
            return (from i in item.Elements()
                    where i.Name.LocalName == dateTimeName
                    select DateTime.ParseExact(i.Elements().Single(d => d.Name.LocalName == innerName).Value, new[] { "yyyy-MM-dd HH:mm:ss zzz", "yyyy-MM-dd" }, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal)).Single();
        }

        private static decimal LoadValue(string value)
        {
            string[] sections = value.Split("/".ToCharArray());

            var numerator = decimal.Parse(sections[0]);
            var denominator = decimal.Parse(sections[1]);

            return numerator / denominator;
        }

        private static Guid LoadIdIn(XElement item)
        {
            return LoadIdIn(item, "id").Value;
        }

        private static Guid? LoadIdIn(XElement item, string elementName)
        {
            return (from i in item.Elements()
                    where i.Name.LocalName == elementName
                    select (Guid?)(new Guid(i.Value))).SingleOrDefault();
        }

        private static IEnumerable<Commodity> LoadCommoditiesIn(XElement book)
        {
            return from c in book.Elements()
                   where c.Name.LocalName == "commodity"
                   select LoadCommodity(c);
        }

        private static Commodity LoadCommodity(XElement commodity)
        {
            var id = LoadStringIn(commodity, "id");
            var space = LoadStringIn(commodity, "space");
            var name = LoadStringIn(commodity, "name");
            var xCode = LoadStringIn(commodity, "xcode");
            var fraction = (from f in commodity.Elements()
                            where f.Name.LocalName == "fraction"
                            select int.Parse(f.Value)).SingleOrDefault();
            var getQuotes = (from q in commodity.Elements()
                             where q.Name.LocalName == "get_quotes"
                             select q).Any();
            var quoteSource = LoadStringIn(commodity, "quote_source");
            var quoteTimeZone = LoadStringIn(commodity, "quote_tz");

            return new Commodity(id, space, name, xCode, fraction, getQuotes, quoteSource, quoteTimeZone);
        }
    }
}