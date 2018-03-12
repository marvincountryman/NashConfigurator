using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

using NashConfigurator.Model;

namespace NashConfigurator.ViewModel
{
    public class ConnectionViewModel : INotifyPropertyChanged
    {
        private string hostname = "";
        private string database = "";
        private string username = "";
        private string password = "";
        private ISqlAuthenticator authenticator;
        private bool canEditCredentials = true;
        private List<string> hostnames = new List<string>();
        private List<string> databases = new List<string>();
        private ObservableCollection<ISqlAuthenticator> authenticators = new ObservableCollection<ISqlAuthenticator>() {
            new WindowsAuthenticator()
        };

        public event PropertyChangedEventHandler PropertyChanged;

        public string Hostname {
            get => hostname;
            set {
                hostname = value;
                RaiseNotifyPropertyChanged("Hostname");
            }
        }
        public string Database {
            get => database;
            set {
                database = value;
                RaiseNotifyPropertyChanged("Database");
            }
        }
        public string Username {
            get => username;
            set {
                username = value;
                RaiseNotifyPropertyChanged("Username");
            }
        }
        public string Password {
            private get => password;
            set {
                password = value;
                RaiseNotifyPropertyChanged("Password");
            }
        }
        public ISqlAuthenticator Authenticator {
            get => authenticator;
            set {
                authenticator = value;
                authenticator.LoadDefaults();
                Username = authenticator.Username;
                Password = authenticator.Password;
                CanEditCredentials = !authenticator.IsReadonly;
                RaiseNotifyPropertyChanged("Authenticator");
            }
        }

        public bool IsValid {
            get;
            set;
        }
        public bool CanEditCredentials {
            get => canEditCredentials;
            set {
                canEditCredentials = value;
                RaiseNotifyPropertyChanged("CanEditCredentials");
            }
        }

        public List<string> Hostnames {
            get => hostnames;
            set {
                hostnames = value;
                RaiseNotifyPropertyChanged("Hostnames");
            }
        }
        public List<string> Databases {
            get => databases;
            set {
                databases = value;
                RaiseNotifyPropertyChanged("Hostnames");
            }
        }
        public ObservableCollection<ISqlAuthenticator> Authenticators {
            get => authenticators;
            set {
                authenticators = value;
                RaiseNotifyPropertyChanged("Authenticators");
            }
        }

        public ICommand TestCommand;
        public ICommand SaveCommand;
        public ICommand LoadedCommand;

        public ConnectionViewModel()
        {
            TestCommand = new RelayCommand((args) => OnTest());
            SaveCommand = new RelayCommand((args) => OnSave());
            LoadedCommand = new RelayCommand(async (args) => await OnLoaded());
            
        }

        public async Task OnLoaded()
        {
        }

        private void OnTest() { }
        private void OnSave() { }
        private void RaiseNotifyPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
