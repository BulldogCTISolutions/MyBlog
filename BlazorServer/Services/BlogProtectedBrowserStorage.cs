
using Components.Interfaces;

using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace BlazorServer.Services;

public class BlogProtectedBrowserStorage : IBrowserStorage
{
    private ProtectedSessionStorage Storage { get; set; }

    public BlogProtectedBrowserStorage( ProtectedSessionStorage storage )
    {
        this.Storage = storage;
    }

    public async Task DeleteAsync( string key )
    {
        await this.Storage.DeleteAsync( key ).ConfigureAwait( false );
    }

    public async Task<T?> GetAsync<T>( string key )
    {
        ProtectedBrowserStorageResult<T> value = await this.Storage.GetAsync<T>( key ).ConfigureAwait( false );
        return value.Success ? value.Value : default;
    }

    public async Task SetAsync( string key, object value )
    {
        await this.Storage.SetAsync( key, value ).ConfigureAwait( false );
    }
}
