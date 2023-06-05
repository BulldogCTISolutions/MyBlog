using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace Data.Models.Models;

public sealed class BlogPost
{
    public string? Id { get; set; }

    [Required, MinLength( 5 )]
    public string Title { get; set; } = string.Empty;

    [Required]
    public string Text { get; set; } = string.Empty;

    public DateTime PublishDate { get; set; }

    public Category? Category { get; set; }

    public Collection<Tag> Tags { get; } = new Collection<Tag>();
}
