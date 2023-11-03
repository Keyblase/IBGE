using IBGEModel = IBGE.Data.Model.IBGE;

namespace IBGE.Data.Service.Interface;
public interface IUserService
{
    Task<bool> CreateUser(IBGEModel user);
    Task<List<IBGEModel>> GetUserList();
    Task<IBGEModel> UpdateUser(IBGEModel user);
    Task<bool> DeleteUser(int key);
}
