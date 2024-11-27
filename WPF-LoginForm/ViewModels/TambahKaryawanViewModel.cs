using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using WPF_LoginForm.Models;
using WPF_LoginForm.Repositories;
using System.IO;
using System.Collections.ObjectModel;

namespace WPF_LoginForm.ViewModels
{
    public class TambahKaryawanViewModel : ViewModelBase
    {

        private string _buttonContent;
        public string ButtonContent
        {
            get { return _buttonContent; }
            set
            {
                _buttonContent = value;
                OnPropertyChanged(nameof(ButtonContent));
            }
        }


        // Properti untuk UserID
        private int _userId;
        public int UserID
        {
            get { return _userId; }
            set
            {
                _userId = value;
                OnPropertyChanged(nameof(UserID));
            }
        }

        // Properti untuk Username
        private string _username;
        public string Username
        {
            get { return _username; }
            set
            {
                _username = value;
                OnPropertyChanged(nameof(Username));
            }
        }

        // Properti untuk Password
        private string _password;
        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        // Properti untuk NIK
        private string _nik;
        public string NIK
        {
            get { return _nik; }
            set
            {
                _nik = value;
                OnPropertyChanged(nameof(NIK));
            }
        }

        // Properti untuk Nama
        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        // Properti untuk Nama Lengkap
        private string _fullName;
        public string FullName
        {
            get { return _fullName; }
            set
            {
                _fullName = value;
                OnPropertyChanged(nameof(FullName));
            }
        }

        // Properti untuk Email
        private string _email;
        public string Email
        {
            get { return _email; }
            set
            {
                _email = value;
                OnPropertyChanged(nameof(Email));
            }
        }

        // Properti untuk Nomor HP
        private string _nomorHP;
        public string NomorHP
        {
            get { return _nomorHP; }
            set
            {
                _nomorHP = value;
                OnPropertyChanged(nameof(NomorHP));
            }
        }

        // Properti untuk Role
        private string _role;
        public string Role
        {
            get { return _role; }
            set
            {
                _role = value;
                OnPropertyChanged(nameof(Role));
            }
        }

        // Properti untuk Profile Path
        private string _profilePath;
        public string ProfilePath
        {
            get { return _profilePath; }
            set
            {
                _profilePath = value;
                OnPropertyChanged(nameof(ProfilePath));
            }
        }




        // Properti untuk Sign Path
        private string _signPath;
        public string SignPath
        {
            get { return _signPath; }
            set
            {
                _signPath = value;
                OnPropertyChanged(nameof(SignPath));
            }
        }

        // Command untuk Menampilkan Profil
        public ICommand ShowProfileCommand { get; }

        // Command untuk Menampilkan Tanda Tangan
        public ICommand ShowSignCommand { get; }

        public ICommand AddUserCommand { get; }

        public TambahKaryawanViewModel()
        {
            var userRepository = new UserRepository();

            // Ambil data dari tabel Role
            var roles = userRepository.GetAllRoles();
            Roles = new ObservableCollection<RoleModel>(roles);

            // Inisialisasi Role yang dipilih (opsional)
            SelectedRole = Roles.FirstOrDefault();
            ShowProfileCommand = new ViewModelCommand(ExecuteShowProfile);
            ShowSignCommand = new ViewModelCommand(ExecuteShowSign);
            AddUserCommand = new ViewModelCommand(ExecuteAddUser);
            ButtonContent = "Tambah";

            // Pastikan direktori 'Images' ada
            if (!Directory.Exists(_imageDirectory))
                Directory.CreateDirectory(_imageDirectory);
        }
    


        private readonly string _imageDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images");
        // Metode untuk Menampilkan Profil
        private void ExecuteShowProfile(object obj)
        {

            var fileProfileDialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "Image Files (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png",
                Title = "Select Profile Image",
                Multiselect = false
            };

            if (fileProfileDialog.ShowDialog() == true)
            {
                string sourcePath = fileProfileDialog.FileName;
                string extension = Path.GetExtension(sourcePath);
                string targetFileName = $"{Username}{NIK}_Profile{extension}";
                string targetPath = Path.Combine(_imageDirectory, targetFileName);

                // Salin file ke direktori Images
                File.Copy(sourcePath, targetPath, true);

                // Perbarui ProfilePath dengan nama file baru
                ProfilePath = targetFileName;
            }
        }

        // Metode untuk Menampilkan Tanda Tangan
        private void ExecuteShowSign(object obj)
        {
            var fileSignDialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "Image Files (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png",
                Title = "Select Sign Image",
                Multiselect = false
            };

            if (fileSignDialog.ShowDialog() == true)
            {
                string sourcePath = fileSignDialog.FileName;
                string extension = Path.GetExtension(sourcePath);
                string targetFileName = $"{Username}{NIK}_Sign{extension}";
                string targetPath = Path.Combine(_imageDirectory, targetFileName);

                // Salin file ke direktori Images
                File.Copy(sourcePath, targetPath, true);

                // Perbarui SignPath dengan nama file baru
                SignPath = targetFileName;
            }
        }

        // Metode untuk Menambahkan atau Mengedit Pengguna
        public void ExecuteAddUser(object obj)
        {
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(NIK) || string.IsNullOrWhiteSpace(Name))
            {
                MessageBox.Show("Please fill in all required fields.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var userRepository = new UserRepository();

            if (UserID == 0) // Menambahkan pengguna baru
            {
                if (this.Role == "0")
                {
                    Role = "Admin";
                }
                else
                {
                    Role = "Karyawan";
                }

                var userModel = new UserModel
                {
                    Username = Username,
                    Password = Password,
                    NIK = NIK,
                    Name = Name,
                    FullName = FullName,
                    Email = Email,
                    NomorHP = NomorHP,
                    ProfilePicture = ProfilePath,
                    Role = SelectedRole?.NamaRole, // Gunakan RoleName dari SelectedRole
                    TandaTangan = SignPath
                };

                userRepository.Add(userModel);
                MessageBox.Show("User added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else // Mengedit pengguna yang ada
            {
                if (this.Role == "0")
                {
                    Role = "Admin";
                }
                else
                {
                    Role = "Karyawan";
                }

                var userModel = new UserModel
                {
                    UserID = UserID,
                    Username = Username,
                    Password = Password,
                    NIK = NIK,
                    Name = Name,
                    FullName = FullName,
                    Email = Email,
                    NomorHP = NomorHP,
                    ProfilePicture = ProfilePath,
                    Role = SelectedRole?.NamaRole, // Gunakan RoleName dari SelectedRole
                    TandaTangan = SignPath
                };

                userRepository.Edit(userModel);
                MessageBox.Show("User updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w.IsActive)?.Close();
        }

        private ObservableCollection<RoleModel> _roles;
        public ObservableCollection<RoleModel> Roles
        {
            get { return _roles; }
            set
            {
                _roles = value;
                OnPropertyChanged(nameof(Roles));
            }
        }

        private RoleModel _selectedRole;
        public RoleModel SelectedRole
        {
            get { return _selectedRole; }
            set
            {
                _selectedRole = value;
                OnPropertyChanged(nameof(SelectedRole));
            }
        }

    }
}