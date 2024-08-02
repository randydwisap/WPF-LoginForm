using FontAwesome.Sharp;
using System.Threading;
using System.Windows.Input;
using WPF_LoginForm.Models;
using WPF_LoginForm.Repositories;

namespace WPF_LoginForm.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private UserAccountModel _currentUserAccount;
        private ViewModelBase _currentChildView;
        private string _caption;
        private IconChar _icon;

        private IUserRepository userRepository;

        public UserAccountModel CurrentUserAccount
        {
            get { return _currentUserAccount; }
            set
            {
                _currentUserAccount = value;
                OnPropertyChanged(nameof(CurrentUserAccount));
            }
        }

        public ViewModelBase CurrentChildView
        {
            get { return _currentChildView; }
            set
            {
                _currentChildView = value;
                OnPropertyChanged(nameof(CurrentChildView));
            }
        }

        public string Caption
        {
            get { return _caption; }
            set
            {
                _caption = value;
                OnPropertyChanged(nameof(Caption));
            }
        }

        public IconChar Icon
        {
            get { return _icon; }
            set
            {
                _icon = value;
                OnPropertyChanged(nameof(Icon));
            }
        }

        public ICommand ShowHomeViewCommand { get; }
        public ICommand ShowCustomerViewCommand { get; }
        public ICommand ShowOrderViewCommand { get; }
        public ICommand ShowDataKaryawanViewCommand { get; }

        public MainViewModel()
        {
            userRepository = new UserRepository();
            CurrentUserAccount = new UserAccountModel();

            ShowHomeViewCommand = new ViewModelCommand(ExecuteShowHomeViewCommand);
            ShowCustomerViewCommand = new ViewModelCommand(ExecuteShowCustomerViewCommand);
            ShowOrderViewCommand = new ViewModelCommand(ExecuteShowOrderViewCommand);
            ShowDataKaryawanViewCommand = new ViewModelCommand(ExecuteShowDataKaryawanViewCommand);

            ExecuteShowHomeViewCommand(null);
            LoadCurrentUserData();
        }

        private void ExecuteShowCustomerViewCommand(object obj)
        {
            CurrentChildView = new CustomerViewModel();
            Caption = "Email";
            Icon = IconChar.EnvelopeCircleCheck;
        }
        private void ExecuteShowDataKaryawanViewCommand(object obj)
        {
            CurrentChildView = new DataKaryawanViewModel();
            Caption = "Data Karyawan";
            Icon = IconChar.User;
        }

        private void ExecuteShowHomeViewCommand(object obj)
        {
            CurrentChildView = new HomeViewModel();
            Caption = "Tracking Surat";
            Icon = IconChar.EnvelopeOpen;
        }

        private void ExecuteShowOrderViewCommand(object obj)
        {
            CurrentChildView = new OrderViewModel();
            Caption = "Agenda";
            Icon = IconChar.Calendar;
        } 
        private void LoadCurrentUserData()
        {
            var user = userRepository.GetByUsername(Thread.CurrentPrincipal.Identity.Name);
            if (user != null)
            {
                CurrentUserAccount.Username = user.Username;
                CurrentUserAccount.DisplayName = $"{user.Name} {user.LastName}";
                CurrentUserAccount.ProfilePicture = user.ProfilePicture; // Sesuaikan dengan properti yang ada di objek user
                CurrentUserAccount.UserRole = user.UserRole; // Sesuaikan dengan properti yang ada di objek user
            }
            else
            {
                CurrentUserAccount.DisplayName = "Invalid user, not logged in";
                // Sembunyikan tampilan anak.
            }
        }
    }
}
