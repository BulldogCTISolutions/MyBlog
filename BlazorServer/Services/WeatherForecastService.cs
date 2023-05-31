using System.Security.Cryptography;

using BlazorWebAssembly.Common;

namespace BlazorServer.Services;

public class WeatherForecastService
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    public static Task<WeatherForecast[]> GetForecastAsync( DateOnly startDate ) =>
        Task.FromResult( Enumerable.Range( 1, 5 ).Select( index => new WeatherForecast
        {
            Date = startDate.AddDays( index ),
            TemperatureC = RandomNumberGenerator.GetInt32( -20, 55 ),
            Summary = Summaries[RandomNumberGenerator.GetInt32( Summaries.Length )]
        } ).ToArray() );
}
