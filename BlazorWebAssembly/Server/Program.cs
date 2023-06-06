using BlazorWebAssembly.Server.Endpoints;

using Data;
using Data.Models.Interfaces;

namespace BlazorWebAssembly.Server;
public static class Program
{
    public static void Main( string[] args )
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder( args );

        ////	Accept only HTTP/3 connections
        //_ = builder.WebHost.ConfigureKestrel( ( WebHostBuilderContext context, KestrelServerOptions options ) =>
        //        options.Listen( IPAddress.Any, 7192, listenOptions =>
        //        {
        //            // Use HTTP/3
        //            listenOptions.Protocols = HttpProtocols.Http3;
        //            _ = listenOptions.UseHttps();
        //        } ) );

        //	Add API for Data
        //	This is a good place to load from KeyVault or database.
        _ = builder.Services
                   .AddOptions<BlogApiJsonDirectAccessSetting>()
                   .Configure( options =>
                   {
                       options.DataPath = @"..\..\Data\";
                       options.BlogPostsFolder = @"BlogPosts";
                       options.CategoriesFolder = @"Categories";
                       options.TagsFolder = @"Tags";
                   } );
        _ = builder.Services.AddScoped<IBlogApi, BlogApiJsonDirectAccess>();

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
        app.MapBlogPostApi();
        app.MapCategoryApi();
        app.MapTagApi();

        _ = app.MapRazorPages();
        _ = app.MapControllers();
        _ = app.MapFallbackToFile( "index.html" );

        app.Run();
    }
}
