using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherAppGaspar.Models
{
    public class DeserializeExternalApi
    {
        public string Name { get; set; }
        public Main Main { get; set; }
    }

    public class Main
    {
        public string Temp { get; set; }
        public string Pressure { get; set; }
        public string Humidity { get; set; }
    }
}

