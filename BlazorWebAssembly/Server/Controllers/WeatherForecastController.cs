using BlazorWebAssembly.Common;

using Microsoft.AspNetCore.Mvc;

namespace BlazorWebAssembly.Server.Controllers;
[ApiController]
[Route( "[controller]" )]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController( ILogger<WeatherForecastController> logger ) => this._logger = logger;

    [HttpGet]
    public IEnumerable<WeatherForecast> Get() => Enumerable.Range( 1, 5 ).Select( index => new WeatherForecast
    {
        Date = DateOnly.FromDateTime( DateTime.Now.AddDays( index ) ),
        TemperatureC = System.Security.Cryptography.RandomNumberGenerator.GetInt32( -20, 55 ),
        Summary = Summaries[System.Security.Cryptography.RandomNumberGenerator.GetInt32( Summaries.Length )]
    } ).ToArray();
}
