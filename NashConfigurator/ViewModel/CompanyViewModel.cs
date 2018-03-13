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
        private Company company = new Company();
        private List<Company> companies = new List<Company>();
        private ICommand nextCommand;
        private ICommand lastCommand;
        private ICommand closeCommand;

        public event PropertyChangedEventHandler PropertyChanged;
        
        /// <summary>
        /// Selected Company
        /// </summary>
        public Company Company {
            get => company;
            set {
                company = value;
                RaisePropertyChanged("Company");

                if (company != null)
                {
                    CloseDate = value.CloseDate;
                    FiscalDate = value.FiscalDate;
                }
            }
        }

        /// <summary>
        /// Company CloseDate
        /// </summary>
        public DateTime CloseDate {
            get => Company.CloseDate;
            set {
                Company.CloseDate = value;
                RaisePropertyChanged("CloseDate");
            }
        }

        /// <summary>
        /// Company FiscalDate
        /// </summary>
        public DateTime FiscalDate {
            get => Company.FiscalDate;
            set {
                Company.FiscalDate = value;
                RaisePropertyChanged("FiscalDate");
            }
        }

        /// <summary>
        /// Availible Companies
        /// </summary>
        public List<Company> Companies {
            get => companies;
            set {
                if (companies == null || companies.Count == 0)
                    Company = value.First();

                companies = value;
                RaisePropertyChanged("Companies");
            }
        }

        public ICommand NextCommand {
            get => nextCommand ?? (nextCommand = new AsyncRelayCommand(async (args) => await OnNext()));
        }

        public ICommand LastCommand {
            get => lastCommand ?? (lastCommand = new AsyncRelayCommand(async (args) => await OnLast()));
        }

        public ICommand CloseCommand {
            get => closeCommand ?? (closeCommand = new AsyncRelayCommand(async (args) => await OnClose()));
        }

        /// <summary>
        /// Shows confirmation requiring user to input company name.
        /// </summary>
        /// <returns></returns>
        private async Task<bool> Confirm()
        {
            string input = await dialogCoordinator.ShowInputAsync(this, "Are you sure?", "Type company name to confirm.");
            return input.Trim() == Company.Name.Trim();
        }

        /// <summary>
        /// Show confirmation dialog, then increment FiscalDate month
        /// </summary>
        /// <returns></returns>
        private async Task OnNext()
        {
            if (await Confirm()) {
                FiscalDate = FiscalDate.AddMonths(1);

                await UpdateCompany();
            }
        }

        /// <summary>
        /// Show confirmation dialog, then decrement FiscalDate month
        /// </summary>
        /// <returns></returns>
        private async Task OnLast() {
            if (await Confirm()) {
                FiscalDate = FiscalDate.AddMonths(-1);

                await UpdateCompany();
            }
        }

        /// <summary>
        /// Show confirmation dialog, then Close company
        /// </summary>
        /// <returns></returns>
        private async Task OnClose() {
            if (await Confirm()) {
                Company.Closed = true;
                CloseDate = DateTime.Now;

                await UpdateCompany();
            }
        }
        
        /// <summary>
        /// Updates Company record and displays errors if any occur
        /// </summary>
        /// <returns></returns>
        private async Task UpdateCompany()
        {
            try {
                await Company.Update(AppController.Instance.Connection.SqlConnection);
            } catch(Exception ex) {
                await dialogCoordinator.ShowMessageAsync(this, ":(", "Failed to update Company");
                AppController.Instance.Log.Error(ex);
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
