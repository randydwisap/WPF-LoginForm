using System.IO;
using FontAwesome.Sharp;
using System;
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

        public static UserAccountModel CurrentUserStatic { get; private set; }

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

        public ICommand ShowSuratMasukViewCommand { get; }
        public ICommand ShowSuratKeluarViewCommand { get; }
        public ICommand ShowDisposisiViewCommand { get; }
        public ICommand ShowMemoViewCommand { get; }
        public ICommand ShowAgendaViewCommand { get; }
        public ICommand ShowDataKaryawanViewCommand { get; }

        public MainViewModel()
        {
            userRepository = new UserRepository();
            CurrentUserAccount = new UserAccountModel();

            ShowSuratMasukViewCommand = new ViewModelCommand(ExecuteShowSuratMasukViewCommand);
            ExecuteShowSuratMasukViewCommand(null);
            ShowSuratKeluarViewCommand = new ViewModelCommand(ExecuteShowSuratKeluarViewCommand);
            ShowDisposisiViewCommand = new ViewModelCommand(ExecuteShowDisposisiViewCommand);
            ShowMemoViewCommand = new ViewModelCommand(ExecuteShowMemoViewCommand);
            ShowAgendaViewCommand = new ViewModelCommand(ExecuteShowAgendaViewCommand);
            ShowDataKaryawanViewCommand = new ViewModelCommand(ExecuteShowDataKaryawanViewCommand);

            LoadCurrentUserData();
            CurrentUserStatic = CurrentUserAccount;

            // Cek Role pengguna setelah memuat data
            if (CurrentUserAccount.Role == "Admin")
            {
                // Tampilkan semua tampilan untuk Admin
            }
            else
            {
                // Nonaktifkan command lainnya jika bukan Admin
                ShowDataKaryawanViewCommand = null;
            }
        }

        public bool IsAdmin
        {
            get { return CurrentUserAccount.Role == "Admin"; }
        }


        private void ExecuteShowMemoViewCommand(object obj)
        {
            CurrentChildView = new MemoViewModel();
            Caption = "Memo";
            Icon = IconChar.EnvelopeCircleCheck;
        }
        private void ExecuteShowDataKaryawanViewCommand(object obj)
        {
            CurrentChildView = new DataKaryawanViewModel();
            Caption = "Data Karyawan";
            Icon = IconChar.User;
        }

        private void ExecuteShowSuratMasukViewCommand(object obj)
        {
            CurrentChildView = new SuratMasukViewModel();
            Caption = "Surat Masuk";
            Icon = IconChar.EnvelopeOpen;
        }

        private void ExecuteShowSuratKeluarViewCommand(object obj)
        {
            CurrentChildView = new SuratKeluarViewModel();
            Caption = "Surat Keluar";
            Icon = IconChar.EnvelopeOpen;
        }

        private void ExecuteShowDisposisiViewCommand(object obj)
        {
            CurrentChildView = new DisposisiViewModel();
            Caption = "Disposisi";
            Icon = IconChar.EnvelopeOpen;
        }


        private void ExecuteShowAgendaViewCommand(object obj)
        {
            CurrentChildView = new AgendaViewModel();
            Caption = "Agenda";
            Icon = IconChar.Calendar;
        }

        private readonly string _imageDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images");

        private void LoadCurrentUserData()
        {
            var user = userRepository.GetByUsername(Thread.CurrentPrincipal.Identity.Name);
            if (user != null)
            {
                CurrentUserAccount.Username = user.Username;
                CurrentUserAccount.Name = $"{user.Name}";
                CurrentUserAccount.NomorHP= $"{user.NomorHP}";
                CurrentUserAccount.UserID = user.UserID;

                // Perbarui ProfilePicture agar menunjuk ke direktori Images
                if (!string.IsNullOrEmpty(user.ProfilePicture))
                {
                    string profilePath = Path.Combine(_imageDirectory, user.ProfilePicture);
                    if (File.Exists(profilePath))
                    {
                        CurrentUserAccount.ProfilePicture = profilePath;
                    }
                    else
                    {
                        CurrentUserAccount.ProfilePicture = "Images/DefaultProfile.png"; // Path ke gambar default
                    }
                }
                else
                {
                    CurrentUserAccount.ProfilePicture = "Images/DefaultProfile.png"; // Path ke gambar default
                }

                CurrentUserAccount.Role = $"{user.Role}";                
                CurrentUserStatic = CurrentUserAccount;
            }
            else
            {
                CurrentUserAccount.Name = "Invalid user, not logged in";
                CurrentUserAccount.ProfilePicture = "Images/DefaultProfile.png"; // Gambar default jika pengguna tidak valid
            }
        }
    }
}
