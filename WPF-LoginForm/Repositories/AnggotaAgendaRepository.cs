using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Web.Configuration;
using WPF_LoginForm.Models;

namespace WPF_LoginForm.Repositories
{
    public class AnggotaAgendaRepository : RepositoryBase, IAnggotaAgendaRepository
    {
        // Menambahkan AnggotaAgenda ke Database
        public void AddAnggotaAgenda(AnggotaAgendaModel anggotaAgendaHModel)
        {
            try
            {
                using (var connection = GetConnection())
                using (var command = new MySqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;

                    // Periksa apakah AgendaID sudah ada di tabel agendah
                    command.CommandText = "SELECT COUNT(*) FROM agendah WHERE AgendaID = @AgendaID";
                    command.Parameters.Add("@AgendaID", MySqlDbType.Int32).Value = anggotaAgendaHModel.AgendaID;
                    int count = Convert.ToInt32(command.ExecuteScalar());

                    if (count > 0)  // AgendaID ada di tabel agendah
                    {
                        if (anggotaAgendaHModel.Value) // Insert hanya jika Value = true
                        {
                            command.CommandText = @"INSERT INTO AgendaD 
                                            (AgendaID, UserID, Username)
                                            VALUES 
                                            (@AgendaID, @UserID, @Username)";

                            command.Parameters.Add("@UserID", MySqlDbType.Int32).Value = anggotaAgendaHModel.UserID;
                            command.Parameters.Add("@UserName", MySqlDbType.VarChar).Value = anggotaAgendaHModel.UserName;

                            command.ExecuteNonQuery();
                        }
                        else
                        {
                            Console.WriteLine("Value is false, skipping insert.");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"AgendaID {anggotaAgendaHModel.AgendaID} not found in agendah table.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Terjadi kesalahan saat menambahkan AnggotaAgenda: {ex.Message}");
                throw;
            }
        }


        // Mengambil AnggotaAgenda Berdasarkan ID
        public IEnumerable<AnggotaAgendaModel> GetAnggotaAgendaById(int AgendaId)
        {
            List<AnggotaAgendaModel> anggotaAgendaList = new List<AnggotaAgendaModel>();

            try
            {
                using (var connection = GetConnection())
                using (var command = new MySqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = @"
                SELECT 
                    u.UserID, 
                    u.Username, 
                    u.Name, 
                    CASE 
                        WHEN MAX(CASE WHEN h.AgendaID IS NOT NULL THEN 'true' ELSE 'false' END) = 'false' 
                        THEN NULL 
                        ELSE u.NomorHP 
                    END AS NomorHP,
                    h.AgendaID, 
                    h.NamaAgenda, 
                    MAX(CASE WHEN h.AgendaID IS NOT NULL THEN 'true' ELSE 'false' END) AS Value
                FROM 
                    user u 
                LEFT JOIN 
                    agendad d ON u.UserID = d.UserID 
                LEFT JOIN 
                    agendah h ON h.AgendaID = d.AgendaID AND h.AgendaID = @AgendaID 
                GROUP BY 
                    u.UserID, u.Username";

                    command.Parameters.Add("@AgendaID", MySqlDbType.Int32).Value = AgendaId;

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            anggotaAgendaList.Add(new AnggotaAgendaModel
                            {
                                UserID = Convert.ToInt32(reader["UserID"]),
                                UserName = reader["Username"].ToString(),

                                // Handle DBNull for Name
                                Name = reader["Name"] != DBNull.Value ? reader["Name"].ToString() : null,

                                // Handle DBNull for NomorHP
                                NomorHP = reader["NomorHP"] != DBNull.Value ? reader["NomorHP"].ToString() : null,

                                // Handle DBNull for AgendaID
                                AgendaID = reader["AgendaID"] != DBNull.Value ? Convert.ToInt32(reader["AgendaID"]) : (int?)null,

                                NamaAgenda = reader["NamaAgenda"].ToString(),

                                // Handle DBNull for Value
                                Value = reader["Value"] != DBNull.Value && reader["Value"].ToString() == "true"
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Terjadi kesalahan saat mengambil AnggotaAgenda berdasarkan AgendaID: {ex.Message}");
                throw;
            }

            return anggotaAgendaList;  // Mengembalikan koleksi
        }



        // Menghapus AnggotaAgenda dari Database
        public void RemoveAnggotaAgenda(int agendaId)
        {
            try
            {
                using (var connection = GetConnection())
                using (var command = new MySqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;

                    command.CommandText = "DELETE FROM agendaD WHERE AgendaID = @AgendaID";
                    command.Parameters.Add("@AgendaID", MySqlDbType.Int32).Value = agendaId;

                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Terjadi kesalahan saat menghapus AnggotaAgenda: {ex.Message}");
                throw;
            }
        }
    }
}
