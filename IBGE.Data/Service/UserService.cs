using IBGE.Data.Service.Interface;
using IBGEModel = IBGE.Data.Model.IBGE;
namespace IBGE.Data.Service;
public class UserService : IUserService
{
    private readonly IDbService _dbService;

    public UserService(IDbService dbService) => _dbService = dbService;

    public async Task<bool> CreateUser(IBGEModel employee)
    {
        _ =
            await _dbService.EditData(
                "INSERT INTO public.employee (id,name, age, address, mobile_number) VALUES (@Id, @Name, @Age, @Address, @MobileNumber)",
                employee);
        return true;
    }

    public async Task<List<IBGEModel>> GetUserList()
    {
        List<IBGEModel> employeeList = await _dbService.GetAll<IBGEModel>("SELECT * FROM public.employee", new { });
        return employeeList;
    }


    public async Task<IBGEModel> GetEmployee(int id)
    {
        IBGEModel employeeList = await _dbService.GetAsync<IBGEModel>("SELECT * FROM public.employee where id=@id", new { id });
        return employeeList;
    }

    public async Task<IBGEModel> UpdateUser(IBGEModel employee)
    {
        _ =
            await _dbService.EditData(
                "Update public.employee SET name=@Name, age=@Age, address=@Address, mobile_number=@MobileNumber WHERE id=@Id",
                employee);
        return employee;
    }

    public async Task<bool> DeleteUser(int id)
    {
        _ = await _dbService.EditData("DELETE FROM public.employee WHERE id=@Id", new { id });
        return true;
    }
}
