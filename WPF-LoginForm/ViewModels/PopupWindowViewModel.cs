using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;  // Untuk ICommand
using WPF_LoginForm.Models;
using WPF_LoginForm.Repositories;
using WPF_LoginForm.Services;

namespace WPF_LoginForm.ViewModels
    {
        public class PopupWindowViewModel : ViewModelBase
        {
            private readonly IAnggotaAgendaRepository _anggotaAgendaRepository;

            public ObservableCollection<AnggotaAgendaModel> AnggotaAgenda { get; set; }

            private int _agendaID;
            public int AgendaID
            {
                get => _agendaID;
                set
                {
                    if (_agendaID != value)
                    {
                        _agendaID = value;
                        OnPropertyChanged(nameof(AgendaID));
                        LoadAnggotaAgenda();
                    }
                }
            }

            // Command untuk tombol Simpan
            public ICommand SimpanCommand { get; }

        public PopupWindowViewModel(IAnggotaAgendaRepository anggotaAgendaRepository, int agendaId)
        {
            _anggotaAgendaRepository = anggotaAgendaRepository;
            AnggotaAgenda = new ObservableCollection<AnggotaAgendaModel>();
            AgendaID = agendaId;
            LoadAnggotaAgenda();

            // Mengupdate binding value
            foreach (var anggota in AnggotaAgenda)
            {
                // Force notify when Value is updated (debugging purposes)
                OnPropertyChanged(nameof(AnggotaAgenda)); // Force notify for debugging
            }

            // Inisialisasi SimpanCommand
            SimpanCommand = new ViewModelCommand(SimpanAnggotaAgenda);
        }


        private void LoadAnggotaAgenda()
            {
                var anggotaAgendaList = _anggotaAgendaRepository.GetAnggotaAgendaById(AgendaID);
                AnggotaAgenda.Clear();

                foreach (var anggota in anggotaAgendaList)
                {
                    AnggotaAgenda.Add(anggota);
                }
            OnPropertyChanged(nameof(AnggotaAgenda)); // Menambahkan OnPropertyChanged setelah menambah data
        }

        // Method untuk menyimpan data anggota agenda
        // Method untuk menyimpan data anggota agenda dengan Value = true
        private void SimpanAnggotaAgenda(object obj)
        {
            _anggotaAgendaRepository.RemoveAnggotaAgenda(AgendaID);
            foreach (var anggota in AnggotaAgenda)
            {
                // Debug untuk melihat status checkbo
                
                if (anggota.Value)
                {
                    _anggotaAgendaRepository.AddAnggotaAgenda(new AnggotaAgendaModel
                    {
                        AgendaID = this.AgendaID ,
                        UserID = anggota.UserID,
                        UserName = anggota.UserName,
                        Value = anggota.Value  // Pastikan Value dikirim ke repository
                    });
                    
                }                
            }
            Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w.IsActive)?.Close();
        }

    }
}
