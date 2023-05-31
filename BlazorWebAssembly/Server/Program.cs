using System.Net;

using Microsoft.AspNetCore.Server.Kestrel.Core;

namespace BlazorWebAssembly.Server;
public static class Program
{
    public static void Main( string[] args )
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder( args );

        //	Accept only HTTP/3 connections
        _ = builder.WebHost.ConfigureKestrel( ( WebHostBuilderContext context, KestrelServerOptions options ) =>
                options.Listen( IPAddress.Any, 7192, listenOptions =>
                {
                    // Use HTTP/3
                    listenOptions.Protocols = HttpProtocols.Http3;
                    _ = listenOptions.UseHttps();
                } ) );

        // Add services to the container.

        _ = builder.Services.AddControllersWithViews();
        _ = builder.Services.AddRazorPages();

        WebApplication app = builder.Build();

        // Configure the HTTP request pipeline.
        if( app.Environment.IsDevelopment() )
        {
            app.UseWebAssemblyDebugging();
        }
        else
        {
            _ = app.UseExceptionHandler( "/Error" );
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            _ = app.UseHsts();
        }

        _ = app.UseHttpsRedirection();

        _ = app.UseBlazorFrameworkFiles();
        _ = app.UseStaticFiles();

        _ = app.UseRouting();


        _ = app.MapRazorPages();
        _ = app.MapControllers();
        _ = app.MapFallbackToFile( "index.html" );

        app.Run();
    }
}
