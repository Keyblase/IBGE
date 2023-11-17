using IBGE.Data.Model;

namespace IBGE.Data.Service.Interface;
public interface IUserService
{
    Task<bool> CreateUser(User user);
    Task<List<User>> GetUserList();
    Task<User> GetUser(int id);
    Task<User> UpdateUser(User user);
    Task<bool> DeleteUser(int key);
}
