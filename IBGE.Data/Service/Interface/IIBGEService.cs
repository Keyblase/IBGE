using IBGE.Data.Model;
using IBGEModel = IBGE.Data.Model.IBGE;

namespace IBGE.Data.Service.Interface;
public interface IIBGEService
{
    Task<bool> Create(IBGEModel user);
    Task<List<IBGEModel>> GetAllInformation();
    Task<IBGEModel> Update(IBGEModel user);
    Task<bool> Delete(string key);
    Task<IBGEModel> GetInformationByCity(string cityName);
    Task<IBGEModel> GetInformationByState(string StateUF);
    Task<IBGEModel> GetInformationByCode(string id);
}
