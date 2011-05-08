namespace SharpBooks.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Newtonsoft.Json;
    using SharpBooks.Plugins;
    using SharpBooks.ViewModels;

    public class MainController
    {
        private Book book;

        public MainController()
        {
            this.book = BuildFakeBook();
        }

        public ReadOnlyBook Book
        {
            get
            {
                return this.book.AsReadOnly();
            }
        }

        public Overview GetOverview()
        {
            List<OverviewColumnJson> widgetsLayout = null;
            try
            {
                widgetsLayout = JsonConvert.DeserializeObject<List<OverviewColumnJson>>(this.Book.GetSetting("overview-layout"));
            }
            catch (JsonReaderException)
            {
            }

            if (widgetsLayout == null)
            {
                widgetsLayout = new List<OverviewColumnJson>();
                widgetsLayout.Add(new OverviewColumnJson
                {
                    ColumnWidth = 200,
                    Widgets = new List<OverviewColumnJson.WidgetDescriptionJson>(),
                });
            }

            return new Overview();
        }

        private static Book BuildFakeBook()
        {
            var usd = new Security(Guid.NewGuid(), SecurityType.Currency, "United States dollar", "USD", "{0:$#,##0.00#;($#,##0.00#)}", 1000);
            var account1 = new Account(Guid.NewGuid(), usd, null, "Assets", 100);
            var account2 = new Account(Guid.NewGuid(), usd, account1, "My Bank Account", 100);
            var account3 = new Account(Guid.NewGuid(), usd, account1, "My Other Bank", 100);
            var account4 = new Account(Guid.NewGuid(), usd, null, "Liabilities", 100);
            var account5 = new Account(Guid.NewGuid(), usd, account4, "My Home Loan", 100);

            var transaction1 = new Transaction(Guid.NewGuid(), usd);
            using (var tlock = transaction1.Lock())
            {
                transaction1.SetDate(DateTime.Today.AddDays(-1), tlock);

                var split1 = transaction1.AddSplit(tlock);
                split1.SetAccount(account2, tlock);
                split1.SetAmount(3520, tlock);
                split1.SetTransactionAmount(3520, tlock);
                split1.SetDateCleared(DateTime.Today, tlock);

                var split2 = transaction1.AddSplit(tlock);
                split2.SetAccount(account3, tlock);
                split2.SetAmount(-3520, tlock);
                split2.SetTransactionAmount(-3520, tlock);
            }

            var transaction2 = new Transaction(Guid.NewGuid(), usd);
            using (var tlock = transaction2.Lock())
            {
                transaction2.SetDate(DateTime.Today.AddDays(-2), tlock);

                var split3 = transaction2.AddSplit(tlock);
                split3.SetAccount(account5, tlock);
                split3.SetAmount(1750, tlock);
                split3.SetTransactionAmount(1750, tlock);
                split3.SetDateCleared(DateTime.Today.AddDays(-1), tlock);

                var split4 = transaction2.AddSplit(tlock);
                split4.SetAccount(account2, tlock);
                split4.SetAmount(-1750, tlock);
                split4.SetTransactionAmount(-1750, tlock);
            }

            var book = new Book();
            book.AddSecurity(usd);
            book.AddAccount(account1);
            book.AddAccount(account2);
            book.AddAccount(account3);
            book.AddAccount(account4);
            book.AddAccount(account5);

            book.AddTransaction(transaction1);
            book.AddTransaction(transaction2);

            book.SetSetting("overview-layout", "[{\"ColumnWidth\":200,\"Widgets\":[{\"Name\":\"widget1\"},{\"Name\":\"widget2\"}]},{\"ColumnWidth\":250,\"Widgets\":[{\"Name\":\"widget3\"}]}]");
            book.SetSetting("overview-widgets-widget1-name", "Favorite Accounts");
            book.SetSetting("overview-widgets-widget1-type", "SharpBooks.StandardPlugins.FavoriteAccountsWidgetFactory, SharpBooks.StandardPlugins, Version=1.0.0.0, Culture=neutral, PublicKeyToken=6fee4057cb920410");
            book.SetSetting("overview-widgets-widget1-settings", "{\"PathSeperator\":\"\\\\\",\"AccountPaths\":[\"Assets\\\\My Bank Account\",\"Assets\\\\My Other Bank\",\"Liabilities\\\\My Home Loan\"]}");
            book.SetSetting("overview-widgets-widget3-name", "Recent Expenses");
            book.SetSetting("overview-widgets-widget3-type", "SharpBooks.StandardPlugins.RecentExpensesWidgetFactory, SharpBooks.StandardPlugins, Version=1.0.0.0, Culture=neutral, PublicKeyToken=6fee4057cb920410");

            return book;
        }

        public IEnumerable<IWidget> LoadWidgets()
        {
            if (this.book == null)
            {
                return null;
            }


            ////int columnNumber = 0;
            ////foreach (var widgetColumn in widgetsLayout)
            ////{
            ////    var panel = new StackPanel
            ////    {
            ////        Orientation = Orientation.Vertical
            ////    };
            ////
            ////    if (widgetColumn.Widgets != null)
            ////    {
            ////        foreach (var widgetDescr in widgetColumn.Widgets)
            ////        {
            ////            var widget = this.LoadWidget(widgetDescr.Name);
            ////            widget.IsExpanded = widgetDescr.IsExpanded;
            ////            panel.Children.Add(widget);
            ////        }
            ////    }
            ////
            ////    OverviewGrid.ColumnDefinitions.Add(new ColumnDefinition
            ////    {
            ////        MinWidth = 100.0d,
            ////        Width = new GridLength(widgetColumn.ColumnWidth, GridUnitType.Pixel),
            ////    });
            ////
            ////    var splitter = new GridSplitter
            ////    {
            ////        Width = 3.0d,
            ////    };
            ////
            ////    Grid.SetColumn(panel, columnNumber);
            ////    Grid.SetColumn(splitter, columnNumber);
            ////
            ////    OverviewGrid.Children.Add(panel);
            ////    OverviewGrid.Children.Add(splitter);
            ////
            ////    columnNumber++;
            ////}
            ////
            ////OverviewGrid.ColumnDefinitions.Add(new ColumnDefinition
            ////{
            ////    Width = new GridLength(1.0d, GridUnitType.Auto),
            ////});

            return null;
        }

        private class OverviewColumnJson
        {
            public int ColumnWidth { get; set; }

            public List<WidgetDescriptionJson> Widgets { get; set; }

            public class WidgetDescriptionJson
            {
                public string Name { get; set; }

                public bool IsExpanded { get; set; }
            }
        }

        public void AddAccount(Account account)
        {
            this.book.AddAccount(account);
        }

        internal void AddTransaction(Transaction transaction)
        {
            this.book.AddTransaction(transaction);
        }
    }
}
