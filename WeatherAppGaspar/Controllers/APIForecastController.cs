using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace WeatherAppGaspar.Controllers
{
    
    [Produces("application/json")]
    [Route("API/Forecast")]
    public class APIForecastController : Controller
    {
        [HttpGet("[action]/{latitude}/{longitude}")]
        public async Task<IActionResult> coordinates(string latitude, string longitude)

        {
            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri("http://api.openweathermap.org");
                    var response = await client.GetAsync($"/data/2.5/forecast?lat={latitude}&lon={longitude}&units=metric&APPID=747935212617bea48b0bbb48e8f6a455");
                    response.EnsureSuccessStatusCode();

                    var stringResult = await response.Content.ReadAsStringAsync();
                    var rawWeather = JsonConvert.DeserializeObject<ForecastResponse>(stringResult);
                    return Json(new
                    {
                        city = rawWeather.City.Name,
                        country = rawWeather.City.Country,
                        Forec = rawWeather.List

                    });
                }
                catch (HttpRequestException httpRequestException)
                {
                    return BadRequest($"Error getting weather from OpenWeather: {httpRequestException.Message}");
                }
            }
        }

    }
    public class ForecastResponse
    {

        public ICollection<T> List { get; set; }
        public FMain City { get; set; }
    }
    public class T
    {
        public object Main { get; set; }
        public string Dt_txt { get; set; }
    }


    public class FMain
    {
        public string Name { get; set; }
        public string Country { get; set; }

    }
}
