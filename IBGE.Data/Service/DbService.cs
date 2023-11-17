using System.Data;
using Dapper;
using IBGE.Data.Service.Interface;
using Microsoft.Data.SqlClient;

namespace IBGE.Data.Service;
public class DbService : IDbService
{
    private readonly IDbConnection _db;
    public DbService() => _db = new SqlConnection("Data Source=sqlserver;Persist Security Info=True;User ID=sa;Password=SqlServer2019!;Encrypt=True;Trust Server Certificate=True");

    public async Task<T> GetAsync<T>(string command, object parms)
    {
        T result;

        result = (await _db.QueryAsync<T>(command, parms).ConfigureAwait(false)).FirstOrDefault();

        return result;
    }

    public async Task<List<T>> GetAll<T>(string command, object parms)
    {

        var result = new List<T>();

        result = (await _db.QueryAsync<T>(command, parms)).ToList();

        return result;
    }

    public async Task<int> EditData(string command, object parms)
    {
        int result;

        result = await _db.ExecuteAsync(command, parms);

        return result;
    }
}
