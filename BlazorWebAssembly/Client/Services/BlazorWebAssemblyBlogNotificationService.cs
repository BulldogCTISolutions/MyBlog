
using Components.Interfaces;

using Data.Models.Models;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;

namespace BlazorWebAssembly.Client.Services;

public sealed class BlazorWebAssemblyBlogNotificationService : IBlogNotificationService, IAsyncDisposable
{
    private readonly HubConnection _hubConnection;

    public event Action<BlogPost>? BlogPostChanged;

    public BlazorWebAssemblyBlogNotificationService( NavigationManager navigationManager )
    {
        if( navigationManager is null )
        {
            throw new ArgumentNullException( nameof( navigationManager ) );
        }
        this._hubConnection = new HubConnectionBuilder().WithUrl( navigationManager.ToAbsoluteUri( "/BlogNotificationHub" ) )
                                                        .Build();

        using( this._hubConnection.On<BlogPost>( "BlogPostChanged", ( blogPost ) => BlogPostChanged?.Invoke( blogPost ) ) )
        {
            _ = this._hubConnection.StartAsync();
        }
    }

    public async Task SendNotification( BlogPost blogPost )
    {
        await this._hubConnection.SendAsync( "SendNotification", blogPost ).ConfigureAwait( false );
    }

    public async ValueTask DisposeAsync()
    {
        await this._hubConnection.DisposeAsync().ConfigureAwait( false );
    }
}
