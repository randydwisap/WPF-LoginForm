using System.Collections.Generic;
using System.Net;
using WPF_LoginForm.Models;

public interface IUserRepository
{
    void Add(UserModel userModel);
    void Edit(UserModel userModel);
    void Remove(int id);
    IEnumerable<UserModel> GetByAll();
    IEnumerable<AgendaHModel> GetAgendaH();
    UserModel GetById(int userId); // Pastikan ini dideklarasikan
    UserModel GetByUsername(string username);
    IEnumerable<RoleModel> GetAllRoles();
    bool AuthenticateUser(NetworkCredential credential);
}
