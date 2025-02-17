using Google.Protobuf.WellKnownTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_LoginForm.Models
{
    public class MemoModel
    {
        public int MemoID { get; set; }
        public int AgendaID { get; set; }
        public string NamaMemo { get; set; }
        public string NamaAgenda { get; set; }
        public string IsiMemo { get; set; }
        public string FileMemo { get; set; }
        public string UserCreateName { get; set; }
        public int UserCreateID { get; set; }
    }
}
