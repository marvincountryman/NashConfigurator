using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.ComponentModel;

using NashConfigurator.Model;
using NashConfigurator.Helper;

using MahApps.Metro.Controls.Dialogs;

namespace NashConfigurator.ViewModel
{
    public class CompanyViewModel : INotifyPropertyChanged
    {
        private IDialogCoordinator dialogCoordinator;
        private List<Company> companies = new List<Company>();
        private Company company;

        public event PropertyChangedEventHandler PropertyChanged;
        
        public Company Company {
            get => company;
            set {
                company = value;
                CloseDate = value.CloseDate;
                FiscalDate = value.FiscalDate;

                RaisePropertyChanged("Company");
            }
        }
        public DateTime CloseDate { get; set; }
        public DateTime FiscalDate { get; set; }
        public List<Company> Companies {
            get => companies;
            set {
                companies = value;
                RaisePropertyChanged("Companies");
            }
        }

        private ICommand nextCommand;
        public ICommand NextCommand {
            get => nextCommand ?? (nextCommand = new AsyncRelayCommand(async (args) => await OnNext()));
        }

        private ICommand lastCommand;
        public ICommand LastCommand {
            get => lastCommand ?? (lastCommand = new AsyncRelayCommand(async (args) => await OnLast()));
        }

        private ICommand closeCommand;
        public ICommand CloseCommand {
            get => closeCommand ?? (closeCommand = new AsyncRelayCommand(async (args) => await OnClose()));
        }

        private async Task<bool> Confirm()
        {
            return await dialogCoordinator.ShowInputAsync(this, "Are you sure?", "Type company name to confirm.") == Company.Name;
        }

        private async Task OnNext()
        {
            if (await Confirm()) {
                FiscalDate.AddDays(1);
            }
        }
        private async Task OnLast() {
            if (await Confirm()) {
                FiscalDate.AddDays(-1);
            }
        }
        private async Task OnClose() {
            if (await Confirm()) {
                CloseDate = DateTime.Now;
            }
        }

        public CompanyViewModel(IDialogCoordinator instance)
        {
            dialogCoordinator = instance;
        }

        private void RaisePropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
