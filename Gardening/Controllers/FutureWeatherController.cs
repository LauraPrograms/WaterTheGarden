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
            string UAgent = api.agent;
            string UAccept = api.accept;

            HttpWebRequest request = (HttpWebRequest)WebRequest.CreateHttp(ForcastAPI);
            request.UserAgent = UAgent;
            request.Accept = UAccept;
           
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader rd = new StreamReader(response.GetResponseStream());
            string data = rd.ReadToEnd();
            rd.Close();

            List<Forecast> Weekly = new List<Forecast>();

            MetaPeriods WeeklyForecast = JsonConvert.DeserializeObject<MetaPeriods>(data);
            Weekly.AddRange(WeeklyForecast.periods.ToList());
            ViewBag.Results = Weekly;
            return View();
        }
    }
}