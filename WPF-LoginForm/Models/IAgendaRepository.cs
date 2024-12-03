using System.Collections.Generic;
using WPF_LoginForm.Models;

public interface IAgendaRepository
{
    // Menambahkan Agenda baru ke database
    void AddAgenda(AgendaHModel agendaHModel);

    // Mengambil semua agenda
    IEnumerable<AgendaHModel> GetAllAgenda();
    IEnumerable<AgendaHModel> GetAllAgendaByUsername(string username);

    // Mengambil agenda berdasarkan ID
    AgendaHModel GetAgendaById(int agendaId);

    // Mengupdate agenda yang sudah ada
    void UpdateAgenda(AgendaHModel agendaHModel);

    // Menghapus agenda berdasarkan ID
    void RemoveAgenda(int agendaId);
}
