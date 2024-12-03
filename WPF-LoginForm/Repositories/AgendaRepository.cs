using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using WPF_LoginForm.Models;

namespace WPF_LoginForm.Repositories
{
    public class AgendaRepository : RepositoryBase, IAgendaRepository
    {
        // Menambahkan Agenda ke Database
        public void AddAgenda(AgendaHModel agendaHModel)
        {
            try
            {
                using (var connection = GetConnection())
                using (var command = new MySqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;

                    command.CommandText = @"INSERT INTO agendah 
                                            (NamaAgenda, TglAgenda, KeteranganAgenda, UserCreate)
                                            VALUES 
                                            (@NamaAgenda, @TglAgenda, @KeteranganAgenda, @UserCreate)";

                    // Menambahkan parameter
                    command.Parameters.Add("@NamaAgenda", MySqlDbType.VarChar).Value = agendaHModel.NamaAgenda;
                    command.Parameters.Add("@TglAgenda", MySqlDbType.DateTime).Value = agendaHModel.TglAgenda;
                    command.Parameters.Add("@KeteranganAgenda", MySqlDbType.VarChar).Value = agendaHModel.KeteranganAgenda;
                    command.Parameters.Add("@UserCreate", MySqlDbType.VarChar).Value = agendaHModel.UserCreate;

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
        public IEnumerable<AgendaHModel> GetAllAgenda()
        {
            var agendaList = new List<AgendaHModel>();

            try
            {
                using (var connection = GetConnection())
                using (var command = new MySqlCommand("SELECT * FROM AgendaH", connection))
                {
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            agendaList.Add(new AgendaHModel
                            {
                                AgendaID = Convert.ToInt32(reader["AgendaID"]),
                                NamaAgenda = reader["NamaAgenda"].ToString(),
                                TglAgenda = Convert.ToDateTime(reader["TglAgenda"]),
                                KeteranganAgenda = reader["KeteranganAgenda"].ToString(),
                                UserCreate = reader["UserCreate"].ToString(),
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

            return agendaList;
        }

        public IEnumerable<AgendaHModel> GetAllAgendaByUsername(string username)
        {
            var agendaList = new List<AgendaHModel>();

            try
            {
                using (var connection = GetConnection())
                {
                    // Menggunakan parameterized query untuk mencegah SQL injection
                    var query = @"SELECT h.AgendaID,NamaAgenda,TglAgenda,KeteranganAgenda,UserCreate 
                                  FROM agendah h ,agendad d
                                  WHERE d.AgendaID = h.AgendaID
                                  AND d.Username = @Username
                                  OR UserCreate = @Username";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        // Menambahkan parameter Username
                        command.Parameters.AddWithValue("@Username", username);

                        connection.Open();
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                agendaList.Add(new AgendaHModel
                                {
                                    AgendaID = Convert.ToInt32(reader["AgendaID"]),
                                    NamaAgenda = reader["NamaAgenda"].ToString(),
                                    TglAgenda = Convert.ToDateTime(reader["TglAgenda"]),
                                    KeteranganAgenda = reader["KeteranganAgenda"].ToString(),
                                    UserCreate = reader["UserCreate"].ToString(),
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

            return agendaList;
        }



        // Mengambil Agenda Berdasarkan ID
        public AgendaHModel GetAgendaById(int agendaId)
        {
            AgendaHModel agenda = null;

            try
            {
                using (var connection = GetConnection())
                using (var command = new MySqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "SELECT * FROM AgendaH WHERE AgendaID = @AgendaID";
                    command.Parameters.Add("@AgendaID", MySqlDbType.Int32).Value = agendaId;

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            agenda = new AgendaHModel
                            {
                                AgendaID = Convert.ToInt32(reader["AgendaID"]),
                                NamaAgenda = reader["NamaAgenda"].ToString(),
                                TglAgenda = Convert.ToDateTime(reader["TglAgenda"]),
                                KeteranganAgenda = reader["KeteranganAgenda"].ToString(),
                                UserCreate = reader["UserCreate"].ToString(),
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

            return agenda;
        }

        // Mengupdate Agenda di Database
        public void UpdateAgenda(AgendaHModel agendaHModel)
        {
            try
            {
                using (var connection = GetConnection())
                using (var command = new MySqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;

                    command.CommandText = @"UPDATE agendah 
                                            SET NamaAgenda = @NamaAgenda, 
                                                TglAgenda = @TglAgenda, 
                                                KeteranganAgenda = @KeteranganAgenda, 
                                                UserCreate = @UserCreate
                                            WHERE AgendaID = @AgendaID";

                    command.Parameters.Add("@AgendaID", MySqlDbType.Int32).Value = agendaHModel.AgendaID;
                    command.Parameters.Add("@NamaAgenda", MySqlDbType.VarChar).Value = agendaHModel.NamaAgenda;
                    command.Parameters.Add("@TglAgenda", MySqlDbType.DateTime).Value = agendaHModel.TglAgenda;
                    command.Parameters.Add("@KeteranganAgenda", MySqlDbType.VarChar).Value = agendaHModel.KeteranganAgenda;
                    command.Parameters.Add("@UserCreate", MySqlDbType.VarChar).Value = agendaHModel.UserCreate;

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
        public void RemoveAgenda(int agendaId)
        {
            try
            {
                using (var connection = GetConnection())
                using (var command = new MySqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;

                    command.CommandText = "DELETE FROM agendah WHERE AgendaID = @AgendaID";
                    command.Parameters.Add("@AgendaID", MySqlDbType.Int32).Value = agendaId;

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
