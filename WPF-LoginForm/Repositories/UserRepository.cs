using MySql.Data.MySqlClient;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Net;
using WPF_LoginForm.Models;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace WPF_LoginForm.Repositories
{
    public class UserRepository : RepositoryBase, IUserRepository
    {
        // Menambahkan User ke Database
        public void Add(UserModel userModel)
        {
            try
            {
                using (var connection = GetConnection())
                using (var command = new MySqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;

                    command.CommandText = @"INSERT INTO user 
                                            (Username, Password, NIK, Name, FullName, Email, NomorHP, ProfilePicture, Role, TandaTangan) 
                                            VALUES 
                                            (@Username, @Password, @NIK, @Name, @FullName, @Email, @NomorHP, @ProfilePicture, @Role, @TandaTangan)";

                    // Menambahkan parameter
                    command.Parameters.Add("@Username", MySqlDbType.VarChar).Value = userModel.Username;
                    command.Parameters.Add("@Password", MySqlDbType.VarChar).Value = userModel.Password;
                    command.Parameters.Add("@NIK", MySqlDbType.VarChar).Value = userModel.NIK;
                    command.Parameters.Add("@Name", MySqlDbType.VarChar).Value = userModel.Name;
                    command.Parameters.Add("@FullName", MySqlDbType.VarChar).Value = userModel.FullName;
                    command.Parameters.Add("@Email", MySqlDbType.VarChar).Value = userModel.Email;
                    command.Parameters.Add("@NomorHP", MySqlDbType.VarChar).Value = userModel.NomorHP;
                    command.Parameters.Add("@ProfilePicture", MySqlDbType.VarChar).Value = userModel.ProfilePicture;
                    command.Parameters.Add("@Role", MySqlDbType.VarChar).Value = userModel.Role;
                    command.Parameters.Add("@TandaTangan", MySqlDbType.VarChar).Value = userModel.TandaTangan;

                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while adding user: {ex.Message}");
                throw;
            }
        }

        public int GetLastUserIdByUsername(string username)
        {
            int userId = 0;
            try
            {
                using (var connection = GetConnection())
                using (var command = new MySqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;

                    // Query untuk mendapatkan UserID berdasarkan Username
                    command.CommandText = "SELECT UserID FROM user WHERE Username = @Username ORDER BY UserID DESC LIMIT 1";
                    command.Parameters.Add("@Username", MySqlDbType.VarChar).Value = username;

                    var result = command.ExecuteScalar();
                    if (result != null)
                        userId = Convert.ToInt32(result);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while getting last UserID: {ex.Message}");
                throw;
            }

            return userId;
        }


        // Update User di Database
        public void Edit(UserModel userModel)
        {
            try
            {
                using (var connection = GetConnection())
                using (var command = new MySqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;

                    command.CommandText = @"UPDATE user 
                                            SET Username = @Username, 
                                                Password = @Password, 
                                                NIK = @NIK, 
                                                Name = @Name, 
                                                FullName = @FullName, 
                                                Email = @Email, 
                                                NomorHP = @NomorHP, 
                                                ProfilePicture = @ProfilePicture, 
                                                Role = @Role, 
                                                TandaTangan = @TandaTangan
                                            WHERE UserID = @UserID";

                    command.Parameters.Add("@UserID", MySqlDbType.Int32).Value = userModel.UserID;
                    command.Parameters.Add("@Username", MySqlDbType.VarChar).Value = userModel.Username;
                    command.Parameters.Add("@Password", MySqlDbType.VarChar).Value = userModel.Password;
                    command.Parameters.Add("@NIK", MySqlDbType.VarChar).Value = userModel.NIK;
                    command.Parameters.Add("@Name", MySqlDbType.VarChar).Value = userModel.Name;
                    command.Parameters.Add("@FullName", MySqlDbType.VarChar).Value = userModel.FullName;
                    command.Parameters.Add("@Email", MySqlDbType.VarChar).Value = userModel.Email;
                    command.Parameters.Add("@NomorHP", MySqlDbType.VarChar).Value = userModel.NomorHP;
                    command.Parameters.Add("@ProfilePicture", MySqlDbType.VarChar).Value = userModel.ProfilePicture;
                    command.Parameters.Add("@Role", MySqlDbType.VarChar).Value = userModel.Role;
                    command.Parameters.Add("@TandaTangan", MySqlDbType.VarChar).Value = userModel.TandaTangan;

                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while updating user: {ex.Message}");
                throw;
            }
        }

        // Menghapus User dari Database
        public void Remove(int id)
        {
            try
            {
                using (var connection = GetConnection())
                using (var command = new MySqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;

                    command.CommandText = "DELETE FROM user WHERE UserID = @UserID";
                    command.Parameters.Add("@UserID", MySqlDbType.Int32).Value = id;

                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while removing user: {ex.Message}");
                throw;
            }
        }

        public UserModel GetById(int userId)
        {
            UserModel user = null;

            try
            {
                using (var connection = GetConnection())
                using (var command = new MySqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "SELECT * FROM user WHERE UserID = @UserID";
                    command.Parameters.Add("@UserID", MySqlDbType.Int32).Value = userId;

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            user = new UserModel
                            {
                                UserID = Convert.ToInt32(reader["UserID"]),
                                Username = reader["Username"].ToString(),
                                Password = reader["Password"].ToString(),
                                NIK = reader["NIK"].ToString(),
                                Name = reader["Name"].ToString(),
                                FullName = reader["FullName"].ToString(),
                                Email = reader["Email"].ToString(),
                                NomorHP = reader["NomorHP"].ToString(),
                                ProfilePicture = reader["ProfilePicture"].ToString(),
                                Role = reader["Role"].ToString(),
                                TandaTangan = reader["TandaTangan"].ToString(),
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while fetching user by ID: {ex.Message}");
                throw;
            }

            return user;
        }


        // Mendapatkan Semua User
        public IEnumerable<UserModel> GetByAll()
        {
            List<UserModel> users = new List<UserModel>();
            try
            {
                using (var connection = GetConnection())
                using (var command = new MySqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "SELECT * FROM user";

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            users.Add(new UserModel
                            {
                                UserID = Convert.ToInt32(reader["UserID"]),
                                Username = reader["Username"].ToString(),
                                Password = reader["Password"].ToString(),
                                NIK = reader["NIK"].ToString(),
                                Name = reader["Name"].ToString(),
                                FullName = reader["FullName"].ToString(),
                                Email = reader["Email"].ToString(),
                                NomorHP = reader["NomorHP"].ToString(),
                                ProfilePicture = reader["ProfilePicture"].ToString(),
                                Role = reader["Role"].ToString(),
                                TandaTangan = reader["TandaTangan"].ToString(),
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while fetching users: {ex.Message}");
                throw;
            }

            return users;
        }

        // Mendapatkan User Berdasarkan Username
        public UserModel GetByUsername(string username)
        {
            UserModel user = null;
            try
            {
                using (var connection = GetConnection())
                using (var command = new MySqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "SELECT * FROM user WHERE Username = @Username";
                    command.Parameters.Add("@Username", MySqlDbType.VarChar).Value = username;

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            user = new UserModel
                            {
                                UserID = Convert.ToInt32(reader["UserID"]),
                                Username = reader["Username"].ToString(),
                                Password = reader["Password"].ToString(),
                                NIK = reader["NIK"].ToString(),
                                Name = reader["Name"].ToString(),
                                FullName = reader["FullName"].ToString(),
                                Email = reader["Email"].ToString(),
                                NomorHP = reader["NomorHP"].ToString(),
                                ProfilePicture = reader["ProfilePicture"].ToString(),
                                Role = reader["Role"].ToString(),
                                TandaTangan = reader["TandaTangan"].ToString(),
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while fetching user by username: {ex.Message}");
                throw;
            }

            return user;
        }

        // Autentikasi User
        public bool AuthenticateUser(NetworkCredential credential)
        {
            try
            {
                using (var connection = GetConnection())
                using (var command = new MySqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "SELECT COUNT(1) FROM user WHERE Username = @Username AND Password = @Password";
                    command.Parameters.Add("@Username", MySqlDbType.VarChar).Value = credential.UserName;
                    command.Parameters.Add("@Password", MySqlDbType.VarChar).Value = credential.Password;

                    return Convert.ToInt32(command.ExecuteScalar()) > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while authenticating user: {ex.Message}");
                throw;
            }
        }

        public IEnumerable<RoleModel> GetAllRoles()
        {
            var roles = new List<RoleModel>();

            try
            {
                using (var connection = GetConnection())
                using (var command = new MySqlCommand("SELECT RoleID, NamaRole FROM role", connection))
                {
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            roles.Add(new RoleModel
                            {
                                RoleID = Convert.ToInt32(reader["RoleID"]),
                                NamaRole = reader["NamaRole"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching roles: {ex.Message}");
                throw;
            }

            return roles;
        }


        public IEnumerable<AgendaHModel> GetAgendaH()
        {
            var AgendaH = new List<AgendaHModel>();

            try
            {
                using (var connection = GetConnection())
                using (var command = new MySqlCommand("SELECT * FROM AgendaH", connection))
                //command.Parameters.Add("@Username", MySqlDbType.VarChar).Value = username;
                {
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            AgendaH.Add(new AgendaHModel
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
                Console.WriteLine($"Error fetching Agenda Header: {ex.Message}");
                throw;
            }

            return AgendaH;
        }

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
                                            (NamaAgenda,TglAgenda, KeteranganAgenda, UserCreate)
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
                Console.WriteLine($"Error while adding user: {ex.Message}");
                throw;
            }
        }



    }
}
