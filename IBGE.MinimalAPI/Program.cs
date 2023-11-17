using ClosedXML.Excel;
using IBGE.Data.Model;
using IBGE.Data.Service;
using IBGE.Data.Service.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.Resource;
using Microsoft.OpenApi.Models;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));
builder.Services.AddAuthorization();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IDbService, DbService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IIBGEService, IBGEService>();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    _ = app.UseSwagger();
    _ = app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

string scopeRequiredByApi = app.Configuration["AzureAd:Scopes"] ?? "";
string[] summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", (HttpContext httpContext) =>
{
    httpContext.VerifyUserHasAnyAcceptedScope(scopeRequiredByApi);

    WeatherForecast[] forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi()
.RequireAuthorization();

#region User
app.MapPost("/users", (User user) =>
{
    // Logic to store the user in a database goes here
    User userReceive = user;
    // For simplicity, we'll just return the user
    return Results.Created($"/users/{user.Email}", user);
})
.WithOpenApi(operation => new(operation)
{
    Summary = "This is a summary",
    Description = "This is a description"
})
.WithTags("Authentication & Authorization");

app.MapGet("/user", async ([FromServices] IUserService employeeService) => await employeeService.GetAllAsList()).WithTags("Teste");
app.MapGet("/user/{id}", async ([FromServices] IUserService employeeService, int id) => await employeeService.GetById(id));
app.MapPost("/user", async ([FromServices] IUserService employeeService, User user) => await employeeService.Create(user));
app.MapPut("/user", async ([FromServices] IUserService employeeService, User user) => await employeeService.Update(user));
app.MapDelete("/user/{id}", async ([FromServices] IUserService employeeService, int id) => await employeeService.Delete(id));
#endregion User
#region IBGE
app.MapGet("/ibge", async ([FromServices] IIBGEService ibgeService) => await ibgeService.GetAllInformation())
.WithOpenApi(operation => new(operation)
{
    Summary = "Busca todos os dados IBGE",
    Description = "Enpoint para buscar todos os registros da Tabela IBGE."
}).WithTags("Informações IBGE");

app.MapGet("/ibge/buscaPorEstadoUF/{idstate}", async ([FromServices] IIBGEService ibgeService, string ufState) => await ibgeService.GetInformationByState(ufState))
.WithOpenApi(operation => new(operation)
{
    Summary = "Busca todos os dados IBGE filtrado pelo UF do estado",
    Description = "Enpoint para buscar todos os registros da Tabela IBGE que sejam do UF do estado fornecido."
}).WithTags("Informações IBGE");

app.MapGet("/ibge/buscaPorId/{idibge}", async ([FromServices] IIBGEService ibgeService, string id) => await ibgeService.GetInformationByCode(id))
    .WithOpenApi(operation => new(operation) { Summary = "Busca todos os dados IBGE filtrado pelo codigo de cadastro IBGE", Description = "Enpoint para buscar todos os registros da Tabela IBGE." })
.WithTags("Informações IBGE");

app.MapGet("/ibge/buscaPorNomeCidade/{idcity}", async ([FromServices] IIBGEService ibgeService, string cityName) => await ibgeService.GetInformationByCity(cityName)).WithOpenApi(operation => new(operation) { Summary = "Busca todos os dados IBGE", Description = "Enpoint para buscar todos os registros da Tabela IBGE que sejam do identificador do estado fornecido." }).WithTags("Informações IBGE");

app.MapPost("/ibge", async ([FromServices] IIBGEService ibgeService, IBGE.Data.Model.IBGE ibge) => await ibgeService.Create(ibge))
    .WithOpenApi(operation => new(operation)
    {
        Summary = "Insere um novo registro na tabela IBGE",
        Description = "Enpoint para inserir novos registros da Tabela IBGE."
    })
    .WithTags("Informações IBGE");

app.MapPut("/ibge", async ([FromServices] IIBGEService ibgeService, IBGE.Data.Model.IBGE ibge) => await ibgeService.Update(ibge)).WithOpenApi(operation => new(operation) { Summary = "Atualiza dados IBGE filtrado pelo codigo de cadastro IBGE", Description = "Enpoint para buscar todos os registros da Tabela IBGE." }).WithTags("Informações IBGE");

app.MapDelete("/ibge/{id}", async ([FromServices] IIBGEService ibgeService, string id) => await ibgeService.Delete(id))
    .WithOpenApi(operation => new(operation) { Summary = "Apaga dados IBGE filtrado pelo codigo de cadastro IBGE", Description = "Enpoint para deletar o registro da Tabela IBGE." })
    .WithTags("Informações IBGE");
#endregion IBGE

#region Aditional Functionality
app.MapPost("/upload", (IFormFile file) =>
{
    using Stream stream = file.OpenReadStream();
    using var workbook = new XLWorkbook(stream);
    // Process the data from the .xlsx file
    // ...
    return Results.Ok();
})
.WithOpenApi(operation => new(operation)
{
    Summary = "Upload an Excel file",
    Description = "This endpoint allows you to upload an Excel file",
    OperationId = "UploadExcelFile",
});
#endregion Aditional Functionality
app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
