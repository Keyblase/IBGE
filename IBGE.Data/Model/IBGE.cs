using System.ComponentModel.DataAnnotations;

namespace IBGE.Data.Model;
public class IBGE
{
    [Key]
    [MaxLength(7)]
    public required string Id { get; set; }

    [MaxLength(2)]
    public required string State { get; set; }

    [MaxLength(80)]
    public required string City { get; set; }
}

