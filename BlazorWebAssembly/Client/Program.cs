using System.Reflection;

using Components.RazorComponents;

using Data.Models.Interfaces;

using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration.UserSecrets;

namespace BlazorWebAssembly.Client;

public static class Program
{
    public static async Task Main( string[] args )
    {
        WebAssemblyHostBuilder builder = WebAssemblyHostBuilder.CreateDefault( args );
        builder.RootComponents.Add<App>( "#app" );
        builder.RootComponents.Add<HeadOutlet>( "head::after" );

        _ = builder.Services.AddScoped( sp => new HttpClient
        {
            BaseAddress = new Uri( builder.HostEnvironment.BaseAddress )
        } );

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

        _ = builder.Services.AddHttpClient( "Public",
                                    client => client.BaseAddress = new Uri( builder.HostEnvironment.BaseAddress ) );
        _ = builder.Services.AddHttpClient( "Authenticated",
                                    client => client.BaseAddress = new Uri( builder.HostEnvironment.BaseAddress ) )
                            .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

        _ = builder.Services.AddOidcAuthentication( options =>
        {
            builder.Configuration.Bind( "Auth0", options.ProviderOptions );
            options.ProviderOptions.ResponseType = "code";
            options.ProviderOptions.AdditionalProviderParameters.Add( "audience", builder.Configuration["Auth0:Audience"] );
        } );

        _ = builder.Services.AddTransient<IBlogApi, BlogApiWebClient>();
        _ = builder.Services.AddTransient<ILoginStatus, LoginStatusWasm>();

        await builder.Build().RunAsync().ConfigureAwait( false );
    }
}
