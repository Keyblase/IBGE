using IBGE.Data.Model;
using IBGE.Data.Service.Interface;

namespace IBGE.Data.Service;
public class UserService : IUserService
{
    private readonly IDbService _dbService;

    public UserService(IDbService dbService) => _dbService = dbService;

    public async Task<bool> CreateUser(User employee)
    {
        _ =
            await _dbService.EditData(
                "INSERT INTO public.user (id,name, age, address, mobile_number) VALUES (@Id, @Name, @Age, @Address, @MobileNumber)",
                employee);
        return true;
    }

    public async Task<List<User>> GetUserList()
    {
        List<User> employeeList = await _dbService.GetAll<User>("SELECT * FROM public.user", new { });
        return employeeList;
    }


    public async Task<User> GetUser(int id)
    {
        User employeeList = await _dbService.GetAsync<User>("SELECT * FROM public.user where id=@id", new { id });
        return employeeList;
    }

    public async Task<User> UpdateUser(User employee)
    {
        _ =
            await _dbService.EditData(
                "Update public.user SET name=@Name, age=@Age, address=@Address, mobile_number=@MobileNumber WHERE id=@Id",
                employee);
        return employee;
    }

    public async Task<bool> DeleteUser(int id)
    {
        _ = await _dbService.EditData("DELETE FROM public.user WHERE id=@Id", new { id });
        return true;
    }
}
