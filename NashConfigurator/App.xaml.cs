using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

namespace NashConfigurator
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        { 
        }

        private async void OnStartup(object sender, StartupEventArgs e)
        {
            await AppController.Instance.Load();
        }
    }
}
