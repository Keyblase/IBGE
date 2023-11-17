using IBGE.Data.Model;
using IBGE.Data.Service.Interface;

namespace IBGE.Data.Service;
public class UserService : IUserService
{
    private readonly IDbService _dbService;

    public UserService(IDbService dbService) => _dbService = dbService;

    public async Task<bool> Create(User employee)
    {
        _ = await _dbService.EditData(
                "INSERT INTO User (id,name, age, address, mobile_number) VALUES (@Id, @Name, @Age, @Address, @MobileNumber)",
                employee);
        return true;
    }

    public async Task<List<User>> GetAllAsList()
    {
        List<User> employeeList = await _dbService.GetAll<User>("SELECT * FROM User", new { });
        return employeeList;
    }


    public async Task<User> GetById(int id)
    {
        User employeeList = await _dbService.GetAsync<User>("SELECT * FROM User where id=@id", new { id });
        return employeeList;
    }

    public async Task<User> Update(User employee)
    {
        _ = await _dbService.EditData(
                "Update User SET name=@Name, age=@Age, address=@Address, mobile_number=@MobileNumber WHERE id=@Id",
                employee);
        return employee;
    }

    public async Task<bool> Delete(int id)
    {
        _ = await _dbService.EditData("DELETE FROM User WHERE id=@Id", new { id });
        return true;
    }
}
