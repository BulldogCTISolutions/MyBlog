using Data.Models.Models;

namespace Components.Interfaces;

public interface IBlogNotificationService
{
    event Action<BlogPost>? BlogPostChanged;
    Task SendNotification( BlogPost blogPost );
}
