using System.Security.Claims;
using System.Text;
using ClosedXML.Excel;
using IBGE.Data;
using IBGE.Data.Model;
using IBGE.Data.Service;
using IBGE.Data.Service.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddAuthentication(x =>
    {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(x =>
    {
        x.RequireHttpsMetadata = false;
        x.SaveToken = true;
        x.TokenValidationParameters = new TokenValidationParameters
        {
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration.PrivateKey)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy => policy.RequireRole("manager"));
    options.AddPolicy("Employee", policy => policy.RequireRole("employee"));
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IDbService, DbService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IIBGEService, IBGEService>();
builder.Services.AddTransient<TokenService>();

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

#region User
app.MapGet("/hello", (HttpContext context)
        => Results.Ok(context.User.Identity?.Name ?? string.Empty))
.WithOpenApi(operation => new(operation)
{
    Summary = "Pagina Inicial",
    Description = "Enpoint para buscar todos os registros da Tabela IBGE."
}).WithTags("Authentication & Authorization")
.RequireAuthorization();

app.MapPost("/login", (User user, TokenService tokenService)
    => TokenService.Generate(user))
.WithOpenApi(operation => new(operation)
{
    Summary = "Busca todos os dados IBGE",
    Description = "Enpoint para buscar todos os registros da Tabela IBGE."
}).WithTags("Authentication & Authorization");

app.MapGet("/employee",
        (ClaimsPrincipal user) =>
            Results.Ok(new { message = $"Authenticated as {user.Identity?.Name}" }))
.WithOpenApi(operation => new(operation)
{
    Summary = "Busca todos os dados IBGE",
    Description = "Enpoint para buscar todos os registros da Tabela IBGE."
}).WithTags("Authentication & Authorization")
.RequireAuthorization("Employee");

app.MapGet("/manager",
        (ClaimsPrincipal user) =>
            Results.Ok(new { message = $"Authenticated as {user.Identity?.Name}" }))
.WithOpenApi(operation => new(operation)
{
    Summary = "Busca todos os dados IBGE",
    Description = "Enpoint para buscar todos os registros da Tabela IBGE."
}).WithTags("Authentication & Authorization")
.RequireAuthorization("Admin");

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
