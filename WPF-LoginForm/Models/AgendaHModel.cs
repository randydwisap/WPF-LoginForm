using Google.Protobuf.WellKnownTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_LoginForm.Models
{
    public class AgendaHModel
    {
        public int AgendaID { get; set; }
        public string NamaAgenda { get; set; }
        public DateTime TglAgenda { get; set; }
        public string KeteranganAgenda { get; set; }
        public string UserCreate { get; set; }
        public string Waktu => TglAgenda.ToString("HH:mm"); // Format waktu sesuai kebutuhan
        public string Tgl => TglAgenda.ToString("dd/MM/yyyy"); // Format waktu sesuai kebutuhan
    }
}
