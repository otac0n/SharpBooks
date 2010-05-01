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
    using SharpBooks.Plugins;
    using SharpBooks.ViewModels;
    using Newtonsoft.Json;

    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : Window
    {
        public MainView()
        {
            InitializeComponent();

            //this.LoadAllWidgets();
        }

        private class OverviewColumn
        {
            public class WidgetDescription
            {
                public string Name { get; set; }
                public bool IsExpanded { get; set; }
            }

            public int ColumnWidth { get; set; }
            public List<WidgetDescription> Widgets { get; set; }
        }

        //private void LoadAllWidgets()
        //{
        //    List<OverviewColumn> widgetsLayout = null;
        //    try
        //    {
        //        widgetsLayout = JsonConvert.DeserializeObject<List<OverviewColumn>>(this.Book.GetSetting("overview-layout"));
        //    }
        //    catch (JsonReaderException)
        //    {
        //    }

        //    if (widgetsLayout == null)
        //    {
        //        widgetsLayout = new List<OverviewColumn>();
        //        widgetsLayout.Add(new OverviewColumn
        //        {
        //            ColumnWidth = 200,
        //            Widgets = null,
        //        });
        //    }

        //    int columnNumber = 0;
        //    foreach (var widgetColumn in widgetsLayout)
        //    {
        //        var panel = new StackPanel
        //        {
        //            Orientation = Orientation.Vertical
        //        };

        //        if (widgetColumn.Widgets != null)
        //        {
        //            foreach (var widgetDescr in widgetColumn.Widgets)
        //            {
        //                var widget = this.LoadWidget(widgetDescr.Name);
        //                widget.IsExpanded = widgetDescr.IsExpanded;
        //                panel.Children.Add(widget);
        //            }
        //        }

        //        OverviewGrid.ColumnDefinitions.Add(new ColumnDefinition
        //        {
        //            MinWidth = 100.0d,
        //            Width = new GridLength(widgetColumn.ColumnWidth, GridUnitType.Pixel),
        //        });

        //        var splitter = new GridSplitter
        //        {
        //            Width = 3.0d,
        //        };

        //        Grid.SetColumn(panel, columnNumber);
        //        Grid.SetColumn(splitter, columnNumber);

        //        OverviewGrid.Children.Add(panel);
        //        OverviewGrid.Children.Add(splitter);

        //        columnNumber++;
        //    }

        //    OverviewGrid.ColumnDefinitions.Add(new ColumnDefinition
        //    {
        //        Width = new GridLength(1.0d, GridUnitType.Auto),
        //    });
        //}

        private void Account_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                var args = new AccountSelectedEventArgs
                {
                    AccountId = (Guid)((StackPanel)sender).Tag,
                };

                //this.AccountSelected(sender, args);

                e.Handled = true;
            }
        }
    }
}
