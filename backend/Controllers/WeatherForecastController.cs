using System.Data;
using Microsoft.AspNetCore.Mvc;

namespace CSAProjectReview.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }   

    [HttpGet(Name = "GetWeatherForecast")]
       public IEnumerable<WeatherForecast> Get()
       {

        return Enumerable.Range(1, 1).Select(index => new WeatherForecast
        {
            Stadt = "Berlin",
            Datum = DateTime.Now.AddDays(index),
            Temperatur = 16
        }).ToArray();
           
}
}
