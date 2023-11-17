using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace IBGE.Data.Model;
public class User
{

    [SetsRequiredMembers]
    public User(string email, string password)
    {
        Email = email;
        Password = password;
    }

    [Key]
    public required int Id { get; set; }

    public required string Email { get; set; }
    public required string Password { get; set; }
}
