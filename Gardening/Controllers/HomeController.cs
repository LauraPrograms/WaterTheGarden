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
    public class HomeController : Controller
    {
        //reference for later in getting soil temperature predictions https://www.ncdc.noaa.gov/cdo-web/api/v2/datatypes?offset=500&limit=500
        //reference to explain local codes https://www.ncdc.noaa.gov/cdo-web/api/v2/datacategories?locationid=ZIP:48242
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult RecentObservations()
        {
            string API = "https://api.weather.gov/stations/KONZ/observations/latest";
            HttpWebRequest request = WebRequest.CreateHttp(API);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader rd = new StreamReader(response.GetResponseStream());
            string data = rd.ReadToEnd();
            rd.Close();
            string test;

            return View();
        }

        //will become it's own controller later once the database is made
        //database will need to gather 5 years of data for March-November.
        //build a predicting model of the 
        public ActionResult PastObservations()
        {
            List<NOAAWeather> ChosenOnes = new List<NOAAWeather>();

            APIKeyClass keys = new APIKeyClass();
            string NOAAkey = keys.NKey;
            string locationid = keys.locationA; //can change to other locations

            for (int year = 2018; year <= DateTime.Today.Year; year++)
            {
                //this is a gardening app for watering, I don't need to water past November or before March
                for (int month = 3; month <= 11; month++)
                {
                    if(month==4 && year==2019)
                    {
                        break;
                    }
                    string monthstring = "0" + month;
                    monthstring = monthstring.Substring(monthstring.Length - 2, 2);

                    string startDate = year + "-" + monthstring + "-01";
                    //string endofMonth = GetDate(month);
                    string endDate = year + "-" + monthstring + "-" + GetDate(month);

                    //GHCND is the daily summaries
                    string NOAAAPI = "https://www.ncdc.noaa.gov/cdo-web/api/v2/data?datasetid=GHCND&" + "locationid=" + locationid + "&startdate=" + startDate + "&enddate=" + endDate;
                    HttpWebRequest WR = WebRequest.CreateHttp(NOAAAPI);
                    WR.Headers.Add("token", NOAAkey);
                    HttpWebResponse Response = (HttpWebResponse)WR.GetResponse();
                    StreamReader rd = new StreamReader(Response.GetResponseStream());
                    string data = rd.ReadToEnd();
                    rd.Close();

                    MetaWrapper AllEvents = JsonConvert.DeserializeObject<MetaWrapper>(data);
                    ChosenOnes.AddRange(AllEvents.results.ToList());
                }

            }
            ViewBag.Results = ChosenOnes;
            return View(ChosenOnes);
        }


    public string GetDate(int month)
    {
        string endOfMonth;
        if (month == 3 || month == 5 || month == 7 || month == 8 || month == 10)
        {
            return endOfMonth = "31";
        }
        else
        {
            return endOfMonth = "30";
        }
    }



        public string 

}
}