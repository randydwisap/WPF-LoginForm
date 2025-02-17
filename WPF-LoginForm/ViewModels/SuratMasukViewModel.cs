using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using WPF_LoginForm.Models;
using WPF_LoginForm.Views;

namespace WPF_LoginForm.ViewModels
{
    public class SuratMasukViewModel : ViewModelBase
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

        public SuratMasukViewModel()
        {          
            Message = "Hello from UserControl ViewModel!";
        }
    }
}
