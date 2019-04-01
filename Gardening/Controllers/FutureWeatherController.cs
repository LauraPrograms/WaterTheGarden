using Gardening.APIKeys;
using Gardening.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
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
            List<DailyWeather> WetDays = DaysOfRain(Weekly);
            if (WetDays != null)
            {
                ViewBag.Waterdays = WetDays;
            }
            else
            {
                ViewBag.NeedWater = "Its going to be dry for the next week.";
            }
            List<Forecast> DryDays = (List<Forecast>)Session["forecasts"];
            ViewBag.Results = DryDays;
            return View();
        }
        
        public List<DailyWeather> DaysOfRain(List<Forecast> forecasts)
        {
            List<string> RainWords = new List<string>() { "rain", "shower", "precipitation" };
            List<DailyWeather> RainyDays = new List<DailyWeather>();
            int rainPercent;
            
            foreach (Forecast Event in forecasts)
            {
                string[] EventWeatherList = Event.detailedForecast.ToLower().Split('.');
                for (int i = 0; i < RainWords.Count(); i++)
                {
                    List<string> wetDay = EventWeatherList.Where(x => x.Contains(RainWords[i])
                                                            && x.Contains("percent")).ToList();
                    if (wetDay != null)
                    {
                        foreach (string wetEvent in wetDay)
                        {
                            DailyWeather dw = new DailyWeather();
                            char[] rainChance = wetEvent.Where(Char.IsDigit).ToArray();
                            string percentage = String.Join("", rainChance);
                            rainPercent = int.Parse(percentage);
                            dw.eventNumber = Event.EventNumber;
                            dw.name = Event.name;
                            dw.temperature = GetTemperature(EventWeatherList);
                            dw.weather = "Chance of precipitation";
                            if (rainPercent >= 50)
                            {
                                dw.waterPlants = "No";
                            }
                            else if (rainPercent < 50 && rainPercent > 30)
                            {
                                dw.waterPlants = "Check the hygrometer";
                            }
                            else
                            {
                                dw.waterPlants = "Yes";
                            }
                            RainyDays.Add(dw);
                        }
                    }

                }

            }            
            for (int i = 0; i < RainyDays.Count(); i++)
            {
                forecasts.RemoveAll(e => e.EventNumber == RainyDays[i].eventNumber);
            }
            Session["forecasts"] = forecasts;
            return RainyDays;
        }

        public int GetTemperature(string[] EventWeatherList)
        {
            int temperature=0;
            List<string> eventTemperature = EventWeatherList.Where(x => x.Contains("highs") || x.Contains("lows")).ToList();
            foreach(string eventTemp in eventTemperature)
            {  string result = Regex.Match(eventTemp, @"\d+").Value;
                temperature = int.Parse(result);
            }
            return temperature;
        }
    }

}