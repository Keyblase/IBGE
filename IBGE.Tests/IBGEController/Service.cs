using IBGE.Data.Service;
using Testcontainers.MsSql;

namespace IBGE.Tests.IBGE;

public sealed class Service : IAsyncLifetime
{
    private readonly MsSqlContainer _mssql = new MsSqlBuilder()
        .WithImage("mcr.microsoft.com/mssql/server:2019-latest")
        .Build();

    public Task InitializeAsync() => _mssql.StartAsync();

    public Task DisposeAsync() => _mssql.DisposeAsync().AsTask();

    [Fact]
    public void ShouldReturnTwoIBGE()
    {
        // Given
        var ibgeService = new IBGEService(new DbService(_mssql.GetConnectionString()));

        // When
        _ = ibgeService.Create(new Data.Model.IBGE("1","Rio Preto da garoa","Acrelandia"));
        _ = ibgeService.Create(new Data.Model.IBGE("2", "Lagoa dos patos rachados", "Cisneilandia"));
        Task<List<Data.Model.IBGE>> customers = ibgeService.GetAllInformation();

        // Then
        Assert.Equal(2, customers.Result.Count);
    }
}
