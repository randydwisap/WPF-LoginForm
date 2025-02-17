using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WPF_LoginForm.Models;
using WPF_LoginForm.Repositories;
using WPF_LoginForm.Services;
using WPF_LoginForm.Views;

namespace WPF_LoginForm.ViewModels
{
    public class AddAgendaWindowViewModel : ViewModelBase
    {
        private readonly IAgendaRepository _agendaRepository;
        private readonly IAnggotaAgendaRepository _anggotaAgendaRepository;

        public ObservableCollection<AnggotaAgendaModel> AnggotaAgenda { get; set; }

        public int AgendaID { get; set; }
        public string NamaAgenda { get; set; }
        public DateTime TglAgenda { get; set; }
        public string KeteranganAgenda { get; set; }
        public string UserCreate { get; set; }
        public bool add { get; set; }

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
        public ICommand ShowNomorWACommand { get; }

        public AddAgendaWindowViewModel()
        {
            _agendaRepository = new AgendaRepository();
            _anggotaAgendaRepository = new AnggotaAgendaRepository();
            // Inisialisasi AnggotaAgenda dengan ObservableCollection
            AnggotaAgenda = new ObservableCollection<AnggotaAgendaModel>();
            SaveCommand = new ViewModelCommand(ExecuteSave);
            TglAgenda = DateTime.Today;
            ShowPopupCommand = new ViewModelCommand(ShowPopup);
            ShowNomorWACommand = new ViewModelCommand(ShowNomorWA);
            LoadAnggotaAgenda();
        }
        private void LoadAnggotaAgenda()
        {
            var anggotaAgendaList = _anggotaAgendaRepository.GetAnggotaAgendaById(AgendaID);
            AnggotaAgenda.Clear();

            foreach (var anggota in anggotaAgendaList)
            {
                AnggotaAgenda.Add(anggota);
            }
        }

        private void ShowNomorWA(object obj)
        {
            var anggotaAgendaList = _anggotaAgendaRepository.GetAnggotaAgendaById(AgendaID);
            var nomorWhatsAppList = new List<string>();
            var namaList = new List<string>();
            foreach (var anggota in anggotaAgendaList)
            {
                // Debug untuk melihat status checkbo

                if (!string.IsNullOrEmpty(anggota.NomorHP))
                {
                    nomorWhatsAppList.Add(anggota.NomorHP);
                    namaList.Add(anggota.Name);
                   // MessageBox.Show($"Nama: {anggota.UserName}\nNomor HP: {anggota.NomorHP}");
                }
            }
            // Menampilkan nomor WhatsApp yang terkumpul
            string nomorWhatsAppString = string.Join(",", nomorWhatsAppList);
            string namaListString = string.Join(",", namaList);
          //  MessageBox.Show($"Nomor WhatsApp: {nomorWhatsAppString}");
            SendWhatsAppMessages(nomorWhatsAppList,namaList);

        }


        private async Task SendWhatsAppMessages(List<string> nomorWhatsAppList, List<string> namaList)
        {
            // Inisialisasi objek WhatsappBlast
            var whatsappBlast = new WhatsappBlast();

            // Format tanggal hanya tanggal (yyyy-MM-dd)
            string formattedTglAgenda = TglAgenda.ToString("dd MMMM yyyy", new CultureInfo("id-ID"));

            // Format waktu hanya jam dan menit (HH:mm)
            string formattedSelectedTime = SelectedTime?.ToString("HH:mm");

            // Pesan yang ingin dikirim
            string message = $"*SEPURANE NYEPAM, NGETEST*\n*[REMINDER]*\n*{NamaAgenda}*\nMembahas : *{KeteranganAgenda}*\nTanggal : *{formattedTglAgenda}*\nPukul : *{formattedSelectedTime} WIB*\nMohon kehadirannya , Terima Kasih !";

            // Mengirimkan pesan ke semua nomor WhatsApp yang ada di dalam list
            await Task.Run(() => whatsappBlast.SendMessagesAsync(nomorWhatsAppList, namaList, message));

            // Tidak perlu menutup sesi karena menggunakan Fonnte API
            // whatsappBlast.Close(); // Hapus atau komentar baris ini
        }


        private void ShowPopup(object obj)
        {
            // Jika agenda baru (AgendaID == 0), tampilkan pesan peringatan
            if (AgendaID == 0 && !add)  // Untuk kondisi agenda baru
            {
                MessageBox.Show("ANGGOTA DAPAT DITAMPILKAN DAN DITAMBAHKAN SAAT MENYIMPAN AGENDA.",
                                "SIMPAN AGENDA TERLEBIH DAHULU",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning);
                return;  // Jika data belum disimpan, keluar dari method dan tidak menampilkan pop-up
            }

            // Jika agenda sudah ada (sudah disimpan atau sedang dalam mode edit), tampilkan pop-up
            var agendaId = this.AgendaID;  // Ambil agendaId yang sesuai dari objek atau sumber lainnya

            // Buat instance PopupWindow dan kirim agendaId
            var popupWindow = new PopupWindow(agendaId);
            popupWindow.ShowDialog(); // Tampilkan window
        }



        public void InitializeAgenda(AgendaHModel agenda)
        {
            if (agenda != null)
            {
                add = true;
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

        public ICommand ShowPopupCommand { get; set; }
    }
}
