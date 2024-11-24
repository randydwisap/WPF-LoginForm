using System.ComponentModel;

namespace WPF_LoginForm.Models
{
    public class UserAccountModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string _UserName;
        public string Username
        {
            get { return _UserName; }
            set
            {
                _UserName = value;
                OnPropertyChanged(nameof(Username));
            }
        }

        private string _Name;
        public string Name
        {
            get { return _Name; }
            set
            {
                _Name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        private string _ProfilePicture;
        public string ProfilePicture
        {
            get { return _ProfilePicture; }
            set
            {
                _ProfilePicture = value;
                OnPropertyChanged(nameof(ProfilePicture));
            }
        }

        private string _Role;
        public string Role
        {
            get { return _Role; }
            set
            {
                _Role = value;
                OnPropertyChanged(nameof(Role));
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
