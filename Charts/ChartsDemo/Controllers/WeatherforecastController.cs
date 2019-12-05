using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChartsDemo.Models;
using ChartsDemo.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChartsDemo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class WeatherforecastController : ControllerBase
    {
        public WeatherforecastController(JsonFileWeatherService weatherService)
        {
            this.WeatherService = weatherService;
        }

        public JsonFileWeatherService WeatherService { get; set; }

        [HttpGet]
        public IEnumerable<Weatherforecast> GetWeatherforecasts()
        {
            return WeatherService.GetWeatherforecasts();
        }
    }
}