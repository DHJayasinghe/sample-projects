using Microsoft.AspNetCore.Mvc;

namespace servicebus_demo.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController(ServiceBusReceiverService serviceBus) : ControllerBase
{
    private static readonly string[] Summaries =
    [
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    ];

    private readonly ServiceBusReceiverService _serviceBus = serviceBus;

    [HttpGet]
    public IEnumerable<WeatherForecast> Get()
    {
        var forecast = Enumerable.Range(1, 5).Select(index => new WeatherForecast(DateOnly.FromDateTime(DateTime.Now.AddDays(index)), Random.Shared.Next(-20, 55), Summaries[Random.Shared.Next(Summaries.Length)]));

        if (_serviceBus.CustomWeatherMessage is not null)
            forecast = forecast.Append(_serviceBus.CustomWeatherMessage);

        return forecast;
    }
}