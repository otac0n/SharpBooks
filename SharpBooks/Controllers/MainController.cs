namespace SharpBooks.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Newtonsoft.Json;
    using SharpBooks.Plugins;
    using SharpBooks.ViewModels;
    using SharpBooks.StandardPlugins;
    using System.Windows.Forms;

    public class MainController
    {
        private Book book;

        public MainController()
        {
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
                string settings;
                this.Book.Settings.TryGetValue("overview-layout", out settings);
                widgetsLayout = JsonConvert.DeserializeObject<List<OverviewColumnJson>>(settings);
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

        public void Run()
        {
            Application.Run(new MainView(this));
        }
    }
}
