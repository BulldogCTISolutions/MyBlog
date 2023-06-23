using Blazored.SessionStorage;

using Components.Interfaces;

namespace BlazorWebAssembly.Client.Services;

public class BlogBrowserStorage : IBrowserStorage
{
    private ISessionStorageService Storage { get; set; }

    public BlogBrowserStorage( ISessionStorageService storage )
    {
        this.Storage = storage;
    }

    public async Task DeleteAsync( string key )
    {
        await this.Storage.RemoveItemAsync( key ).ConfigureAwait( false );
    }

    public async Task<T?> GetAsync<T>( string key )
    {
        return await this.Storage.GetItemAsync<T>( key ).ConfigureAwait( false );
    }

    public async Task SetAsync( string key, object value )
    {
        await this.Storage.SetItemAsync( key, value ).ConfigureAwait( false );
    }
}
