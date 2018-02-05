using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WeatherAppGaspar.Data;
using WeatherAppGaspar.Models;

namespace WeatherAppGaspar.Controllers
{
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            return View(await _context.User.ToListAsync());
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .SingleOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Username,Password,FullName,Dni,Latitude,Longitude,Meteorology,Forecast")] User user)
        {
            if (ModelState.IsValid)
            {

                // Hacemos una llamada a la Api de Openweather obteniendo datos metereologicos para las coordenadas dadas y las guardamos algunos en el campo Metereology del usuario
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://api.openweathermap.org");
                    var response = await client.GetAsync($"/data/2.5/weather?lat={user.Latitude}&lon={user.Longitude}&units=metric&APPID=747935212617bea48b0bbb48e8f6a455");
                    response.EnsureSuccessStatusCode();

                    var stringResult = await response.Content.ReadAsStringAsync();
                    var rawWeather = JsonConvert.DeserializeObject<DeserializeExternalApi>(stringResult);
                    user.Meteorology = ("Ciudad: " + rawWeather.Name + "  Temperatura: " + rawWeather.Main.Temp + "  Presión: " + rawWeather.Main.Pressure + "  Humidity: " + rawWeather.Main.Humidity);

                }

                // Creamos una url a nuestra Api Forecast (que toma datos de OpenWeather) para mostrar la Prediccion para el usuario
                var urlforecast = ("https://localhost:44335/api/forecast/coordinates/" + user.Latitude + "/" + user.Longitude);
                user.Forecast = urlforecast;


                // Hasheamos la contraseña dada antes de guardarla
                string password = user.Password;
                byte[] salt = new byte[128 / 8];
                using (var rng = RandomNumberGenerator.Create())
                {
                    rng.GetBytes(salt);
                }
                string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));
                user.Password = hashed;

                // Guardamos el usuario
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User.SingleOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Username,Password,FullName,Dni,Latitude,Longitude,Meteorology,Forecast")] User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Hacemos una llamada a la Api de Openweather obteniendo datos metereologicos para las coordenadas dadas y las guardamos algunos en el campo Metereology del usuario
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri("http://api.openweathermap.org");
                        var response = await client.GetAsync($"/data/2.5/weather?lat={user.Latitude}&lon={user.Longitude}&units=metric&APPID=747935212617bea48b0bbb48e8f6a455");
                        response.EnsureSuccessStatusCode();

                        var stringResult = await response.Content.ReadAsStringAsync();
                        var rawWeather = JsonConvert.DeserializeObject<DeserializeExternalApi>(stringResult);
                        user.Meteorology = ("Ciudad: " + rawWeather.Name + "  Temperatura: " + rawWeather.Main.Temp + "  Presión: " + rawWeather.Main.Pressure + "  Humidity: " + rawWeather.Main.Humidity);

                    }

                    // Hasheamos la contraseña dada antes de guardarla
                    string password = user.Password;
                    byte[] salt = new byte[128 / 8];
                    using (var rng = RandomNumberGenerator.Create())
                    {
                        rng.GetBytes(salt);
                    }
                    string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: password,
                    salt: salt,
                    prf: KeyDerivationPrf.HMACSHA1,
                    iterationCount: 10000,
                    numBytesRequested: 256 / 8));
                    user.Password = hashed;

                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .SingleOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.User.SingleOrDefaultAsync(m => m.Id == id);
            _context.User.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.User.Any(e => e.Id == id);
        }
    }
}
