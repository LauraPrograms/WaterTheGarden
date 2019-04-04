using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gardening.Models
{
    public class NOAAWeather
    {
        public string date { get; set; }
        public string datatype { get; set; }
        public string station { get; set; }
        public string attributes { get; set; }
        public string value { get; set; }
    }

    public class MetaWrapper
    {
        public List<NOAAWeather> results { get; set; }
    }

    public class datatype
    {
        public string id { get; set; }
        public string name { get; set; }

    }

}