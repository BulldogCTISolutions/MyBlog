using System.Net;

using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

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
            BaseAddress = new Uri( builder.HostEnvironment.BaseAddress ),
            DefaultRequestVersion = HttpVersion.Version30,
            DefaultVersionPolicy = HttpVersionPolicy.RequestVersionExact
        } );

        await builder.Build().RunAsync().ConfigureAwait( false );
    }
}
