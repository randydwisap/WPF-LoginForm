using System.Collections.Generic;
using WPF_LoginForm.Models;

public interface IAnggotaAgendaRepository
{
    // Menambahkan Agenda baru ke database
    void AddAnggotaAgenda(AnggotaAgendaModel anggotaAgendaModel);

    // Mengambil agenda berdasarkan ID
    IEnumerable<AnggotaAgendaModel> GetAnggotaAgendaById(int agendaId);
    void RemoveAnggotaAgenda(int agendaId);
}
