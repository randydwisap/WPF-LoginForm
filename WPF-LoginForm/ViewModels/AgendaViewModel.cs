using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using WPF_LoginForm.Models;
using WPF_LoginForm.Repositories;
using WPF_LoginForm.Views;

namespace WPF_LoginForm.ViewModels
{
    public class AgendaViewModel : ViewModelBase
    {
        private DateTime _currentDate;
        private readonly IAgendaRepository _agendaRepository;
        private DateTime? _selectedDate;

        public ObservableCollection<AgendaHModel> AgendaH { get; set; }
        public ObservableCollection<AgendaHModel> FilteredAgendaH { get; set; }

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

        public ICommand ShowAddAgendaCommand { get; }
        public ICommand EditAgendaCommand { get; }
        public ICommand DeleteAgendaCommand { get; }

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

        public DateTime? SelectedDate
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
                FilterAgenda(); // Call to filter the agenda when date changes
            }
        }

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


        private string _searchKeyword;
        public string SearchKeyword
        {
            get => _searchKeyword;
            set
            {
                _searchKeyword = value;
                OnPropertyChanged(nameof(SearchKeyword));
                FilterAgenda(); // Call to filter the agenda when search keyword changes
            }
        }

        public string NomorWA { get; set; }

        public AgendaViewModel()
        {
            _agendaRepository = new AgendaRepository();

            // Fetch data from repository
            AgendaH = new ObservableCollection<AgendaHModel>(_agendaRepository.GetAllAgendaByUsername(MainViewModel.CurrentUserStatic.Username));
            FilteredAgendaH = new ObservableCollection<AgendaHModel>(AgendaH); // Initialize with all agendas
            Message = "Agenda View";
            NomorWA = MainViewModel.CurrentUserStatic.Name;

            CurrentDate = DateTime.Now;
            SelectedDate = CurrentDate; // Set default SelectedDate to CurrentDate


            ShowAddAgendaCommand = new ViewModelCommand(ExecuteShowAddAgenda);
            EditAgendaCommand = new ViewModelCommand(ExecuteEditAgenda);
            DeleteAgendaCommand = new ViewModelCommand(ExecuteDeleteAgenda);
            // Initialize commands
            NextMonthCommand = new ViewModelCommand((obj) => NextMonth());
            PreviousMonthCommand = new ViewModelCommand((obj) => PreviousMonth());
            NextDayCommand = new ViewModelCommand((obj) => NextDay());
            PreviousDayCommand = new ViewModelCommand((obj) => PreviousDay());
            NextYearCommand = new ViewModelCommand((obj) => NextYear());
            PreviousYearCommand = new ViewModelCommand((obj) => PreviousYear());

        }

        private void ExecuteShowAddAgenda(object obj)
        {
            var addAgendaWindowViewModel = new AddAgendaWindowViewModel();
            var addAgendaWindow = new AddAgendaWindow
            {
                DataContext = addAgendaWindowViewModel
            };

            addAgendaWindow.Closed += (sender, e) => RefreshData();
            addAgendaWindow.ShowDialog();
        }

        private void ExecuteEditAgenda(object obj)
        {
            if (obj is AgendaHModel agenda)
            {
                var addAgendaWindowViewModel = new AddAgendaWindowViewModel
                {
                    AgendaID = agenda.AgendaID,
                    NamaAgenda = agenda.NamaAgenda,
                    TglAgenda = agenda.TglAgenda,
                    KeteranganAgenda = agenda.KeteranganAgenda,
                    SelectedTime = DateTime.Today.Add(agenda.TglAgenda.TimeOfDay),
                    UserCreate = agenda.UserCreate
                };

                var addAgendaWindow = new AddAgendaWindow
                {
                    DataContext = addAgendaWindowViewModel
                };

                addAgendaWindow.Closed += (sender, e) => RefreshData();
                addAgendaWindow.ShowDialog();
            }
        }

        private void ExecuteDeleteAgenda(object obj)
        {
            if (obj is AgendaHModel agenda)
            {
                var result = MessageBox.Show($"Are you sure you want to delete the agenda: {agenda.NamaAgenda}?",
                                             "Confirm Delete",
                                             MessageBoxButton.YesNo,
                                             MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        _agendaRepository.RemoveAgenda(agenda.AgendaID);
                        AgendaH.Remove(agenda);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Failed to delete agenda: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            RefreshData();
        }

        private void FilterAgenda()
        {
            var filtered = AgendaH.AsEnumerable();

            // Filter by selected date
            if (SelectedDate.HasValue)
            {
                var selectedDate = SelectedDate.Value.Date;
                filtered = filtered.Where(a => a.TglAgenda.Date == selectedDate);
            }

            FilteredAgendaH.Clear();
            foreach (var agenda in filtered)
            {
                FilteredAgendaH.Add(agenda);
            }
        }

        private void RefreshData()
        {
            // Ambil data agenda terbaru dari repository berdasarkan username
            var agendaFromDb = _agendaRepository.GetAllAgendaByUsername(MainViewModel.CurrentUserStatic.Username);

            // Menampilkan log untuk memverifikasi jumlah agenda yang diambil
            Console.WriteLine($"Agenda count from DB: {agendaFromDb.Count()}");

            // Clear data yang ada sebelumnya untuk memperbarui dengan data baru
            AgendaH.Clear();
            FilteredAgendaH.Clear();

            // Menambahkan data agenda yang baru dari repository
            foreach (var agenda in agendaFromDb)
            {
                Console.WriteLine($"Adding agenda: {agenda.AgendaID}");  // Menampilkan ID agenda yang ditambahkan
                AgendaH.Add(agenda);
            }

            // Terapkan filter pada data yang baru
            FilterAgenda();
        }

        public ICommand NextMonthCommand { get; }
        public ICommand PreviousMonthCommand { get; }
        public ICommand NextDayCommand { get; }
        public ICommand PreviousDayCommand { get; }
        public ICommand NextYearCommand { get; }
        public ICommand PreviousYearCommand { get; }

        public void NextMonth()
        {
            SelectedDate = SelectedDate?.AddMonths(1);
            FilterAgenda();
        }

        public void PreviousMonth()
        {
            SelectedDate = SelectedDate?.AddMonths(-1);
            FilterAgenda();
        }

        public void NextDay()
        {
            SelectedDate = SelectedDate?.AddDays(1);
            FilterAgenda();
        }

        public void PreviousDay()
        {
            SelectedDate = SelectedDate?.AddDays(-1);
            FilterAgenda();
        }

        // Method untuk menambah tahun
        public void NextYear()
        {
            SelectedDate = SelectedDate?.AddYears(1);
            FilterAgenda();
        }

        // Method untuk mengurangi tahun
        public void PreviousYear()
        {
            SelectedDate = SelectedDate?.AddYears(-1);
            FilterAgenda();
        }

    }
}