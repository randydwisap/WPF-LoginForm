using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
using WPF_LoginForm.Models;
using WPF_LoginForm.Repositories;

namespace WPF_LoginForm.ViewModels
{
    public class AddMemoWindowViewModel : INotifyPropertyChanged
    {
        private int _memoID;
        private string _namaMemo;
        private AgendaHModel _selectedAgenda;
        private string _isiMemo;
        private string _fileMemo;
        private string _fileMemoDisplay;
        private ObservableCollection<AgendaHModel> _agendaList;
        private readonly MemoRepository _memoRepository;
        private readonly AgendaRepository _agendaRepository;
        private bool _isEditMode;

        public event PropertyChangedEventHandler PropertyChanged;

        public AddMemoWindowViewModel()
        {
            _memoRepository = new MemoRepository();
            _agendaRepository = new AgendaRepository();

            // Mengisi daftar agenda dari database
            AgendaList = new ObservableCollection<AgendaHModel>(_agendaRepository.GetAllAgenda());

            // Command untuk menyimpan atau mengedit memo
            SaveMemoCommand = new ViewModelCommand(SimpanMemo, CanExecuteSimpan);

            // Command untuk upload file memo
            UploadFileCommand = new ViewModelCommand(UploadFile);
        }

        // Indikator apakah dalam mode edit atau tambah memo
        public bool IsEditMode
        {
            get => _isEditMode;
            set
            {
                _isEditMode = value;
                OnPropertyChanged(nameof(IsEditMode));
                OnPropertyChanged(nameof(FileButtonText));  // Perbarui teks tombol upload
            }
        }

        public int MemoID
        {
            get => _memoID;
            set
            {
                _memoID = value;
                OnPropertyChanged(nameof(MemoID));
            }
        }

        public string NamaMemo
        {
            get => _namaMemo;
            set
            {
                _namaMemo = value;
                OnPropertyChanged(nameof(NamaMemo));
            }
        }

        public AgendaHModel SelectedAgenda
        {
            get => _selectedAgenda;
            set
            {
                _selectedAgenda = value;
                OnPropertyChanged(nameof(SelectedAgenda));
            }
        }

        public string IsiMemo
        {
            get => _isiMemo;
            set
            {
                _isiMemo = value;
                OnPropertyChanged(nameof(IsiMemo));
            }
        }

        public string FileMemo
        {
            get => _fileMemo;
            set
            {
                _fileMemo = value;
                OnPropertyChanged(nameof(FileMemo));

                // Hanya menampilkan nama file tanpa path lengkap
                FileMemoDisplay = string.IsNullOrEmpty(_fileMemo) ? string.Empty : Path.GetFileName(_fileMemo);
                OnPropertyChanged(nameof(UploadButtonColor)); // Perbarui warna tombol upload
            }
        }

        public string FileMemoDisplay
        {
            get => _fileMemoDisplay;
            set
            {
                _fileMemoDisplay = value;
                OnPropertyChanged(nameof(FileMemoDisplay));
            }
        }

        public ObservableCollection<AgendaHModel> AgendaList
        {
            get => _agendaList;
            set
            {
                _agendaList = value;
                OnPropertyChanged(nameof(AgendaList));
            }
        }

        // Teks pada tombol upload, berubah menjadi "Open" saat mode edit
        public string FileButtonText => IsEditMode ? "Open" : "Upload";

        // Warna tombol upload berubah menjadi hijau jika file telah dipilih
        public string UploadButtonColor => !string.IsNullOrEmpty(FileMemo) ? "Green" : "DarkGray";

        // Command untuk menyimpan memo
        public ICommand SaveMemoCommand { get; }
        private void SimpanMemo(object obj)
        {
            try
            {
                if (SelectedAgenda == null)
                {
                    MessageBox.Show("Silakan pilih agenda terlebih dahulu.", "Peringatan", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (IsEditMode)
                {
                    // Mode Edit
                    MemoModel updatedMemo = new MemoModel
                    {
                        MemoID = MemoID,
                        NamaMemo = NamaMemo,
                        AgendaID = SelectedAgenda.AgendaID,
                        NamaAgenda = SelectedAgenda.NamaAgenda,
                        IsiMemo = IsiMemo,
                        FileMemo = FileMemo,
                        UserCreateID = MainViewModel.CurrentUserStatic.UserID,
                        UserCreateName = MainViewModel.CurrentUserStatic?.Username ?? "Unknown"
                    };

                    _memoRepository.UpdateMemo(updatedMemo);
                    MessageBox.Show("Memo berhasil diperbarui!", "Sukses", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    // Mode Tambah
                    MemoModel newMemo = new MemoModel
                    {
                        NamaMemo = NamaMemo,
                        AgendaID = SelectedAgenda.AgendaID,
                        NamaAgenda = SelectedAgenda.NamaAgenda,
                        IsiMemo = IsiMemo,
                        FileMemo = FileMemo,
                        UserCreateID = MainViewModel.CurrentUserStatic.UserID,
                        UserCreateName = MainViewModel.CurrentUserStatic?.Username ?? "Unknown"
                    };

                    _memoRepository.AddMemo(newMemo);
                    MessageBox.Show("Memo berhasil disimpan!", "Sukses", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                ClearFields();
                Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w.IsActive)?.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Terjadi kesalahan: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool CanExecuteSimpan(object obj)
        {
            return !string.IsNullOrWhiteSpace(NamaMemo) &&
                   SelectedAgenda != null &&
                   !string.IsNullOrWhiteSpace(IsiMemo);
        }

        // Command untuk upload file memo
        public ICommand UploadFileCommand { get; }
        private void UploadFile(object obj)
        {
            if (IsEditMode && !string.IsNullOrEmpty(FileMemo))
            {
                try
                {
                    // Jika file ada, buka menggunakan aplikasi default
                    if (File.Exists(FileMemo))
                    {
                        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                        {
                            FileName = FileMemo,
                            UseShellExecute = true
                        });
                    }
                    else
                    {
                        MessageBox.Show("File tidak ditemukan.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Gagal membuka file: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                // Jika bukan mode edit, buka dialog untuk memilih file
                OpenFileDialog openFileDialog = new OpenFileDialog
                {
                    Filter = "PDF Files|*.pdf|Word Documents|*.doc;*.docx|All Files|*.*",
                    Title = "Pilih File Memo"
                };

                if (openFileDialog.ShowDialog() == true)
                {
                    FileMemo = openFileDialog.FileName;
                }
            }
        }

        // Method untuk mengisi data saat mode Edit
        public void LoadMemoForEdit(MemoModel memo)
        {
            if (memo != null)
            {
                IsEditMode = true;
                MemoID = memo.MemoID;
                NamaMemo = memo.NamaMemo;
                SelectedAgenda = AgendaList.FirstOrDefault(a => a.AgendaID == memo.AgendaID);
                IsiMemo = memo.IsiMemo;
                FileMemo = memo.FileMemo;
            }
        }

        // Method untuk mengosongkan field setelah menyimpan
        private void ClearFields()
        {
            NamaMemo = string.Empty;
            SelectedAgenda = null;
            IsiMemo = string.Empty;
            FileMemo = string.Empty;
            IsEditMode = false;
            MemoID = 0;
        }

        // Event PropertyChanged untuk binding
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
