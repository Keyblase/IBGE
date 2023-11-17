using IBGE.Data.Model;
using IBGE.Data.Service;
using Testcontainers.MsSql;

namespace IBGE.Tests.UserController;

public sealed class Controller : IAsyncLifetime
{
    private readonly MsSqlContainer _mssql = new MsSqlBuilder()
        .WithImage("mcr.microsoft.com/mssql/server:2019-latest")
        .Build();

    public Task InitializeAsync() => _mssql.StartAsync();

    public Task DisposeAsync() => _mssql.DisposeAsync().AsTask();

    [Fact]
    public void ShouldReturnTwoCustomers()
    {
        // Given
        var userService = new UserService(new DbService(_mssql.GetConnectionString()));

        // When
        _ = userService.Create(new User("nico_sansoares@hsf.com", "George"));
        _ = userService.Create(new User("ana_sansoares@hsf.com", "John"));
        Task<List<User>> customers = userService.GetAllAsList();

        // Then
        Assert.Equal(2, customers.Result.Count);
    }
}
