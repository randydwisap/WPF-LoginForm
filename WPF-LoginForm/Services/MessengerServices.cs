using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_LoginForm.Services
{
    public class MessengerService
    {
        public string CurrentUsername { get; set; }

        public MessengerService()
        {
            CurrentUsername = string.Empty; // Default value
        }
    }
}
