using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using WPF_LoginForm.Models;
using WPF_LoginForm.Repositories;

namespace WPF_LoginForm.ViewModels
{
    public class AddAgendaWindowViewModel : ViewModelBase
    {
        private readonly IAgendaRepository _agendaRepository;

        public int AgendaID { get; set; }
        public string NamaAgenda { get; set; }
        public DateTime TglAgenda { get; set; }
        public string KeteranganAgenda { get; set; }
        public string UserCreate { get; set; }

        private DateTime? _selectedTime;
        public DateTime? SelectedTime
        {
            get => _selectedTime;
            set
            {
                if (_selectedTime != value)
                {
                    _selectedTime = value;
                    OnPropertyChanged(nameof(SelectedTime));
                    OnPropertyChanged(nameof(SelectedTimeAsString)); // Menambahkan Notifikasi untuk perubahan Text
                }
            }
        }

        public string SelectedTimeAsString
        {
            get
            {
                // Konversi SelectedTime menjadi string yang sesuai untuk ComboBox
                return SelectedTime?.ToString("HH:mm");
            }
            set
            {
                if (DateTime.TryParse(value, out DateTime parsedTime))
                {
                    SelectedTime = parsedTime;
                }
            }
        }

        public ICommand SaveCommand { get; }

        public AddAgendaWindowViewModel()
        {
            _agendaRepository = new AgendaRepository();
            SaveCommand = new ViewModelCommand(ExecuteSave);
            TglAgenda = DateTime.Today;
        }

        public void InitializeAgenda(AgendaHModel agenda)
        {
            if (agenda != null)
            {
                AgendaID = agenda.AgendaID;
                NamaAgenda = agenda.NamaAgenda;
                KeteranganAgenda = agenda.KeteranganAgenda;
                TglAgenda = agenda.TglAgenda;

                // Mengatur waktu untuk SelectedTime
                SelectedTime = DateTime.Today.Add(agenda.TglAgenda.TimeOfDay);
            }
        }

        private void ExecuteSave(object obj)
        {
            if (string.IsNullOrWhiteSpace(NamaAgenda) || string.IsNullOrWhiteSpace(KeteranganAgenda))
            {
                MessageBox.Show("All fields must be filled!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Pastikan waktu yang dipilih tidak null
            if (SelectedTime.HasValue)
            {
                // Gabungkan TglAgenda dengan waktu yang dipilih
                TglAgenda = TglAgenda.Date.AddHours(SelectedTime.Value.Hour)
                                             .AddMinutes(SelectedTime.Value.Minute);
            }

            var agenda = new AgendaHModel
            {
                AgendaID = AgendaID,
                NamaAgenda = NamaAgenda,
                TglAgenda = TglAgenda,// TglAgenda sudah digabungkan dengan waktu yang dipilih
                KeteranganAgenda = KeteranganAgenda,
                UserCreate = MainViewModel.CurrentUserStatic?.Username ?? "Unknown"
            };

            try
            {
                if (AgendaID == 0) // Jika ini agenda baru
                {
                    _agendaRepository.AddAgenda(agenda);
                }
                else // Jika mengedit agenda
                {
                    _agendaRepository.UpdateAgenda(agenda);
                }

                MessageBox.Show("Agenda saved successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w.IsActive)?.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to save agenda: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
