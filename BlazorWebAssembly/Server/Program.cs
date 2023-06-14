using System.Reflection;

using BlazorWebAssembly.Server.Endpoints;

using Data;
using Data.Models.Interfaces;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration.UserSecrets;

namespace BlazorWebAssembly.Server;

public static class Program
{
    public static void Main( string[] args )
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder( args );

        // Add Configuration stack
        // Add keys and secrets to builder configuration
        _ = builder.Configuration
                   .SetBasePath( AppDomain.CurrentDomain.BaseDirectory )
                   .AddJsonFile( "appSettings.json" )
                   // Passing “false” as the second variable for UserSecrets
                   // That’s because in .NET 6, User Secrets were made “required” by default
                   // and by passing true, we make them optional. 
                   .AddUserSecrets( Assembly.GetExecutingAssembly().GetCustomAttribute<UserSecretsIdAttribute>()!.UserSecretsId, false )
                   .Build();

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

        // Add services to the container.
        _ = builder.Services.AddControllersWithViews();
        _ = builder.Services.AddRazorPages();

        _ = builder.Services.AddScoped<IBlogApi, BlogApiJsonDirectAccess>();

        //  Chapter 8, [page 173]
        _ = builder.Services.AddAuthentication( JwtBearerDefaults.AuthenticationScheme )
                            .AddJwtBearer( JwtBearerDefaults.AuthenticationScheme, c =>
                            {
                                c.Authority = builder.Configuration["Auth0:Domain"];
                                c.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                                {
                                    ValidAudience = builder.Configuration["Auth0:Audience"],
                                    ValidIssuer = builder.Configuration["Auth0:Domain"]
                                };
                            } );
        _ = builder.Services.AddAuthorization();

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
        _ = app.UseAuthentication();
        _ = app.UseAuthorization();

        app.MapBlogPostApi();
        app.MapCategoryApi();
        app.MapTagApi();

        _ = app.MapRazorPages();
        _ = app.MapControllers();
        _ = app.MapFallbackToFile( "index.html" );

        app.Run();
    }
}
