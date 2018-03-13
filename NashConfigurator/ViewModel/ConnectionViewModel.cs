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
        private Connection connection;
        private IDialogCoordinator dialogCoordinator;

        public event PropertyChangedEventHandler PropertyChanged;

        public string Hostname {
            get => connection.Hostname;
            set {
                connection.Hostname = value;
                RaiseNotifyPropertyChanged("Hostname");
            }
        }
        public string Database {
            get => connection.Database;
            set {
                connection.Database = value;
                RaiseNotifyPropertyChanged("Database");
            }
        }

        private List<string> hostnames = new List<string>();
        public List<string> Hostnames {
            get => hostnames;
            set {
                hostnames = value;
                RaiseNotifyPropertyChanged("Hostnames");
            }
        }

        private List<string> databases = new List<string>();
        public List<string> Databases {
            get => databases;
            set {
                databases = value;
                RaiseNotifyPropertyChanged("Hostnames");
            }
        }

        private ICommand testCommand;
        public ICommand TestCommand {
            get => testCommand ?? (testCommand = new RelayCommand((args) => OnTest()));
        }

        private ICommand saveCommand;
        public ICommand SaveCommand {
            get => saveCommand ?? (saveCommand = new RelayCommand((args) => OnSave()));
        }

        public ConnectionViewModel(IDialogCoordinator instance)
        {
            connection = new Connection();
            dialogCoordinator = instance;
        }

        public async Task OnLoaded()
        {
            try {
                Connection connection = Connection.Load();

                Hostname = connection.Hostname;
                Database = connection.Database;
            } catch(FileNotFoundException ex) { 
            } catch {
                await dialogCoordinator.ShowMessageAsync(this, "Error", "Failed to load configuration!");
            }

            Hostnames.AddRange(await HostnameResolver.GetHostnameListAsync());
        }

        private async Task OnTest()
        {
            var controller = await dialogCoordinator.ShowProgressAsync(this, "Please wait...", "Attempting to connect to database");
            controller.SetIndeterminate();

            try {
                
                await connection.ConnectAsync();
            } catch {
                await controller.CloseAsync();
                await dialogCoordinator.ShowMessageAsync(this, ":(", "Failed to connect.");
            }

            await controller.CloseAsync();
        }
        private void OnSave() {
            connection.Save();
        }
        private void RaiseNotifyPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
