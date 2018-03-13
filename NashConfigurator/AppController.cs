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
        public Connection Connection;
        public CompanyViewModel CompanyViewModel;
        public ConnectionViewModel ConnectionViewModel;

        public async Task Load()
        {
            CompanyViewModel = new CompanyViewModel(DialogCoordinator.Instance);
            ConnectionViewModel = new ConnectionViewModel(DialogCoordinator.Instance);

            Connection = Connection.Load();
            ConnectionViewModel.Connection = Connection;

            // Potential valid connection
            if (!Connection.IsEmpty) {
                // Connect to database
                //if (await ConnectionViewModel.Connect(false)) {
                    // Query dbo.Company
                    //CompanyViewModel.Companies = new List<Company>(Connection.SqlConnection.Query<Company>("SELECT * FROM dbo.Company"));

                    // Our work is complete
                    return;
                //}
            } else {
                // Get Hostname list for configuration
                await Task.Run(() => ConnectionViewModel.Hostnames = HostnameResolver.GetHostnameList());
            }

            // Wait for MainWindow
            while (App.Current.MainWindow == null)
                await Task.Delay(1000);

            // Move to ConnectionView tab page
            (App.Current.MainWindow as MainWindow).SelectedIndex++;
        }

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
    }
}
