using Data.Models.Interfaces;
using Data.Models.Models;

using Microsoft.AspNetCore.Mvc;

namespace BlazorWebAssembly.Server.Endpoints;

public static class CategoriesEndpoints
{
    public static void MapCategoryApi( this WebApplication app )
    {
        _ = app.MapGet( "/api/Categories",
            async ( IBlogApi api ) => Results.Ok( await api.GetCategoriesAsync().ConfigureAwait( false ) ) );

        _ = app.MapGet( "/api/Categories/{*id}",
        async ( IBlogApi api, string id ) => Results.Ok( await api.GetCategoryAsync( id ).ConfigureAwait( false ) ) );

        _ = app.MapPut( "/api/Categories",
        async ( IBlogApi api, [FromBody] Category item ) =>
            Results.Ok( await api.SaveCategoryAsync( item ).ConfigureAwait( false ) ) ).RequireAuthorization();

        _ = app.MapDelete( "/api/Categories/{*id}",
        async ( IBlogApi api, string id ) =>
        {
            await api.DeleteCategoryAsync( id ).ConfigureAwait( false );
            return Results.Ok();
        } ).RequireAuthorization();
    }
}
