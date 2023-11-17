using IBGE.Data.Model;
using IBGE.Data.Service.Interface;
using IBGEModel = IBGE.Data.Model.IBGE;

namespace IBGE.Data.Service;
public class IBGEService : IIBGEService
{
    private readonly IDbService _dbService;

    public IBGEService(IDbService dbService) => _dbService = dbService;

    #region CRUD
    public async Task<bool> Create(IBGEModel ibgeinput)
    {
        _ =
            await _dbService.EditData(
                "INSERT INTO IBGE (Id,State, City) VALUES (@Id, @State, @City)",
                ibgeinput);
        return true;
    }

    public async Task<IBGEModel> Update(IBGEModel ibgeinput)
    {
        _ =
            await _dbService.EditData(
                "UPDATE IBGE SET State=@State, City=@City WHERE Id=@Id",
                ibgeinput);
        return ibgeinput;
    }

    public async Task<bool> Delete(string id)
    {
        _ = await _dbService.EditData("DELETE FROM IBGE WHERE ID=@Id", new { id });
        return true;
    }

    public async Task<List<IBGEModel>> GetAllInformation()
    {
        List<IBGEModel> employeeList = await _dbService.GetAll<IBGEModel>("SELECT * FROM IBGE", new { });
        return employeeList;
    }

    #endregion

    public async Task<IBGEModel> GetInformationByCity(string cityName)
    {
        IBGEModel employeeList = await _dbService.GetAsync<IBGEModel>("SELECT * FROM IBGE WHERE City=@cityName", new { cityName });
        return employeeList;
    }
    public async Task<IBGEModel> GetInformationByState(string stateUF)
    {
        IBGEModel employeeList = await _dbService.GetAsync<IBGEModel>("SELECT * FROM IBGE WHERE State=@stateUF", new { stateUF });
        return employeeList;
    }
    public async Task<IBGEModel> GetInformationByCode(string id)
    {
        IBGEModel employeeList = await _dbService.GetAsync<IBGEModel>("SELECT * FROM IBGE WHERE Id=@Id", new { id });
        return employeeList;
    }
}
