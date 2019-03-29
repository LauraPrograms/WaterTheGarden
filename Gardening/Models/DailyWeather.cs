using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gardening.Models
{
    public class DailyWeather
    {
        public int eventNumber { get; set; }
        public string name { get; set; }
        public string weather { get; set; }
        public string waterPlants { get; set; }
    }
}