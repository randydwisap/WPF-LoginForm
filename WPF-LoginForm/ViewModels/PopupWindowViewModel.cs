using System;

namespace WPF_LoginForm.ViewModels
{
    public class PopupWindowViewModel : ViewModelBase
    {
        private string _currentDate;
        public string CurrentDate
        {
            get { return _currentDate; }
            set
            {
                _currentDate = value;
                OnPropertyChanged(nameof(CurrentDate));
            }
        }
        private string _popupMessage;
        public string PopupMessage
        {
            get { return _popupMessage; }
            set
            {
                _popupMessage = value;
                OnPropertyChanged(nameof(PopupMessage));
            }
        }
        public PopupWindowViewModel()
        {
            PopupMessage = "This is a popup window!";
            CurrentDate = DateTime.Now.ToString("yyyy-MM-dd");
        }
    }
}
