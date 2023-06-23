using System.Reflection;

using Auth0.AspNetCore.Authentication;

using Components.Interfaces;
using Components.RazorComponents;

using Data;
using Data.Models.Interfaces;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Configuration.UserSecrets;

namespace BlazorServer;

public static class Program
{
    public static void Main( string[] args )
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder( args );

        // Add Configuration stack
        // Add keys and secrets to builder configuration
        _ = builder.Configuration
                   .SetBasePath( AppDomain.CurrentDomain.BaseDirectory )
                   .AddEnvironmentVariables()
                   .AddJsonFile( "appSettings.json" )
                   // Passing “false” as the second variable for UserSecrets
                   // That’s because in .NET 6, User Secrets were made “required” by default
                   // and by passing true, we make them optional. 
                   .AddUserSecrets( Assembly.GetExecutingAssembly().GetCustomAttribute<UserSecretsIdAttribute>()!.UserSecretsId, false )
                   .Build();

        // Add API for Data
        // This is a good place to load from KeyVault or database.
        _ = builder.Services
                   .AddOptions<BlogApiJsonDirectAccessSetting>()
                   .Configure( options =>
                   {
                       options.DataPath = @"..\Data\";
                       options.BlogPostsFolder = @"BlogPosts";
                       options.CategoriesFolder = @"Categories";
                       options.TagsFolder = @"Tags";
                   } );

        // Add Authentication
        _ = builder.Services
                   .AddAuth0WebAppAuthentication( options =>
                   {
                       options.Domain = builder.Configuration.GetValue( "Auth0:Domain", string.Empty );
                       options.ClientId = builder.Configuration.GetValue( "Auth0:ClientId", string.Empty );
                       options.ClientSecret = builder.Configuration.GetValue( "Auth0:ClientSecret", string.Empty );
                   } );

        // Add services to the container.
        _ = builder.Services.AddRazorPages();
        _ = builder.Services.AddServerSideBlazor();

        _ = builder.Services.AddScoped<IBlogApi, BlogApiJsonDirectAccess>();
        _ = builder.Services.AddTransient<ILoginStatus, LoginStatus>();

        WebApplication app = builder.Build();

        // Configure the HTTP request pipeline.
        if( app.Environment.IsDevelopment() == false )
        {
            _ = app.UseExceptionHandler( "/Error" );

            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            _ = app.UseHsts();
        }

        _ = app.UseHttpsRedirection();

        _ = app.UseStaticFiles();

        _ = app.UseRouting();
        _ = app.UseAuthentication();
        _ = app.UseAuthorization();

        _ = app.MapBlazorHub();
        _ = app.MapFallbackToPage( "/_Host" );

        // Use Minimal API endpoints to login and logout.
        _ = app.MapGet( "authentication/login", async ( string redirectUri, HttpContext context ) =>
        {
            AuthenticationProperties? authenticationProperties = new LoginAuthenticationPropertiesBuilder()
                .WithRedirectUri( redirectUri )
                .Build();

            await context.ChallengeAsync( Auth0Constants.AuthenticationScheme, authenticationProperties )
                         .ConfigureAwait( false );
        } );

        _ = app.MapGet( "authentication/logout", async ( HttpContext context ) =>
        {
            AuthenticationProperties? authenticationProperties = new LogoutAuthenticationPropertiesBuilder()
                .WithRedirectUri( "/" )
                .Build();

            await context.SignOutAsync( Auth0Constants.AuthenticationScheme, authenticationProperties )
                         .ConfigureAwait( false );
            await context.SignOutAsync( CookieAuthenticationDefaults.AuthenticationScheme )
                         .ConfigureAwait( false );
        } );

        app.Run();
    }
}
