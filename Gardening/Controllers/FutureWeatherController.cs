using Gardening.APIKeys;
using Gardening.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Gardening.Controllers
{
    public class FutureWeatherController : Controller
    {
        // GET: FutureWeather
        public ActionResult WeeklyWeather()
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

            //make a method to determine if there is rain
            List<string> TestMethodList = DaysOfRain(Weekly);


            ViewBag.Results = Weekly;
            return View();
        }
        //make a list of key words to figure out how to parse the various words (rain, precipitation, showers) and get the percent
        public List<string> DaysOfRain(List<Forecast> forecasts)
        {
            List<string> RainWords = new List<string>() { "rain", "shower", "precipitation" };
            List<string> RainyDays = new List<string>();
            foreach (Forecast Event in forecasts)
            {
                string[] EventWeatherList = Event.detailedForecast.ToLower().Split('.');
                for (int i = 0; i < RainWords.Count(); i++)
                {
                    List<string> wetDay = EventWeatherList.Where(x => x.Contains(RainWords[i])
                                                            && x.Contains("percent")).ToList();
                    foreach(string wetEvent in wetDay)
                    {
                        char[] rainChance = wetEvent.Where(Char.IsDigit).ToArray();
                        string percentage = String.Join("", rainChance);
                        int rainPercent = int.Parse(percentage);
                    }
                    
                    //int[] percent = Array.ConvertAll(rainChance, c => (int)Char.GetNumericValue(c));
                   
                   
                }

            }
            //foreach (string weather in EventWeatherList)
            //{
            //    ///use linq check if the even has the key words (store in a list of strings) and then build a new object that requires the percent, day name, and add the date based off current date, and max temperature

            //}


            return RainyDays;
        }
    }

}