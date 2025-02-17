using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using WPF_LoginForm.Models;

namespace WPF_LoginForm.Repositories
{
    public class MemoRepository : RepositoryBase, IMemoRepository
    {
        // Menambahkan Agenda ke Database
        public void AddMemo(MemoModel memoModel)
        {
            try
            {
                using (var connection = GetConnection())
                using (var command = new MySqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;

                    command.CommandText = @"INSERT INTO memo 
                                            (AgendaID, NamaMemo, NamaAgenda, IsiMemo, FileMemo, UserCreateID, UserCreateName)
                                            VALUES 
                                            (@AgendaID, @NamaMemo, @NamaAgenda, @IsiMemo, @FileMemo, @UserCreateID, @UserCreateName)";

                    // Menambahkan parameter
                    command.Parameters.Add("@AgendaID", MySqlDbType.Int32).Value = memoModel.AgendaID;
                    command.Parameters.Add("@NamaMemo", MySqlDbType.VarChar).Value = memoModel.NamaMemo;
                    command.Parameters.Add("@NamaAgenda", MySqlDbType.VarChar).Value = memoModel.NamaAgenda;
                    command.Parameters.Add("@IsiMemo", MySqlDbType.VarChar).Value = memoModel.IsiMemo;
                    command.Parameters.Add("@FileMemo", MySqlDbType.VarChar).Value = memoModel.FileMemo;                    
                    command.Parameters.Add("@UserCreateID", MySqlDbType.Int32).Value = memoModel.UserCreateID;
                    command.Parameters.Add("@UserCreateName", MySqlDbType.VarChar).Value = memoModel.UserCreateName;

                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Terjadi kesalahan saat menambahkan agenda: {ex.Message}");
                throw;
            }
        }

        // Mengambil semua Agenda dari Database
        public IEnumerable<MemoModel> GetAllMemo()
        {
            var memoList = new List<MemoModel>();

            try
            {
                using (var connection = GetConnection())
                using (var command = new MySqlCommand("SELECT * FROM memo", connection))
                {
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            memoList.Add(new MemoModel
                            {
                                MemoID = Convert.ToInt32(reader["MemoID"]),
                                AgendaID = Convert.ToInt32(reader["AgendaID"]),
                                NamaMemo = reader["NamaMemo"].ToString(),
                                NamaAgenda = reader["NamaAgenda"].ToString(),
                                IsiMemo = reader["IsiMemo"].ToString(),
                                FileMemo = reader["FileMemo"].ToString(),                                
                                UserCreateID = Convert.ToInt32(reader["UserCreateID"]),
                                UserCreateName = reader["UserCreateName"].ToString(),
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Terjadi kesalahan saat mengambil daftar agenda: {ex.Message}");
                throw;
            }

            return memoList;
        }

        public IEnumerable<MemoModel> GetAllMemoByAgendaID(int agendaID)
        {
            var memoList = new List<MemoModel>();

            try
            {
                using (var connection = GetConnection())
                {
                    // Menggunakan parameterized query untuk mencegah SQL injection
                    var query = @"SELECT *
                                  FROM MEMO
                                  WHERE AgendaID = @AgendaID";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        // Menambahkan parameter Username
                        command.Parameters.AddWithValue("@AgendaID", agendaID);

                        connection.Open();
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                memoList.Add(new MemoModel
                                {
                                    MemoID = Convert.ToInt32(reader["MemoID"]),
                                    AgendaID = Convert.ToInt32(reader["AgendaID"]),
                                    NamaMemo = reader["NamaMemo"].ToString(),
                                    NamaAgenda = reader["NamaAgenda"].ToString(),
                                    IsiMemo = reader["IsiMemo"].ToString(),
                                    FileMemo = reader["FileMemo"].ToString(),
                                    UserCreateID = Convert.ToInt32(reader["UserCreateID"]),
                                    UserCreateName = reader["UserCreateName"].ToString(),
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Terjadi kesalahan saat mengambil daftar agenda: {ex.Message}");
                throw;
            }

            return memoList;
        }



        // Mengambil Agenda Berdasarkan ID
        public MemoModel GetMemoById(int memoID)
        {
            MemoModel memo = null;

            try
            {
                using (var connection = GetConnection())
                using (var command = new MySqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "SELECT * FROM memo WHERE MemoID = @MemoID";
                    command.Parameters.Add("@MemoID", MySqlDbType.Int32).Value = memoID;

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            memo = new MemoModel
                            {
                                MemoID = Convert.ToInt32(reader["MemoID"]),
                                AgendaID = Convert.ToInt32(reader["AgendaID"]),
                                NamaMemo = reader["NamaMemo"].ToString(),
                                NamaAgenda = reader["NamaAgenda"].ToString(),
                                IsiMemo = reader["IsiMemo"].ToString(),
                                FileMemo = reader["FileMemo"].ToString(),
                                UserCreateID = Convert.ToInt32(reader["UserCreateID"]),
                                UserCreateName = reader["UserCreateName"].ToString(),
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Terjadi kesalahan saat mengambil agenda berdasarkan ID: {ex.Message}");
                throw;
            }

            return memo;
        }

        // Mengupdate Agenda di Database
        public void UpdateMemo(MemoModel memoModel)
        {
            try
            {
                using (var connection = GetConnection())
                using (var command = new MySqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;

                    command.CommandText = @"UPDATE memo 
                                            SET AgendaID = @AgendaID, 
                                                NamaMemo = @NamaMemo,
                                                NamaAgenda = @NamaAgenda,
                                                IsiMemo = @IsiMemo,
                                                FileMemo = @FileMemo, 
                                                UserCreateID = @UserCreateID,
                                                UserCreateName = @UserCreateName
                                            WHERE MemoID = @MemoID";

                    command.Parameters.Add("@MemoID", MySqlDbType.Int32).Value = memoModel.MemoID;
                    command.Parameters.Add("@AgendaID", MySqlDbType.Int32).Value = memoModel.AgendaID;
                    command.Parameters.Add("@NamaMemo", MySqlDbType.VarChar).Value = memoModel.NamaMemo;
                    command.Parameters.Add("@NamaAgenda", MySqlDbType.VarChar).Value = memoModel.NamaAgenda;
                    command.Parameters.Add("@IsiMemo", MySqlDbType.VarChar).Value = memoModel.IsiMemo;
                    command.Parameters.Add("@FileMemo", MySqlDbType.VarChar).Value = memoModel.FileMemo;
                    command.Parameters.Add("@UserCreateName", MySqlDbType.VarChar).Value = memoModel.UserCreateName;
                    command.Parameters.Add("@UserCreateID", MySqlDbType.Int32).Value = memoModel.UserCreateID;

                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Terjadi kesalahan saat mengupdate agenda: {ex.Message}");
                throw;
            }
        }

        // Menghapus Agenda dari Database
        public void RemoveMemo(int memoId)
        {
            try
            {
                using (var connection = GetConnection())
                using (var command = new MySqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;

                    command.CommandText = "DELETE FROM memo WHERE MemoID = @MemoID";
                    command.Parameters.Add("@MemoID", MySqlDbType.Int32).Value = memoId;

                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Terjadi kesalahan saat menghapus agenda: {ex.Message}");
                throw;
            }
        }
    }
}
