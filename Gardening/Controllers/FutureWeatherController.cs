using Gardening.APIKeys;
using Gardening.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Gardening.Controllers
{
    public class FutureWeatherController : Controller
    {
        // GET: FutureWeather
        public ActionResult Rain()
        {
            APIKeyClass api = new APIKeyClass();
            string ForcastAPI = api.APIForecast;
            HttpWebRequest request = WebRequest.CreateHttp(ForcastAPI);

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader rd = new StreamReader(response.GetResponseStream());
            string data = rd.ReadToEnd();
            rd.Close();

            MetaPeriods WeeklyForecast = JsonConvert.DeserializeObject<MetaPeriods>(data);
            ViewBag.Results = WeeklyForecast;
            return View();
        }
    }
}