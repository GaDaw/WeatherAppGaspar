using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherAppGaspar.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }

        [JsonIgnore]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [JsonIgnore]
        public string FullName { get; set; }

        [JsonIgnore]
        public string Dni { get; set; }

        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Meteorology { get; set; }
        public string Forecast { get; set; }

        

        
    }
}
