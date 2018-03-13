using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MahApps.Metro.Controls.Dialogs;

using NashConfigurator.Model;
using NashConfigurator.Helper;
using NashConfigurator.ViewModel;

using Dapper;

namespace NashConfigurator
{
    public class AppController
    {
        public Logger Log;
        public Connection Connection;
        public CompanyViewModel CompanyViewModel;
        public ConnectionViewModel ConnectionViewModel;

        /// <summary>
        /// Loads App resources.  Called on App_Startup
        /// </summary>
        /// <returns></returns>
        public async Task Load()
        {
            Log = new Logger();

            CompanyViewModel = new CompanyViewModel(DialogCoordinator.Instance);
            ConnectionViewModel = new ConnectionViewModel(DialogCoordinator.Instance);

            Connection = Connection.Load();
            ConnectionViewModel.Connection = Connection;

            // Wait for MainWindow
            while (App.Current.MainWindow == null)
                await Task.Delay(1000);

            // Get Hostname list for configuration
            Task.Run(() => ConnectionViewModel.Hostnames = HostnameResolver.GetHostnameList());

            // Potential valid connection
            if (!Connection.IsEmpty) {
                // Connect to database
                if (await ConnectionViewModel.Connect(false)) {
                    return;
                }
            }

            // Move to ConnectionView tab page
            (App.Current.MainWindow as MainWindow).SelectedIndex++;
        }

        #region Singleton
        private static object instanceLock = new object();
        private static AppController instance;
        public static AppController Instance {
            get {
                lock(instanceLock) {
                    if (instance == null)
                        instance = new AppController();

                    return instance;
                }
            }
        }
        #endregion
    }
}
