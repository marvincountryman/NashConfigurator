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

using MahApps.Metro.Controls.Dialogs;
using NashConfigurator.ViewModel;

namespace NashConfigurator.View
{
    /// <summary>
    /// Interaction logic for ConnectionView.xaml
    /// </summary>
    public partial class ConnectionView : UserControl
    {
        public ConnectionView()
        {
            InitializeComponent();

            DataContext = new ConnectionViewModel(DialogCoordinator.Instance);
        }

        private async void onLoaded(object sender, RoutedEventArgs e)
        {
            if (!IsUserVisible())
                return;
            if (DataContext == null)
                return;
            if (!(DataContext is ConnectionViewModel))
                return;

            await (DataContext as ConnectionViewModel).OnLoaded();
        }

        // Source: https://stackoverflow.com/a/1517794
        private bool IsUserVisible()
        {
            if (!IsVisible)
                return false;

            FrameworkElement container = VisualParent as FrameworkElement;

            Rect bounds = TransformToAncestor(container).TransformBounds(new Rect(0.0, 0.0, ActualWidth, ActualHeight));
            Rect rect = new Rect(0.0, 0.0, container.ActualWidth, container.ActualHeight);

            return rect.Contains(bounds.TopLeft) || rect.Contains(bounds.BottomRight);
        }
    }
}
