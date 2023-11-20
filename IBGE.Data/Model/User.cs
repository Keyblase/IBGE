namespace IBGE.Data.Model;

public record User(int Id, string Email, string Password, string[] Roles);
