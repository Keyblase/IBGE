using System.ComponentModel.DataAnnotations;

namespace IBGE.Data.ViewModel;
public class IBGE
{
    [MaxLength(7)]
    public int? Id { get; set; }

    [MaxLength(2)]
    public string State { get; set; } = string.Empty;

    [MaxLength(80)]
    public string City { get; set; } = string.Empty;
}

