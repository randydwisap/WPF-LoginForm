using System;
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
    public class MemoViewModel : ViewModelBase
    {
        private readonly IMemoRepository _memoRepository;
        public ObservableCollection<MemoModel> Items { get; set; }

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

        public ICommand ShowAddMemoCommand { get; }
        public ICommand ShowEditMemoCommand { get; }  // Command Edit Memo
        public ICommand DeleteMemoCommand { get; }

        public MemoViewModel()
        {
            _memoRepository = new MemoRepository();

            // Ambil data dari database
            Items = new ObservableCollection<MemoModel>(_memoRepository.GetAllMemo());
            FilteredItems = new ObservableCollection<MemoModel>(Items);
            Message = "Data Memo View";

            ShowAddMemoCommand = new ViewModelCommand(ExecuteShowTambahMemo);
            ShowEditMemoCommand = new ViewModelCommand(ExecuteShowEditMemo); // Tambah command edit
            DeleteMemoCommand = new ViewModelCommand(ExecuteDeleteMemo);
        }

        // Menampilkan form tambah memo
        private void ExecuteShowTambahMemo(object obj)
        {
            var viewModel = new AddMemoWindowViewModel();
            var window = new AddMemoWindow
            {
                DataContext = viewModel
            };

            window.Closed += (sender, e) => RefreshData();
            window.ShowDialog();
        }

        // Menampilkan form edit memo
        private void ExecuteShowEditMemo(object obj)
        {
            if (obj is MemoModel selectedMemo)
            {
                var viewModel = new AddMemoWindowViewModel();
                viewModel.LoadMemoForEdit(selectedMemo); // Mengisi field memo

                var window = new AddMemoWindow
                {
                    DataContext = viewModel
                };

                window.Closed += (sender, e) => RefreshData();
                window.ShowDialog();
            }
        }

        // Menghapus memo
        private void ExecuteDeleteMemo(object obj)
        {
            if (obj is MemoModel memo)
            {
                var result = MessageBox.Show($"Apakah Anda yakin ingin menghapus memo {memo.NamaMemo}?",
                                             "Konfirmasi Hapus",
                                             MessageBoxButton.YesNo,
                                             MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        _memoRepository.RemoveMemo(memo.MemoID);
                        Items.Remove(memo);
                        RefreshData();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Gagal menghapus memo: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        // Penyegaran data memo dari database
        public void RefreshData()
        {
            var memosFromDb = _memoRepository.GetAllMemo();
            FilteredItems.Clear();
            foreach (var memo in memosFromDb)
            {
                FilteredItems.Add(memo);
            }
        }

        // Pencarian data memo
        private string _searchText;
        public string SearchText
        {
            get { return _searchText; }
            set
            {
                _searchText = value;
                OnPropertyChanged(nameof(SearchText));
                FilterData();
            }
        }

        private void FilterData()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                FilteredItems = new ObservableCollection<MemoModel>(Items);
            }
            else
            {
                var filtered = Items
                    .Where(memo =>
                        (memo.NamaMemo?.IndexOf(SearchText, StringComparison.OrdinalIgnoreCase) >= 0) ||
                        (memo.NamaAgenda?.IndexOf(SearchText, StringComparison.OrdinalIgnoreCase) >= 0) ||
                        (memo.IsiMemo?.IndexOf(SearchText, StringComparison.OrdinalIgnoreCase) >= 0)).ToList();

                FilteredItems = new ObservableCollection<MemoModel>(filtered);
            }
        }

        private ObservableCollection<MemoModel> _filteredItems;
        public ObservableCollection<MemoModel> FilteredItems
        {
            get { return _filteredItems; }
            set
            {
                _filteredItems = value;
                OnPropertyChanged(nameof(FilteredItems));
            }
        }
    }
}
