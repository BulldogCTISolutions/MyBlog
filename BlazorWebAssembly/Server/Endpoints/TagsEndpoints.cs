using Data.Models.Interfaces;
using Data.Models.Models;

using Microsoft.AspNetCore.Mvc;

namespace BlazorWebAssembly.Server.Endpoints;

public static class TagsEndpoints
{
    public static void MapTagApi( this WebApplication app )
    {
        _ = app.MapGet( "/api/Tags",
            async ( IBlogApi api ) => Results.Ok( await api.GetTagsAsync().ConfigureAwait( false ) ) );

        _ = app.MapGet( "/api/Tags/{*id}",
        async ( IBlogApi api, string id ) => Results.Ok( await api.GetTagAsync( id ).ConfigureAwait( false ) ) );

        _ = app.MapPut( "/api/Tags",
        async ( IBlogApi api, [FromBody] Tag item ) =>
            Results.Ok( await api.SaveTagAsync( item ).ConfigureAwait( false ) ) ).RequireAuthorization();

        _ = app.MapDelete( "/api/Tags/{*id}",
        async ( IBlogApi api, string id ) =>
        {
            await api.DeleteTagAsync( id ).ConfigureAwait( false );
            return Results.Ok();
        } ).RequireAuthorization();
    }
}
