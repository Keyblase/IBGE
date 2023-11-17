using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace IBGE.Data.Model;
public class IBGE
{
    [SetsRequiredMembers]
    public IBGE(string id, string state, string city)
    {
        Id = id;
        State = state;
        City = city;
    }

    [Key]
    [MaxLength(7)]
    public required string Id { get; set; }

    [MaxLength(2)]
    public required string State { get; set; }

    [MaxLength(80)]
    public required string City { get; set; }
}

