using System.Collections.ObjectModel;

namespace Data.Models.Models;

public sealed class BlogPost
{
    public string? Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;
    public DateTime PublishDate { get; set; }
    public Category? Category { get; set; }
    public Collection<Tag> Tags { get; } = new Collection<Tag>();
}
