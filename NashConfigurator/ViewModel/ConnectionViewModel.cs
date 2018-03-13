using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

using MahApps.Metro.Controls.Dialogs;

using NashConfigurator.Model;
using NashConfigurator.Helper;

namespace NashConfigurator.ViewModel
{
    public class ConnectionViewModel : INotifyPropertyChanged
    {
        private IDialogCoordinator dialogCoordinator;

        private List<string> hostnames = new List<string>();
        private Connection connection = new Connection();
        private ICommand testCommand;
        private ICommand saveCommand;

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Sql Hostname
        /// </summary>
        public string Hostname {
            get => connection.Hostname;
            set {
                connection.Hostname = value;
                RaiseNotifyPropertyChanged("Hostname");
            }
        }

        /// <summary>
        /// Sql Database
        /// </summary>
        public string Database {
            get => connection.Database;
            set {
                connection.Database = value;
                RaiseNotifyPropertyChanged("Database");
            }
        }

        /// <summary>
        /// Availible Hostnames
        /// </summary>
        public List<string> Hostnames {
            get => hostnames;
            set {
                hostnames = value;
                RaiseNotifyPropertyChanged("Hostnames");
            }
        }

        /// <summary>
        /// Sql Connection
        /// </summary>
        public Connection Connection {
            get => connection;
            set {
                connection = value;
                Hostname = value.Hostname;
                Database = value.Database;
                RaiseNotifyPropertyChanged("Connection");
            }
        }

        /// <summary>
        /// Bit flag set by ConnectionView.xaml to indicate presence of UI
        /// </summary>
        public bool Registered { get; set; }

        public ICommand TestCommand {
            get => testCommand ?? (testCommand = new AsyncRelayCommand(async (args) => await Connect()));
        }

        public ICommand SaveCommand {
            get => saveCommand ?? (saveCommand = new AsyncRelayCommand(async (args) => await OnSave()));
        }

        public ConnectionViewModel(IDialogCoordinator instance)
        {
            dialogCoordinator = instance;
        }

        /// <summary>
        /// Connect to MSSQL database and display dialogs along the way.
        /// </summary>
        /// <returns></returns>
        public async Task<bool> Connect(bool show_success = true)
        {
            // Check if ConnectionViewModel is registered with MahApps.DialogCoordinator
            while (!Registered)
                await Task.Delay(1000);

            // Show progress dialog
            var controller = await dialogCoordinator.ShowProgressAsync(this, "Please wait...", "Attempting to connect to database");
            controller.SetIndeterminate();

            try {
                // Attempt to create sql connection
                await connection.ConnectAsync();
            } catch {
                // Failed..
                await controller.CloseAsync();
                await dialogCoordinator.ShowMessageAsync(this, ":(", "Failed to connect.");

                return false;
            }
            
            await controller.CloseAsync();

            if (show_success) {
                await dialogCoordinator.ShowMessageAsync(this, "Success!", $"Connected to {Hostname}!");
            }

            return true;
        }

        /// <summary>
        /// Save Connection configuration to disk.
        /// </summary>
        private async Task OnSave()
        {
            try {
                connection.Save();
            } catch(Exception ex) {
                await dialogCoordinator.ShowMessageAsync(this, ":(", "Failed to save configuration!");
                AppController.Instance.Log.Error(ex);
            }
        }

        /// <summary>
        /// RaiseNotifyPropertyChanged
        /// </summary>
        /// <param name="property"></param>
        private void RaiseNotifyPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
