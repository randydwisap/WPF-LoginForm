using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Windows.Input;
using WPF_LoginForm.Models;
using WPF_LoginForm.Repositories;
using System.Linq;
using System.Windows; // Tambahkan ini untuk menggunakan LINQ 

namespace WPF_LoginForm.ViewModels
{
    public class AgendaViewModel : ViewModelBase
    {
        private DateTime _currentDate;
        private readonly IUserRepository _userRepository;
        private DateTime? _selectedDate; // Tambahkan properti untuk menyimpan tanggal yang dipilih

        public ObservableCollection<AgendaHModel> AgendaH { get; set; }
        public ObservableCollection<AgendaHModel> FilteredAgendaH { get; set; } // Koleksi untuk menampilkan hasil filter

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
        public int TodayAgendaCount => AgendaH.Count(a => a.TglAgenda.Date == DateTime.Today);
        public int TomorrowAgendaCount => AgendaH.Count(a => a.TglAgenda.Date == DateTime.Today.AddDays(1));
        public int ThisMonthAgendaCount => AgendaH.Count(a => a.TglAgenda.Month == DateTime.Today.Month && a.TglAgenda.Year == DateTime.Today.Year);

        public DateTime CurrentDate
        {
            get => _currentDate;
            set
            {
                _currentDate = value;
                OnPropertyChanged(nameof(CurrentDate));
                OnPropertyChanged(nameof(MonthCurrent));
                OnPropertyChanged(nameof(MonthAngkaCurrent));
                OnPropertyChanged(nameof(YearCurrent));
                OnPropertyChanged(nameof(DayCurrent));
                OnPropertyChanged(nameof(HariCurrent));

            }
        }

        public DateTime? SelectedDate // Properti untuk tanggal yang dipilih
        {
            get => _selectedDate;
            set
            {
                _selectedDate = value;
                OnPropertyChanged(nameof(SelectedDate));
                OnPropertyChanged(nameof(Month));
                OnPropertyChanged(nameof(MonthAngka));
                OnPropertyChanged(nameof(Year));
                OnPropertyChanged(nameof(Day));
                OnPropertyChanged(nameof(Hari));
                FilterAgenda(); // Panggil metode untuk memfilter agenda
            }
        }

        public ICommand NextMonthCommand { get; }
        public ICommand PreviousMonthCommand { get; }
        public ICommand PreviousDayCommand { get; }
        public ICommand NextDayCommand { get; }

        public string Month => SelectedDate?.ToString("MMMM");
        public string Day => SelectedDate?.ToString("dd");
        public string Hari => SelectedDate?.ToString("dddd");
        public string MonthAngka => SelectedDate?.ToString("MM");
        public string Year => SelectedDate?.ToString("yyyy");

        public string MonthCurrent => CurrentDate.ToString("MMMM");
        public string DayCurrent => CurrentDate.ToString("dd");
        public string HariCurrent => CurrentDate.ToString("dddd");
        public string MonthAngkaCurrent => CurrentDate.ToString("MM");
        public string YearCurrent => CurrentDate.ToString("yyyy");

        private string _searchKeyword; // Properti untuk menyimpan kata kunci pencarian
        public string SearchKeyword
        {
            get => _searchKeyword;
            set
            {
                _searchKeyword = value;
                OnPropertyChanged(nameof(SearchKeyword));
                FilterAgenda(); // Panggil metode untuk memfilter agenda
            }
        }

        public AgendaViewModel()
        {
            _userRepository = new UserRepository();

            // Ambil data dari database
            AgendaH = new ObservableCollection<AgendaHModel>(_userRepository.GetAgendaH());
            FilteredAgendaH = new ObservableCollection<AgendaHModel>(AgendaH); // Inisialisasi dengan semua agenda
            Message = "Agenda View";
            CurrentDate = DateTime.Now;
            SearchKeyword = MainViewModel.CurrentUserStatic.Username;
            //SearchKeyword = "";
            //MessageBox.Show(SearchKeyword);
            SelectedDate = CurrentDate; // Set default SelectedDate ke CurrentDate
            NextMonthCommand = new ViewModelCommand(_ => NextMonth());
            NextDayCommand = new ViewModelCommand(_ => NextDay());
            PreviousMonthCommand = new ViewModelCommand(_ => PreviousMonth());
            PreviousDayCommand = new ViewModelCommand(_ => PreviousDay());
        }

        private void FilterAgenda()
        {
            // Filter berdasarkan tanggal yang dipilih dan kata kunci pencarian
            var filtered = AgendaH.AsEnumerable();

            if (SelectedDate.HasValue)
            {
                var selectedDate = SelectedDate.Value.Date;
                filtered = filtered.Where(a => a.TglAgenda.Date == selectedDate);
            }

            if (!string.IsNullOrWhiteSpace(SearchKeyword))
            {
                filtered = filtered.Where(a => a.UserCreate.IndexOf(SearchKeyword, StringComparison.OrdinalIgnoreCase) >= 0);
            }

            FilteredAgendaH.Clear();
            foreach (var agenda in filtered)
            {
                FilteredAgendaH.Add(agenda);
            }
        }

        private string _loggedInUsername; // Simpan username yang sedang login

        public DataTable ConvertToDataTable(IEnumerable<AgendaHModel> AgendasH)
        {
            var table = new DataTable();
            table.Columns.Add("TglAgenda", typeof(DateTime));
            table.Columns.Add("NamaAgenda", typeof(string));
            table.Columns.Add("KeteranganAgenda", typeof(string));
            table.Columns.Add("UserCreate", typeof(string));

            foreach (var AgendaH in AgendasH)
            {
                var row = table.NewRow();
                row["TglAgenda"] = AgendaH.TglAgenda;
                row["NamaAgenda"] = AgendaH.NamaAgenda;
                row["KeteranganAgenda"] = AgendaH.KeteranganAgenda;
                row["UserCreate"] = AgendaH.UserCreate;


                table.Rows.Add(row);
            }

            return table;
        }

        public void NextMonth()
        {
            SelectedDate = SelectedDate?.AddMonths(1);
        }

        public void PreviousMonth()
        {
            SelectedDate = SelectedDate?.AddMonths(-1);
        }

        public void NextDay()
        {
            SelectedDate = SelectedDate?.AddDays(1);
        }

        public void PreviousDay()
        {
            SelectedDate = SelectedDate?.AddDays(-1);
        }
    }
}