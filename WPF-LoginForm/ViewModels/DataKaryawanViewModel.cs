using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows;
using Microsoft.Reporting.WinForms;
using WPF_LoginForm.Models;
using WPF_LoginForm.Repositories;
using System.Linq;
using WPF_LoginForm.Views;
using System.Data;
using System.Threading.Tasks;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace WPF_LoginForm.ViewModels
{
    public class DataKaryawanViewModel : ViewModelBase
    {
        private readonly IUserRepository _userRepository;

        public ObservableCollection<UserModel> Items { get; set; }

        private string _message;
        public string Message
        {
            get { return _message; }
            set
            {
                _message = value;
                OnPropertyChanged(nameof(Message));
            }
        }

        public ICommand ShowTambahKaryawanCommand { get; }
        public ICommand ShowPrintCommand { get; }
        public ICommand DeleteUserCommand { get; }
        public ICommand EditUserCommand { get; }

        private string _searchText;
        public string SearchText
        {
            get { return _searchText; }
            set
            {
                _searchText = value;
                OnPropertyChanged(nameof(SearchText));
                FilterData(); // Panggil metode filter saat teks pencarian berubah
            }
        }

        private ObservableCollection<UserModel> _filteredItems;
        public ObservableCollection<UserModel> FilteredItems
        {
            get { return _filteredItems; }
            set
            {
                _filteredItems = value;
                OnPropertyChanged(nameof(FilteredItems));
            }
        }



        public DataKaryawanViewModel()
        {
            _userRepository = new UserRepository();

            // Ambil data dari database
            Items = new ObservableCollection<UserModel>(_userRepository.GetByAll());
            FilteredItems = new ObservableCollection<UserModel>(Items); // Inisialisasi FilteredItems dengan semua data
            //_mainViewModel = mainViewModel;
            Message = "Data Karyawan View";


            // Inisialisasi command
            ShowTambahKaryawanCommand = new ViewModelCommand(ExecuteShowTambahKaryawan);
            ShowPrintCommand = new ViewModelCommand(ExecuteShowPrint);
            DeleteUserCommand = new ViewModelCommand(ExecuteDeleteUser);
            EditUserCommand = new ViewModelCommand(ExecuteEditUser);
          
        }

        public void RefreshData()
        {
            var usersFromDb = _userRepository.GetByAll();
            FilteredItems.Clear();
            foreach (var user in usersFromDb)
            {
                FilteredItems.Add(user);
            }
        }

        private void ExecuteShowTambahKaryawan(object obj)
        {
            var tambahKaryawanViewModel = new TambahKaryawanViewModel();
            var tambahKaryawanWindow = new TambahKaryawan
            {
                DataContext = tambahKaryawanViewModel
            };

            tambahKaryawanWindow.Closed += (sender, e) => RefreshData();

            tambahKaryawanWindow.ShowDialog();
        }

        private void ExecuteDeleteUser(object obj)
        {
            if (obj is UserModel user)
            {
                var result = MessageBox.Show($"Are you sure you want to delete {user.Username}?",
                                             "Confirm Delete",
                                             MessageBoxButton.YesNo,
                                             MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        _userRepository.Remove(user.UserID);
                        Items.Remove(user);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Failed to delete user: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            RefreshData();
        }

        private void ExecuteEditUser(object obj)
        {
            if (obj is UserModel user)
            {
                var tambahKaryawanViewModel = new TambahKaryawanViewModel
                {
                    UserID = user.UserID,
                    Username = user.Username,
                    Password = user.Password,
                    NIK = user.NIK,
                    Name = user.Name,
                    FullName = user.FullName,
                    Email = user.Email,
                    NomorHP = user.NomorHP,
                    ProfilePath = user.ProfilePicture,
                    Role = user.Role,
                    SignPath = user.TandaTangan
                };

                var tambahKaryawanWindow = new TambahKaryawan
                {
                    DataContext = tambahKaryawanViewModel
                };

                tambahKaryawanWindow.Closed += (sender, e) => RefreshData();

                tambahKaryawanWindow.ShowDialog();
            }
        }

        private void FilterData()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                // Jika teks pencarian kosong, tampilkan semua data
                FilteredItems = new ObservableCollection<UserModel>(Items);
            }
            else
            {
                // Filter data berdasarkan teks pencarian
                var filtered = Items
                    .Where(user =>
                        (user.Username?.IndexOf(SearchText, StringComparison.OrdinalIgnoreCase) >= 0) ||
                        (user.Name?.IndexOf(SearchText, StringComparison.OrdinalIgnoreCase) >= 0) ||
                        (user.NIK?.IndexOf(SearchText, StringComparison.OrdinalIgnoreCase) >= 0) ||
                        (user.FullName?.IndexOf(SearchText, StringComparison.OrdinalIgnoreCase) >= 0))
                    .ToList();

                FilteredItems = new ObservableCollection<UserModel>(filtered);
            }
        }

        private void ExecuteShowPrint(object obj)
        {
            GenerateReport();
        }


        public void GenerateReport()
        {
            Task.Run(() =>
            {
                try
                {
                    var usersFromDb = _userRepository.GetByAll();
                    var dataTable = ConvertToDataTable(usersFromDb);

                    var reportDocument = new ReportDocument();
                    reportDocument.Load("D:\\C# Project\\Login-In-WPF-MVVM-C-Sharp-and-SQL-Server-main\\WPF-LoginForm\\Reports\\DataKaryawanReport.rpt");
                    reportDocument.SetDataSource(dataTable);

                    // Gunakan CurrentUserAccount.Username sebagai PrintBy
                    // string printBy = CurrentUserAccount.Username;
                    // Set parameter PrintBy
                    // reportDocument.SetParameterValue("PrintBy", printBy);

                    // Ambil Username dari MainViewModel
                    //string printBy = MainViewModel.CurrentUserStatic?.Username ?? "Unknown";

                    // Set parameter PrintBy di Crystal Report
                    //reportDocument.SetParameterValue("PrintBy", printBy);
                    string printBy = MainViewModel.CurrentUserStatic?.Username ?? "Unknown";
                    reportDocument.SetParameterValue("PrintBy", printBy);

                    string pdfPath = "C:\\Reports\\DataKaryawanReport.pdf";
                    reportDocument.ExportToDisk(ExportFormatType.PortableDocFormat, pdfPath);

                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        ShowPDF(pdfPath);
                    });
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error generating report: {ex.Message}");
                }
            });
        }




        public void ShowPDF(string pdfPath)
        {
            try
            {
                Process.Start(new ProcessStartInfo(pdfPath) { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening PDF: {ex.Message}");
            }
        }

        public DataTable ConvertToDataTable(IEnumerable<UserModel> users)
        {
            var table = new DataTable();
            table.Columns.Add("UserID", typeof(int));
            table.Columns.Add("Username", typeof(string));
            table.Columns.Add("Password", typeof(string));
            table.Columns.Add("NIK", typeof(string));
            table.Columns.Add("Name", typeof(string));
            table.Columns.Add("FullName", typeof(string));
            table.Columns.Add("Email", typeof(string));
            table.Columns.Add("NomorHP", typeof(string));
            table.Columns.Add("ProfilePicture", typeof(string));
            table.Columns.Add("Role", typeof(string));
            table.Columns.Add("TandaTangan", typeof(string));
            table.Columns.Add("PrintBy", typeof(string));

            foreach (var user in users)
            {
                var row = table.NewRow();
                row["UserID"] = user.UserID;
                row["Username"] = user.Username;
                row["Password"] = user.Password;
                row["NIK"] = user.NIK;
                row["Name"] = user.Name;
                row["FullName"] = user.FullName;
                row["Email"] = user.Email;
                row["NomorHP"] = user.NomorHP;
                row["ProfilePicture"] = user.ProfilePicture;
                row["Role"] = user.Role;
                row["TandaTangan"] = user.TandaTangan;

                // Isi kolom PrintBy dengan CurrentUserStatic.Username
                row["PrintBy"] = MainViewModel.CurrentUserStatic?.Username ?? "Unknown";

                table.Rows.Add(row);
            }

            return table;
        }

    }
}
