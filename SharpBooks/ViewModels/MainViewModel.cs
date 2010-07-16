using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpBooks.Plugins;
using System.Windows.Controls;

namespace SharpBooks.ViewModels
{
    internal class MainViewModel : ViewModelBase
    {
        private IEnumerable<IPluginFactory> plugins;

        private Book book;
        private ReadOnlyBook readOnlyBook;

        internal Book Book
        {
            get
            {
                return this.book;
            }

            private set
            {
                this.book = value;

                this.ReadOnlyBook = this.Book.AsReadOnly();
            }
        }

        internal ReadOnlyBook ReadOnlyBook
        {
            get
            {
                return this.readOnlyBook;
            }

            private set
            {
                this.readOnlyBook = value;
            }
        }

        public MainViewModel()
        {
            //this.Book = BuildFakeBook();
            this.plugins = LoadAllPlugins();
        }

        private static IEnumerable<IPluginFactory> LoadAllPlugins()
        {
            var assembly = System.Reflection.Assembly.GetEntryAssembly();

            if (assembly != null)
            {
                var appPath = System.IO.Path.GetDirectoryName(assembly.Location);

                return PluginLoader.LoadAllPlugins(appPath);
            }
            else
            {
                return null;
            }
        }

        private void AccountSelected(object sender, AccountSelectedEventArgs e)
        {
            //AccountsTabItem.IsSelected = true;
            //AccountsList.Visibility = Visibility.Hidden;
            //SplitList.Visibility = Visibility.Visible;

            var account = this.Book.Accounts.Where(a => a.AccountId == e.AccountId).Single();
            //SplitList.DataContext = from s in this.Book.GetAccountSplits(account)
            //                        select new SplitViewModel(s);
        }

        private WidgetContainer LoadWidget(string widgetName)
        {
            var widgetKey = "overview-widgets-" + widgetName;

            var factoryName = this.ReadOnlyBook.GetSetting(widgetKey + "-name");
            var factoryType = this.ReadOnlyBook.GetSetting(widgetKey + "-type");
            var widgetSettings = this.ReadOnlyBook.GetSetting(widgetKey + "-settings");

            if (!string.IsNullOrEmpty(factoryName) && !string.IsNullOrEmpty(factoryType))
            {
                var factory = (from p in this.plugins
                               let w = p as IWidgetFactory
                               where w != null
                               where w.GetType().AssemblyQualifiedName == factoryType
                               where w.Name == factoryName
                               select w).SingleOrDefault();

                var widget = factory.CreateInstance(this.ReadOnlyBook, widgetSettings);

                var events = new EventProxy(
                    this.AccountSelected);

                return new WidgetContainer
                {
                    Widget = widget.Create(this.ReadOnlyBook, events),
                    Title = factoryName,
                };
            }
            else
            {
                return new WidgetContainer
                {
                    Widget = new Label
                    {
                        Content = "Failed to load " + widgetName,
                    },
                    Title = string.IsNullOrEmpty(factoryName) ? widgetName : factoryName,
                    HasFailed = true,
                };
            }
        }
    }
}
