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

namespace SharpBooks
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

        private ObservableCollection<AccountView> accounts = new ObservableCollection<AccountView>();

        public ObservableCollection<AccountView> Accounts
        {
            get
            {
                return this.accounts;
            }
        }

        public FavoriteAccountsConfiguration(ReadOnlyBook book, string settings)
        {
            foreach (var a in book.Accounts)
            {
                this.accounts.Add(new AccountView
                {
                    Name = Account.GetAccountPath(a, '\\'),
                    Favorite = false,
                });
            }

            InitializeComponent();
        }

        
    }
}
