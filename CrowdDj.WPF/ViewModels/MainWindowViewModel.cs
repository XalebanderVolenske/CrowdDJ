using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrowdDj.BL.Contracts;
using CrowdDj.BL.PoCos;
using CrowdDj.DAL;
using CrowdDj.WPF.WindowControllers;

namespace CrowdDj.WPF.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        private Party _selectedParty;
        public RelayCommand CommandGuestLogin { get; set; }

        public MainWindowViewModel() : base(null)
        {
        }

        public MainWindowViewModel(IWindowController windowController) : base(windowController)
        {
            Parties = new ObservableCollection<Party>();
            LoadData();
            LoadCommands();
        }

        public MainWindowViewModel(IWindowController windowController, PartyGuest guest) : base(windowController)
        {
            if (guest != null)
            {
                GuestController guestController = new GuestController();
                GuestViewModel viewModel = new GuestViewModel(guestController, partyGuest: guest);
                guestController.ShowWindow(viewModel);
            }
        }

        private void LoadCommands()
        {
            CommandGuestLogin = new RelayCommand(_ => LoginGuest(), _ => true);
        }

        public ObservableCollection<Party> Parties { get; }

        public Party SelectedParty
        {
            get { return _selectedParty; }
            set
            {
                _selectedParty = value;
                OnPropertyChanged();
            }
        }

        public void LoadData()
        {
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                Parties.Clear();
                var parties = unitOfWork.Parties.Get().ToList();
                parties.ForEach(party => Parties.Add(party));
                SelectedParty = Parties.First();
            }
        }

        private void LoginGuest()
        {
            LoginController loginController =
                new LoginController();
            LoginViewModel viewModel =
                new LoginViewModel(loginController, selectedParty:SelectedParty);
            loginController.ShowWindow(viewModel);
        }
    }
}
