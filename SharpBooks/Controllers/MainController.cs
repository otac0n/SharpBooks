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
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Windows.Forms;
    using Newtonsoft.Json;
    using SharpBooks.Persistence;
    using SharpBooks.Plugins;
    using SharpBooks.StandardPlugins;

    public class MainController
    {
        private IList<IPluginFactory> plugins;
        private Book book;
        private Account activeAccount;
        private PersistenceMethod currentSaveMethod;

        public MainController()
        {
            this.plugins = LoadAllPlugins();
        }

        public event EventHandler<EventArgs> ActiveAccountChanged;

        public event EventHandler<EventArgs> BookChanged;

        public ReadOnlyBook Book
        {
            get
            {
                return this.book == null
                    ? null
                    : this.book.AsReadOnly();
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

        private void SetBook(Book book)
        {
            if (book != this.book)
            {
                this.ActiveAccount = null;

                this.book = book;
                this.BookChanged.SafeInvoke(this, () => new EventArgs());
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

        private IList<IPersistenceStrategyFactory> GetPersistenceStrategies()
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

        public void AddTransaction(Transaction transaction)
        {
            this.book.AddTransaction(transaction);
        }

        public void UpdateTransaction(Transaction oldTransaction, Transaction newTransaction)
        {
            this.book.ReplaceTransaction(oldTransaction, newTransaction);
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
            this.Close();

            using (var wizard = new NewBookWizard(this))
            {
                var result = wizard.ShowDialog();
                if (result == DialogResult.Cancel)
                {
                    return;
                }

                this.SetBook(wizard.NewBook);
            }
        }

        public bool Open()
        {
            if (!this.Close())
            {
                return false;
            }

            var saveMethod = this.currentSaveMethod;
            while (true)
            {
                var plugins = this.GetPersistenceStrategies();
                using (var selector = new PersistencePluginSelector(plugins))
                {
                    var result = selector.ShowDialog();

                    if (result == DialogResult.Cancel)
                    {
                        return false;
                    }

                    var factory = selector.StrategyFactory;
                    saveMethod = new PersistenceMethod(factory.CreateInstance(), null);
                }

                saveMethod.Uri = saveMethod.Strategy.Open(saveMethod.Uri);
                if (saveMethod.Uri == null)
                {
                    return false;
                }

                try
                {
                    saveMethod.Strategy.SetDestination(saveMethod.Uri);
                    this.SetBook(saveMethod.Strategy.Load());

                    this.currentSaveMethod = saveMethod;
                    return true;
                }
                catch (Exception ex)
                {
                    var result = MessageBox.Show(ex.ToString(), "Error Loading", MessageBoxButtons.RetryCancel);

                    if (result == DialogResult.Cancel)
                    {
                        return false;
                    }
                }
            }
        }

        public bool Close()
        {
            if (this.book != null) // TODO: If the book has unsaved changes.
            {
                var result = MessageBox.Show("Save changes to your current book?", "Unsaved Changes", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                if (result == DialogResult.Cancel)
                {
                    return false;
                }
                else if (result == DialogResult.Yes)
                {
                    var success = this.Save(forceSaveAs: false);

                    if (!success)
                    {
                        return false;
                    }
                }
            }

            this.currentSaveMethod = null;
            this.SetBook(null);
            return true;
        }

        public bool Save(bool forceSaveAs)
        {
            if (this.book == null)
            {
                return false;
            }

            var saveMethod = this.currentSaveMethod;
            while (true)
            {
                if (forceSaveAs || saveMethod == null)
                {
                    var plugins = this.GetPersistenceStrategies();
                    using (var selector = new PersistencePluginSelector(plugins))
                    {
                        var result = selector.ShowDialog();

                        if (result == DialogResult.Cancel)
                        {
                            return false;
                        }

                        var factory = selector.StrategyFactory;
                        saveMethod = new PersistenceMethod(factory.CreateInstance(), null);
                    }

                    saveMethod.Uri = saveMethod.Strategy.SaveAs(saveMethod.Uri);
                    if (saveMethod.Uri == null)
                    {
                        return false;
                    }
                }

                forceSaveAs = true;

                try
                {
                    saveMethod.Strategy.SetDestination(saveMethod.Uri);
                    saveMethod.Strategy.Save(this.book);

                    this.currentSaveMethod = saveMethod;
                    return true;
                }
                catch (Exception ex)
                {
                    var result = MessageBox.Show(ex.ToString(), "Error Saving", MessageBoxButtons.RetryCancel);

                    if (result == DialogResult.Cancel)
                    {
                        return false;
                    }
                }
            }
        }

        public void NewAccount(Guid? parentAccountId)
        {
            var parent = parentAccountId.HasValue ? this.book.Accounts.Where(a => a.AccountId == parentAccountId.Value).Single() : null;
            var newAccount = new Account(
                Guid.NewGuid(),
                AccountType.Balance,
                parent == null ? null : parent.Security,
                parent,
                "New Account",
                parent == null ? null : parent.SmallestFraction);

            using (var editor = new EditAccountView(this, newAccount))
            {
                var result = editor.ShowDialog();
                if (result == DialogResult.Cancel)
                {
                    return;
                }

                this.book.AddAccount(editor.NewAccount);
            }
        }
    }
}
