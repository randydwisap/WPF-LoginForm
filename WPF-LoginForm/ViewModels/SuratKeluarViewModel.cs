using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using WPF_LoginForm.Models;
using WPF_LoginForm.Views;

namespace WPF_LoginForm.ViewModels
{
    public class SuratKeluarViewModel : ViewModelBase
    {
        public ObservableCollection<DataItem> Items { get; set; }
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

        public ICommand ShowPopupCommand { get; }

        public SuratKeluarViewModel()
        {
            Items = new ObservableCollection<DataItem>
            {
                new DataItem { No = 1, Pengirim = "Alice", Subjek = "Subject 1", Penerima = "Bob", Keterangan = "Description 1", Status = "Sent", Tanggal = DateTime.Now, Action = "View" },
                new DataItem { No = 2, Pengirim = "Charlie", Subjek = "Subject 2", Penerima = "David", Keterangan = "Description 2", Status = "Pending", Tanggal = DateTime.Now, Action = "Edit" },
                // Tambahkan data lainnya di sini
            };
            Message = "Hello from UserControl ViewModel!";
            ShowPopupCommand = new ViewModelCommand(ExecuteShowPopup);
        }

        private void ExecuteShowPopup(object obj)
        {
            var popupViewModel = new PopupWindowViewModel();
            var popupWindow = new PopupWindow
            {
                DataContext = popupViewModel
            };
            popupWindow.ShowDialog();
        }

    }
}
