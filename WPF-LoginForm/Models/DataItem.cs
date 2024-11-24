using System;

namespace WPF_LoginForm.Models
{
    public class DataItem
    {
        public int No { get; set; }
        public string Pengirim { get; set; }
        public string Subjek { get; set; }
        public string Penerima { get; set; }
        public string Keterangan { get; set; }
        public string Status { get; set; }
        public DateTime Tanggal { get; set; }
        public string Action { get; set; }
    }
}
