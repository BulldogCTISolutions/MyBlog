
using Data.Models.Models;

using Microsoft.AspNetCore.SignalR;

namespace BlazorWebAssembly.Server.Hubs;

public class BlobNotificationHub : Hub
{
    public async Task SendNotificationAsync( BlogPost blogPost )
    {
        await this.Clients.All.SendAsync( "BlogPostChanged", blogPost ).ConfigureAwait( false );
    }
}
