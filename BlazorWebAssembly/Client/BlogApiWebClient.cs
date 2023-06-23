
using System.Net.Http.Json;
using System.Text.Json;

using Data.Models.Interfaces;
using Data.Models.Models;

using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace BlazorWebAssembly.Client;

public sealed class BlogApiWebClient : IBlogApi
{
    private readonly IHttpClientFactory _httpClientFactory;

    public BlogApiWebClient( IHttpClientFactory httpClientFactory ) => this._httpClientFactory = httpClientFactory;

    //  Blog Post API

    #region Blog Post API
    public async Task<List<BlogPost>?> GetBlogPostsAsync( int numberOfPosts, int startIndex )
    {
        using HttpClient? httpClient = this._httpClientFactory.CreateClient( "Public" );
        return await httpClient.GetFromJsonAsync<List<BlogPost>>( $"/api/BlogPosts?numberOfPosts={numberOfPosts}&startIndex={startIndex}" )
                               .ConfigureAwait( false );
    }

    public async Task<BlogPost?> GetBlogPostAsync( string id )
    {
        using HttpClient? httpClient = this._httpClientFactory.CreateClient( "Public" );
        return await httpClient.GetFromJsonAsync<BlogPost>( $"/api/BlogPosts/{id}" ).ConfigureAwait( false );
    }

    public async Task<int> GetBlogPostCountAsync()
    {
        using HttpClient? httpClient = this._httpClientFactory.CreateClient( "Public" );
        return await httpClient.GetFromJsonAsync<int>( $"/api/BlogPostCount" ).ConfigureAwait( false );
    }

    public async Task<BlogPost?> SaveBlogPostAsync( BlogPost item )
    {
        try
        {
            using HttpClient? httpClient = this._httpClientFactory.CreateClient( "Authenticated" );
            using HttpResponseMessage? response = await httpClient.PutAsJsonAsync( $"/api/BlogPosts", item )
                                                                  .ConfigureAwait( false );
            string? json = await response.Content.ReadAsStringAsync().ConfigureAwait( false );
            return JsonSerializer.Deserialize<BlogPost>( json );
        }
        catch( AccessTokenNotAvailableException ex )
        {
            ex.Redirect();
        }
        return null;
    }

    public async Task DeleteBlogPostAsync( string id )
    {
        try
        {
            using HttpClient? httpClient = this._httpClientFactory.CreateClient( "Authenticated" );
            Uri uri = new Uri( $"/api/BlogPosts/{id}" );
            using HttpResponseMessage httpResponseMessage = await httpClient!.DeleteAsync( uri ).ConfigureAwait( false );
        }
        catch( AccessTokenNotAvailableException ex )
        {
            ex.Redirect();
        }
    }

    #endregion Blog Post API

    //  Category API

    #region Category API
    public async Task<List<Category>?> GetCategoriesAsync()
    {
        using HttpClient? httpClient = this._httpClientFactory.CreateClient( "Public" );
        return await httpClient.GetFromJsonAsync<List<Category>>( $"/api/Categories" )
                               .ConfigureAwait( false );
    }

    public async Task<Category?> GetCategoryAsync( string id )
    {
        using HttpClient? httpClient = this._httpClientFactory.CreateClient( "Public" );
        return await httpClient.GetFromJsonAsync<Category>( $"/api/Categories/{id}" ).ConfigureAwait( false );
    }

    public async Task<Category?> SaveCategoryAsync( Category item )
    {
        try
        {
            using HttpClient? httpClient = this._httpClientFactory.CreateClient( "Authenticated" );
            using HttpResponseMessage? response = await httpClient.PutAsJsonAsync( $"/api/Categories", item )
                                                            .ConfigureAwait( false );
            string? json = await response.Content.ReadAsStringAsync().ConfigureAwait( false );
            return JsonSerializer.Deserialize<Category>( json );
        }
        catch( AccessTokenNotAvailableException ex )
        {
            ex.Redirect();
        }
        return null;
    }

    public async Task DeleteCategoryAsync( string id )
    {
        try
        {
            using HttpClient? httpClient = this._httpClientFactory.CreateClient( "Authenticated" );
            Uri uri = new Uri( $"/api/Categories/{id}" );
            using HttpResponseMessage httpResponseMessage = await httpClient.DeleteAsync( uri ).ConfigureAwait( false );
        }
        catch( AccessTokenNotAvailableException ex )
        {
            ex.Redirect();
        }
    }
    #endregion Category API

    //  Tag API

    #region Tag API
    public async Task<List<Tag>?> GetTagsAsync()
    {
        using HttpClient? httpClient = this._httpClientFactory.CreateClient( "Public" );
        return await httpClient.GetFromJsonAsync<List<Tag>>( $"/api/Tags" )
                               .ConfigureAwait( false );
    }

    public async Task<Tag?> GetTagAsync( string id )
    {
        using HttpClient? httpClient = this._httpClientFactory.CreateClient( "Public" );
        return await httpClient.GetFromJsonAsync<Tag>( $"/api/Tags/{id}" ).ConfigureAwait( false );
    }

    public async Task<Tag?> SaveTagAsync( Tag item )
    {
        try
        {
            using HttpClient? httpClient = this._httpClientFactory.CreateClient( "Authenticated" );
            using HttpResponseMessage? response = await httpClient.PutAsJsonAsync( $"/api/Categories", item )
                                                            .ConfigureAwait( false );
            string? json = await response.Content.ReadAsStringAsync().ConfigureAwait( false );
            return JsonSerializer.Deserialize<Tag>( json );
        }
        catch( AccessTokenNotAvailableException ex )
        {
            ex.Redirect();
        }
        return null;
    }

    public async Task DeleteTagAsync( string id )
    {
        try
        {
            using HttpClient? httpClient = this._httpClientFactory.CreateClient( "Authenticated" );
            Uri uri = new Uri( $"/api/Tags/{id}" );
            using HttpResponseMessage httpResponseMessage = await httpClient.DeleteAsync( uri ).ConfigureAwait( false );
        }
        catch( AccessTokenNotAvailableException ex )
        {
            ex.Redirect();
        }
    }
    #endregion Tag API

    public Task InvalidateCacheAsync() => throw new NotImplementedException();
}
