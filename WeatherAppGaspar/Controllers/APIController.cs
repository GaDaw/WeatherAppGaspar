using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WeatherAppGaspar.Data;
using WeatherAppGaspar.Models;

namespace WeatherAppGaspar.Controllers
{
    [Produces("application/json")]
    [Route("/API")]
    public class APIController : Controller
    {
        private readonly ApplicationDbContext _context;

        public APIController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/ApiWeather
        [HttpGet]
        // Esta API muestra los nombres de usuarios, coordenadas, metereología y forecast. El resto de campos no se serializan tal como está definido el modelo User
        public IEnumerable<User> GetUserWeather()
        {
            return _context.User;
        }

        // GET: api/ApiWeather/5
        [HttpGet("{id}")]

        public async Task<IActionResult> GetUserWeather([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userWeather = await _context.User.SingleOrDefaultAsync(m => m.Id == id);

            if (userWeather == null)
            {
                return NotFound();
            }

            return Ok(userWeather);
        }
    }
}
