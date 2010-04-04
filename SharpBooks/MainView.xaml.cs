namespace SharpBooks
{
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
    using System.Windows.Navigation;
    using System.Windows.Shapes;

    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : Window
    {
        public MainView()
        {
            InitializeComponent();

            var factory = new FavoriteAccountsWidgetFactory();
            for (var i = 0; i < 100; i++)
            {
                var widget = factory.CreateInstance(null, null);
                var expander = new Expander
                {
                    Margin = new Thickness(5.0d),
                    Header = factory.Name,
                    Content = widget.Create(null, null)
                };

                StackPanel1.Children.Add(expander);
            }
        }
    }
}
