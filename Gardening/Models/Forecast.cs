using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gardening.Models
{
    public class Forecast
    {
        public string number { get; set; }
        public int EventNumber { get { return int.Parse(number); } }
        public string name { get; set; }
        public string detailedForecast { get; set; }
    }

    public class MetaPeriods
    {
        public List<Forecast> periods { get; set; }
    }
}