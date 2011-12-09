﻿//-----------------------------------------------------------------------
// <copyright file="MainController.cs" company="(none)">
//  Copyright © 2010 John Gietzen. All rights reserved.
// </copyright>
// <author>John Gietzen</author>
//-----------------------------------------------------------------------

namespace SharpBooks.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Newtonsoft.Json;
    using SharpBooks.Plugins;
    using SharpBooks.StandardPlugins;
    using System.Windows.Forms;
    using System.IO;

    public class MainController
    {
        private IList<IPluginFactory> plugins;
        private Book book;
        private Account activeAccount;

        public MainController()
        {
            this.plugins = LoadAllPlugins();
        }

        public event EventHandler<EventArgs> ActiveAccountChanged;

        public ReadOnlyBook Book
        {
            get
            {
                return this.book.AsReadOnly();
            }
        }

        public Account ActiveAccount
        {
            get
            {
                return this.activeAccount;
            }

            private set
            {
                if (this.activeAccount != value)
                {
                    this.activeAccount = value;

                    this.ActiveAccountChanged.SafeInvoke(this, () => new EventArgs());
                }
            }
        }

        private static IList<IPluginFactory> LoadAllPlugins()
        {
            var assembly = System.Reflection.Assembly.GetEntryAssembly();

            if (assembly != null)
            {
                var appPath = Path.GetDirectoryName(assembly.Location);

                return PluginLoader.LoadAllPlugins(appPath);
            }
            else
            {
                return null;
            }
        }

        private void AccountSelected(object sender, AccountSelectedEventArgs e)
        {
            this.ActiveAccount = this.Book.Accounts.Where(a => a.AccountId == e.AccountId).SingleOrDefault();
        }

        private void AccountDeselected(object sender, EventArgs e)
        {
            this.ActiveAccount = null;
        }

        public IList<IPersistenceStrategyFactory> GetPersistenceStrategies()
        {
            return (from p in this.plugins
                    let ps = p as IPersistenceStrategyFactory
                    where ps != null
                    select ps).ToList();
        }

        ////public Overview GetOverview()
        ////{
        ////    List<OverviewColumnJson> widgetsLayout = null;
        ////    try
        ////    {
        ////        string settings;
        ////        this.Book.Settings.TryGetValue("overview-layout", out settings);
        ////        widgetsLayout = JsonConvert.DeserializeObject<List<OverviewColumnJson>>(settings);
        ////    }
        ////    catch (JsonReaderException)
        ////    {
        ////    }
        ////
        ////    if (widgetsLayout == null)
        ////    {
        ////        widgetsLayout = new List<OverviewColumnJson>();
        ////        widgetsLayout.Add(new OverviewColumnJson
        ////        {
        ////            ColumnWidth = 200,
        ////            Widgets = new List<OverviewColumnJson.WidgetDescriptionJson>(),
        ////        });
        ////    }
        ////
        ////    return new Overview();
        ////}
        ////
        ////public IEnumerable<IWidget> LoadWidgets()
        ////{
        ////    if (this.book == null)
        ////    {
        ////        return null;
        ////    }
        ////
        ////    int columnNumber = 0;
        ////    foreach (var widgetColumn in widgetsLayout)
        ////    {
        ////        var panel = new StackPanel
        ////        {
        ////            Orientation = Orientation.Vertical
        ////        };
        ////
        ////        if (widgetColumn.Widgets != null)
        ////        {
        ////            foreach (var widgetDescr in widgetColumn.Widgets)
        ////            {
        ////                var widget = this.LoadWidget(widgetDescr.Name);
        ////                widget.IsExpanded = widgetDescr.IsExpanded;
        ////                panel.Children.Add(widget);
        ////            }
        ////        }
        ////
        ////        OverviewGrid.ColumnDefinitions.Add(new ColumnDefinition
        ////        {
        ////            MinWidth = 100.0d,
        ////            Width = new GridLength(widgetColumn.ColumnWidth, GridUnitType.Pixel),
        ////        });
        ////
        ////        var splitter = new GridSplitter
        ////        {
        ////            Width = 3.0d,
        ////        };
        ////
        ////        Grid.SetColumn(panel, columnNumber);
        ////        Grid.SetColumn(splitter, columnNumber);
        ////
        ////        OverviewGrid.Children.Add(panel);
        ////        OverviewGrid.Children.Add(splitter);
        ////
        ////        columnNumber++;
        ////    }
        ////
        ////    OverviewGrid.ColumnDefinitions.Add(new ColumnDefinition
        ////    {
        ////        Width = new GridLength(1.0d, GridUnitType.Auto),
        ////    });
        ////
        ////    return null;
        ////}
        ////
        ////private WidgetContainer LoadWidget(string widgetName)
        ////{
        ////    var widgetKey = "overview-widgets-" + widgetName;
        ////
        ////    string factoryName;
        ////    string factoryType;
        ////    string widgetSettings;
        ////
        ////    this.ReadOnlyBook.Settings.TryGetValue(widgetKey + "-name", out factoryName);
        ////    this.ReadOnlyBook.Settings.TryGetValue(widgetKey + "-type", out factoryType);
        ////    this.ReadOnlyBook.Settings.TryGetValue(widgetKey + "-settings", out widgetSettings);
        ////
        ////    if (!string.IsNullOrEmpty(factoryName) && !string.IsNullOrEmpty(factoryType))
        ////    {
        ////        var factory = (from p in this.plugins
        ////                       let w = p as IWidgetFactory
        ////                       where w != null
        ////                       where w.GetType().AssemblyQualifiedName == factoryType
        ////                       where w.Name == factoryName
        ////                       select w).SingleOrDefault();
        ////
        ////        var widget = factory.CreateInstance(this.ReadOnlyBook, widgetSettings);
        ////
        ////        var events = new EventProxy(
        ////            this.AccountSelected);
        ////
        ////        return new WidgetContainer
        ////        {
        ////            Widget = widget.Create(this.ReadOnlyBook, events),
        ////            Title = factoryName,
        ////        };
        ////    }
        ////    else
        ////    {
        ////        return new WidgetContainer
        ////        {
        ////            Widget = new Label
        ////            {
        ////                Content = "Failed to load " + widgetName,
        ////            },
        ////            Title = string.IsNullOrEmpty(factoryName) ? widgetName : factoryName,
        ////            HasFailed = true,
        ////        };
        ////    }
        ////}
        ////
        ////private class OverviewColumnJson
        ////{
        ////    public int ColumnWidth { get; set; }
        ////
        ////    public List<WidgetDescriptionJson> Widgets { get; set; }
        ////
        ////    public class WidgetDescriptionJson
        ////    {
        ////        public string Name { get; set; }
        ////
        ////        public bool IsExpanded { get; set; }
        ////    }
        ////}

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
            var view = new MainView(this);
            view.AccountSelected += AccountSelected;
            view.AccountDeselected += AccountDeselected;

            Application.Run(view);
        }

        public void New()
        {
        }

        public void Open(IPersistenceStrategyFactory factory)
        {
        }

        public void Close()
        {
        }

        public void Save()
        {
        }

        public void SaveAs(IPersistenceStrategyFactory factory)
        {
        }
    }
}
