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

using NashConfigurator.ViewModel;

using MahApps.Metro.Controls.Dialogs;

namespace NashConfigurator.View
{
    /// <summary>
    /// Interaction logic for ConfigView.xaml
    /// </summary>
    public partial class CompanyView : UserControl
    {
        public CompanyView()
        {
            InitializeComponent();

            DataContext = AppController.Instance.CompanyViewModel;
        }
    }
}
