using System.Collections.Generic;
using WPF_LoginForm.Models;

public interface IMemoRepository
{
    // Menambahkan Agenda baru ke database
    void AddMemo(MemoModel memoModel);

    // Mengambil semua agenda
    IEnumerable<MemoModel> GetAllMemo();
    IEnumerable<MemoModel> GetAllMemoByAgendaID(int agendaID);

    // Mengambil agenda berdasarkan ID
    MemoModel GetMemoById(int memoId);

    // Mengupdate agenda yang sudah ada
    void UpdateMemo(MemoModel memoModel);

    // Menghapus agenda berdasarkan ID
    void RemoveMemo(int memoId);
}
