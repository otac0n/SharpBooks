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

    public partial class WidgetContainer : UserControl
    {
        public static DependencyProperty TitleProperty =
            DependencyProperty.Register(
                "Title",
                typeof(string),
                typeof(WidgetContainer),
                new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsRender));

        public static DependencyProperty HasFailedProperty =
            DependencyProperty.Register(
                "HasFailed",
                typeof(bool),
                typeof(WidgetContainer),
                new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsRender));

        public static DependencyProperty IsExpandedProperty =
            DependencyProperty.Register(
                "IsExpanded",
                typeof(bool),
                typeof(WidgetContainer),
                new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsRender));

        private static BitmapSource warningIcon =
            System.Windows.Interop.Imaging.CreateBitmapSourceFromHIcon(
                System.Drawing.SystemIcons.Warning.Handle,
                Int32Rect.Empty,
                BitmapSizeOptions.FromWidthAndHeight(16, 16));

        public WidgetContainer()
        {
            InitializeComponent();
        }

        public bool IsExpanded
        {
            get
            {
                return (bool)this.GetValue(IsExpandedProperty);
            }

            set
            {
                this.SetValue(IsExpandedProperty, value);
            }
        }

        public object Widget
        {
            get
            {
                return this.Expander.Content;
            }

            set
            {
                this.Expander.Content = value;
            }
        }

        public BitmapSource WarningIcon
        {
            get
            {
                return warningIcon;
            }
        }

        public string Title
        {
            get
            {
                return (string)this.GetValue(TitleProperty);
            }

            set
            {
                this.SetValue(TitleProperty, value);
            }
        }

        public bool HasFailed
        {
            get
            {
                return (bool)this.GetValue(HasFailedProperty);
            }

            set
            {
                this.SetValue(HasFailedProperty, value);
            }
        }
    }
}
