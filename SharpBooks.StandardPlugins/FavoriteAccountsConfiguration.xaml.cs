using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Collections.ObjectModel;

namespace SharpBooks.StandardPlugins
{
    /// <summary>
    /// Interaction logic for FavoriteAccountsConfiguration.xaml
    /// </summary>
    public partial class FavoriteAccountsConfiguration : Window
    {
        public class AccountView
        {
            public string Name
            {
                get;
                set;
            }

            public bool Favorite
            {
                get;
                set;
            }
        }

        private readonly ObservableCollection<AccountView> accounts = new ObservableCollection<AccountView>();

        public FavoriteAccountsConfiguration()
        {
            InitializeComponent();
        }

        public ObservableCollection<AccountView> Accounts
        {
            get
            {
                return this.accounts;
            }
        }

        public string GetSettings(ReadOnlyBook book, string settings)
        {
            foreach (var a in book.Accounts)
            {
                this.accounts.Add(new AccountView
                {
                    Name = Account.GetAccountPath(a, '\\'),
                    Favorite = false,
                });
            }

            var result = this.ShowDialog();

            if (!result.HasValue || !result.Value)
            {
                return settings;
            }

            var favorites = from a in this.accounts
                            where a.Favorite
                            select a.Name;

            return string.Join(",", favorites.ToArray());
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Hide();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Hide();
        }

        private void This_Loaded(object sender, RoutedEventArgs e)
        {
            this.DialogResult = null;
        }
    }
}
