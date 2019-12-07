using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace ChartsDemo.Models
{
    public class Weatherforecast
    {
        public DateTime Date { get; set; }

        public string Day => Date.ToString("dddd");

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string Summary { get; set; }

        public override string ToString() => JsonSerializer.Serialize<Weatherforecast>(this);
    }
}
