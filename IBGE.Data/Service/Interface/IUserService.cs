using IBGE.Data.Model;

namespace IBGE.Data.Service.Interface;
public interface IUserService
{
    Task<bool> Create(User user);
    Task<List<User>> GetAllAsList();
    Task<User> GetById(int id);
    Task<User> Update(User user);
    Task<bool> Delete(int key);
}
