using System.ComponentModel.DataAnnotations;

namespace Data.Models.Models;

public sealed class Tag
{
    public string? Id { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;
}
