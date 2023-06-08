using Data.Models.Interfaces;
using Data.Models.Models;

using Microsoft.AspNetCore.Mvc;

namespace BlazorWebAssembly.Server.Endpoints;

public static class BlogPostEndpoints
{
    public static void MapBlogPostApi( this WebApplication app )
    {
        _ = app.MapGet( "/api/BlogPosts",
            async ( IBlogApi api, [FromQuery] int numberOfPosts, [FromQuery] int startIndex ) =>
                Results.Ok( await api.GetBlogPostsAsync( numberOfPosts, startIndex ).ConfigureAwait( false ) ) );

        _ = app.MapGet( "/api/BlogPostCount",
        async ( IBlogApi api ) => Results.Ok( await api.GetBlogPostCountAsync().ConfigureAwait( false ) ) );

        _ = app.MapGet( "/api/BlogPosts/{*id}",
        async ( IBlogApi api, string id ) => Results.Ok( await api.GetBlogPostAsync( id ).ConfigureAwait( false ) ) );

        _ = app.MapPut( "/api/BlogPosts",
        async ( IBlogApi api, [FromBody] BlogPost item ) =>
            Results.Ok( await api.SaveBlogPostAsync( item ).ConfigureAwait( false ) ) ).RequireAuthorization();

        _ = app.MapDelete( "/api/BlogPosts/{*id}",
        async ( IBlogApi api, string id ) =>
        {
            await api.DeleteBlogPostAsync( id ).ConfigureAwait( false );
            return Results.Ok();
        } ).RequireAuthorization();
    }
}
