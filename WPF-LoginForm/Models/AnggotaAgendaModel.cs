using Google.Protobuf.WellKnownTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace WPF_LoginForm.Models
{
    public class AnggotaAgendaModel : INotifyPropertyChanged
    {
        private int? _agendaID;
        private bool _value;

        public int? AgendaID
        {
            get => _agendaID;
            set
            {
                if (_agendaID != value)
                {
                    _agendaID = value;
                    OnPropertyChanged(nameof(AgendaID));
                }
            }
        }

        public int UserID { get; set; }

        private string _userName;
        public string UserName
        {
            get => _userName;
            set
            {
                if (_userName != value)
                {
                    _userName = value;
                    OnPropertyChanged(nameof(UserName));
                }
            }
        }

        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        private string _nomorHP;
        public string NomorHP
        {
            get => _nomorHP;
            set
            {
                if (_nomorHP != value)
                {
                    _nomorHP = value;
                    OnPropertyChanged(nameof(NomorHP));
                }
            }
        }

        private string _namaAgenda;
        public string NamaAgenda
        {
            get => _namaAgenda;
            set
            {
                if (_namaAgenda != value)
                {
                    _namaAgenda = value;
                    OnPropertyChanged(nameof(NamaAgenda));
                }
            }
        }

        public bool Value
        {
            get => _value;
            set
            {
                if (_value != value)
                {
                    _value = value;
                    OnPropertyChanged(nameof(Value));
                }
            }
        }

        // INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

